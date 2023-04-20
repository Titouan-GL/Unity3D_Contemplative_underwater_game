using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateFishes : MonoBehaviour
{
    public Transform anchor;
    [SerializeField]private float transformPosSpeed = 0.1f;
    [SerializeField]private float transformRotSpeed = 0.1f;
    [SerializeField]private float maxDistortion = 1f;
    [SerializeField]private bool initiator = false;
    private float maxDistanceToAnchor;
    private float minDistanceToAnchor;
    private Transform m_MainCameraTransform;
    //[SerializeField] private int offset = 100;

    // Start is called before the first frame update
    void Start()
    {
        m_MainCameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        int cc = transform.childCount;
        for(int i = 0; i < cc; i++){
            var go = transform.GetChild(0).gameObject;
            if(go.GetComponent<InconsequencialMovement>() == null){
                if(go.GetComponent<AnimateFishes>() == null){
                    go.AddComponent<AnimateFishes>();
                    go.GetComponent<AnimateFishes>().SetTransformPosSpeed(transformPosSpeed);
                    go.GetComponent<AnimateFishes>().SetTransformRotSpeed(transformRotSpeed);
                    go.GetComponent<AnimateFishes>().SetMaxDistortion(maxDistortion);
                }
                if(go.GetComponent<AnimateFishes>().anchor == null){
                    go.transform.parent = transform.parent;

                    GameObject anchorpoint = new GameObject(go.name + " anchor point");
                    go.GetComponent<AnimateFishes>().SetAnchorPoint(anchorpoint.transform);
                    anchorpoint.transform.parent = transform;
                    anchorpoint.transform.position = go.transform.position;
                    anchorpoint.transform.rotation = go.transform.rotation;
                    //Debug.Log(anchorpoint.transform.position + " " +transform.position + " " + (Vector3.Distance(anchorpoint.transform.position, transform.position)* maxDistortion));
                    go.GetComponent<AnimateFishes>().SetMaxDistance(Vector3.Distance(anchorpoint.transform.position, transform.position)* go.GetComponent<AnimateFishes>().GetMaxDistortion());
                    go.GetComponent<AnimateFishes>().SetMinDistance(Vector3.Distance(anchorpoint.transform.position, transform.position)/ go.GetComponent<AnimateFishes>().GetMaxDistortion());
                }
                else{
                    go.transform.parent = transform.parent;
                }

                if(initiator){
                    SetMaxDistance(Vector3.Distance(anchor.parent.transform.position, transform.position)* maxDistortion);
                    SetMinDistance(Vector3.Distance(anchor.parent.transform.position, transform.position)* maxDistortion);
                }
            }
        }
    }

    public float GetMaxDistortion(){
        return maxDistortion;
    }

    public void SetMaxDistortion(float md){
        maxDistortion = md;
    }

    public void SetTransformPosSpeed(float s){
        transformPosSpeed = s;
    }

    public void SetTransformRotSpeed(float s){
        transformRotSpeed = s;
    }

    public void SetAnchorPoint(Transform anchorpoint){
        anchor = anchorpoint;
    }

    public void SetMaxDistance(float distance){
        maxDistanceToAnchor = distance;
    }

    public void SetMinDistance(float distance){
        minDistanceToAnchor = distance;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(anchor != null){
            //transform.position = Vector3.Lerp(transform.position, anchor.position, transformPosSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, anchor.rotation, transformRotSpeed);

            /*if(initiator == false && initiator == true){
                transform.position = anchor.parent.transform.position + (anchor.parent.transform.up) * 
                    Vector3.Distance(anchor.parent.transform.position, anchor.transform.position);
            }
            else{*/
                transform.position = anchor.position;
            //}
            // Move the GameObject to the new position


        }
    }

}
