using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperBoidManager : MonoBehaviour
{
    struct boidStruct{
        public Vector3 position;
        public Vector3 velocity;
        public Vector3 debug;
    };
    boidStruct[] boidArray;

    struct schoolStruct{
        public Vector3 target;
        public float zoneRepulsion;
        public float zoneAlignement;
        public float zoneAttraction;
        public float forceRepulsion;
        public float forceAlignement;
        public float forceAttraction;
        public float forceTarget;
        public float maxSpeed;
        public float minSpeed;
        public int nbBoids;
    };
    schoolStruct[] schoolArray;

    public ComputeShader compute;

    public List<NewBoidManager> boidManagerList;
    public List<Transform> boidList;

    private int kernel;
    ComputeBuffer boidBuffer;
    ComputeBuffer boidManagerBuffer;

    private int totalBoidsNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        schoolArray = new schoolStruct[boidManagerList.Count]; //j'initialise l'array de bancs

        int index = 0;
        foreach(NewBoidManager bm in boidManagerList){//j'attribue des valeurs pour toutes les valeurs de l'array de bancs
            totalBoidsNumber += (int)Mathf.Ceil(bm.trueNbBoids * FaunaSlider.faunaDensity);
            schoolArray[index].zoneRepulsion = bm.zoneRepulsion;
            schoolArray[index].zoneAlignement = bm.zoneAlignement;
            schoolArray[index].zoneAttraction = bm.zoneAttraction;
            schoolArray[index].forceRepulsion = bm.forceRepulsion;
            schoolArray[index].forceAlignement = bm.forceAlignement;
            schoolArray[index].forceAttraction = bm.forceAttraction;
            schoolArray[index].forceTarget = bm.forceTarget;
            schoolArray[index].maxSpeed = bm.maxSpeed;
            schoolArray[index].minSpeed = bm.minSpeed;
            schoolArray[index].nbBoids = (int)Mathf.Ceil(bm.trueNbBoids * FaunaSlider.faunaDensity);
            index++;
        }

        boidArray = new boidStruct[totalBoidsNumber];//cela me permet d'initialiser l'array de boids

        index = 0;
        foreach(NewBoidManager bm in boidManagerList){//je peux donc attribuer les valeurs de l'array de bancs
            for(int j = 0; j < (int)Mathf.Ceil(bm.trueNbBoids * FaunaSlider.faunaDensity); j ++){
                GameObject go = GameObject.Instantiate<GameObject>(bm.prefabBoid, bm.transform.position, Quaternion.identity);
                Vector3 positionBoid = bm.transform.position + UnityEngine.Random.insideUnitSphere * 1;
                go.transform.position = positionBoid;
                boidList.Add(go.transform);
                boidArray[index].position = positionBoid;
                boidArray[index].velocity = (positionBoid - bm.transform.position).normalized * 10;
                index ++;
            }
        }


        kernel = compute.FindKernel("CSMain");
        boidBuffer = new ComputeBuffer(totalBoidsNumber, sizeof(float) * 9);
        boidManagerBuffer = new ComputeBuffer(boidManagerList.Count, sizeof(float) * 12 + sizeof(int));
        compute.SetBuffer(kernel, "BoidBuffer", boidBuffer);
        compute.SetBuffer(kernel, "SchoolBuffer", boidManagerBuffer);
        compute.SetInt("nbSchools", boidManagerList.Count);
        boidBuffer.SetData(boidArray);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        compute.SetFloat("deltaTime", Time.fixedDeltaTime);
        int index = 0;
        for(int i = 0; i < boidManagerList.Count; i ++){
            schoolArray[i].target = boidManagerList[i].target.position;
            compute.SetInt("schoolIndex", i);
            compute.SetInt("schoolStartIndex", index);
            compute.Dispatch(kernel, boidManagerList[i].nbBoids, 1, 1);
            index += boidManagerList[i].nbBoids;

        }
        
        //boidBuffer.SetData(boidArray);
        boidManagerBuffer.SetData(schoolArray);
        boidBuffer.GetData(boidArray);

        for(int i = 0; i < totalBoidsNumber; i ++){
            boidList[i].position = boidArray[i].position;
            if (boidArray[i].velocity.sqrMagnitude > 0)
                boidList[i].LookAt(boidList[i].position + boidArray[i].velocity);
            if(float.IsNaN(boidArray[i].velocity.x)){
                Debug.Log(boidArray[i].debug + " " + boidArray[i].velocity+ " " +i);
            }
        }
    }
}
