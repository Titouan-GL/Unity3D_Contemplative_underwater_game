using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NewBoidManager : MonoBehaviour
{
    /*struct boidStruct{
        public Vector3 position;
        public Vector3 velocity;
        public Vector3 debug;
    };*/


    public int trueNbBoids = 100;
    public int nbBoids = 100;
    public GameObject prefabBoid;


    //public ComputeShader compute;
    


    public float zoneRepulsion = 10;
    public float zoneAlignement = 20;
    public float zoneAttraction = 30;
    public float forceRepulsion = 30;
    public float forceAlignement = 30;
    public float forceAttraction = 30;
    public float forceTarget = 20;
    public float maxSpeed = 10;
    public float minSpeed = 5;
    public Transform target;

    public List<Transform> boidList = new List<Transform>();
    //private int kernel;
    //boidStruct[] boidArray;
    //ComputeBuffer boidBuffer;

    void Start()
    {
        nbBoids = (int)Mathf.Ceil(trueNbBoids * FaunaSlider.faunaDensity);
        if (target == null){
            target = transform;
        }
        //boidArray = new boidStruct[nbBoids];
        /*
        for (int i = 0; i < nbBoids; i++)
        {
            GameObject go = GameObject.Instantiate<GameObject>(prefabBoid, transform.position, Quaternion.identity);
            Vector3 positionBoid = transform.position + UnityEngine.Random.insideUnitSphere * 1;
            go.transform.position = positionBoid;
            boidList.Add(go.transform);

            //boidArray[i].position = boidList[i].position;
            //boidArray[i].velocity = (positionBoid - transform.position).normalized * 10;
        }*/



        //kernel = compute.FindKernel("CSMain");
        //boidBuffer = new ComputeBuffer(nbBoids, sizeof(float) * 9);

    }

    void FixedUpdate()
    {        /*
        compute.SetFloat("zoneRepulsion", zoneRepulsion);
        compute.SetFloat("zoneAlignement", zoneAlignement);
        compute.SetFloat("zoneAttraction", zoneAttraction);
        compute.SetFloat("forceRepulsion", forceRepulsion);
        compute.SetFloat("forceAlignement", forceAlignement);
        compute.SetFloat("forceAttraction", forceAttraction);
        compute.SetFloat("forceTarget", forceTarget);
        compute.SetFloat("maxSpeed", maxSpeed);
        compute.SetFloat("minSpeed", minSpeed);
        compute.SetFloat("deltaTime", Time.fixedDeltaTime);
        compute.SetFloat("targetx", target.position.x);
        compute.SetFloat("targety", target.position.y);
        compute.SetFloat("targetz", target.position.z);
        compute.SetInt("nbBoids", (int)Math.Min((int)nbBoids, (int)FaunaSlider.maxBoidPrecision));



        // Define an array of Boid structs to initialize the buffer

        // Set the position and velocity of each boid in the array
        for (int i = 0; i < nbBoids; i++)
        {
            boidArray[i].position = boidList[i].position;
        }

        // Set the data of the buffer to the array
        boidBuffer.SetData(boidArray);

        // Pass the buffer to the compute shader
        compute.SetBuffer(kernel, "BoidBuffer", boidBuffer);


        compute.Dispatch(kernel, 1, nbBoids, 1);
        
        boidBuffer.GetData(boidArray);

        for (int i = 0; i < nbBoids; i++)
        {
            boidList[i].position = boidArray[i].position;
            if (boidArray[i].velocity.sqrMagnitude > 0)
                boidList[i].LookAt(boidList[i].position + boidArray[i].velocity);
            if(float.IsNaN(boidArray[i].velocity.x)){
                Debug.Log(boidArray[i].debug + " " + boidArray[i].velocity+ " " +i);
            }
        }*/
    }
}
