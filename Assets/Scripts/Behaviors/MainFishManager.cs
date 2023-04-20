using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainFishManager : MonoBehaviour
{
    private float m_speed = 10.0f ;
    private float m_speedMin = 0f ;
    private float m_effectiveSpeed = 0.0f ;
    private float acceleration_speed = 4f;
    private float turnSpeed = 200f;
    private Rigidbody m_Rigidbody;
    private Transform m_transform;
    private int horizontal_movement_value;
    private int vertical_movement_value;
    private int acceleration_value;
    [SerializeField] private LayerMask layerMaskEnvironment;
    [SerializeField] private LayerMask layerMaskPickable;
    private int distanceMinFromWall = 1;
    private int distanceStartDeceleration = 3;
    private int grabbingSteps = 0;
    private Pickable grabbed;
    private Pickable futureGrabbed;
    private Vector3 grabbingPosition;
    private bool buttonGrab;
    private Vector3 m_rotation = new Vector3(0, 0, 0);
    private float food = 40;
    //private float maxFood = 40;

    //[SerializeField] private BoidManager boidManager;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponentInChildren<Rigidbody>();
        m_transform = GetComponentInChildren<Transform>();
        //layerMaskEnvironment = LayerMask.GetMask("Environment");
        //layerMaskEnvironment = LayerMask.GetMask("Pickable");
        layerMaskPickable += LayerMask.GetMask("Pickable");
    }

    // Update is called once per frame
    void LetGoOfGrab(){
        grabbingSteps = 0;
        grabbed.GetTransform().parent = null;
        grabbed.SetKinematic(false);
        grabbed.IsLetDown();
        grabbed.gameObject.layer = LayerMask.NameToLayer("pickable");
    }

    /*void KillFish(){
        if(boidManager.GetBoids().Count > 0){
            boidManager.KillFish();
        }
    }*/

    /*void Update(){
        RaycastHit hit;
        
        buttonGrab = Input.GetButtonDown("Grab");
        if(buttonGrab){
            if(Physics.Raycast(m_transform.position, transform.TransformDirection(Vector3.forward), out hit, 5, layerMaskPickable) && grabbingSteps == 0){
                grabbingSteps = 1;
                grabbingPosition = hit.point;
                futureGrabbed = hit.collider.gameObject.GetComponentInChildren<Pickable>();
                if(futureGrabbed == null){
                    futureGrabbed = hit.collider.gameObject.GetComponentInParent<Pickable>();
                }
            }

            else if (grabbingSteps >= 2){
                LetGoOfGrab();
                grabbed = null;
            }

        }
        
    }*/

    void FixedUpdate()
    {
        /*food -= Time.fixedDeltaTime;
        if(food <= 0){
            KillFish();
            food = 10;
        }*/
        //RaycastHit hit;

        horizontal_movement_value = 0;
        vertical_movement_value = 0;

        if(grabbingSteps  == 0){
            horizontal_movement_value = (int)Input.GetAxis("Horizontal");
            vertical_movement_value = (int)Input.GetAxis("Vertical") *-1;

        }

        if(grabbingSteps < 1 || grabbingSteps > 2){
            //inputs
            acceleration_value = (int)Input.GetAxis("Accelerate");

            if(m_effectiveSpeed <= m_speed && m_effectiveSpeed >= m_speedMin){
                m_effectiveSpeed += acceleration_value * Time.fixedDeltaTime * acceleration_speed;
            }
            else if (m_effectiveSpeed > m_speed){
                m_effectiveSpeed = m_speed;
            }
            else if (m_effectiveSpeed < m_speedMin){
                m_effectiveSpeed = m_speedMin;
            }

            //detect walls
            /*
            if(Physics.Raycast(m_transform.position, transform.TransformDirection(Vector3.forward), out hit, distanceStartDeceleration, layerMaskEnvironment)){
                float m_comparativeSpeed = m_speed*(Math.Max(hit.distance-distanceMinFromWall, 0))/(distanceStartDeceleration-distanceMinFromWall);
                m_effectiveSpeed = Math.Min(m_effectiveSpeed, m_comparativeSpeed);
            }*/


            //Movement

            m_rotation.x += turnSpeed/2 * Time.fixedDeltaTime * vertical_movement_value;
            m_rotation.y += turnSpeed/2 * Time.fixedDeltaTime * horizontal_movement_value;

            RaycastHit hit1;
            RaycastHit hit2;
            RaycastHit hit3;
            RaycastHit hit4;
            /*float maxhitdistance = m_effectiveSpeed;
            Physics.Raycast(transform.position, transform.TransformDirection(new Vector3(0, 0.4f, 1)), out hit1, maxhitdistance, layerMaskEnvironment);//up
            Physics.Raycast(transform.position, transform.TransformDirection(new Vector3(0, -0.4f, 1)), out hit2, maxhitdistance, layerMaskEnvironment);//down
            Physics.Raycast(transform.position, transform.TransformDirection(new Vector3(0.4f, 0, 1)), out hit3, maxhitdistance, layerMaskEnvironment);//right
            Physics.Raycast(transform.position, transform.TransformDirection(new Vector3(-0.4f, 0, 1)), out hit4, maxhitdistance, layerMaskEnvironment);//left
            if(hit1.distance != 0 && (hit1.distance >= hit2.distance)){
                m_rotation.x += (maxhitdistance-hit1.distance);
                m_effectiveSpeed -= (maxhitdistance-hit1.distance) * Time.fixedDeltaTime*3;
            }
            else if(hit2.distance != 0){
                m_rotation.x -= (maxhitdistance-hit2.distance);
                m_effectiveSpeed -= (maxhitdistance-hit2.distance) * Time.fixedDeltaTime*3;
            }
            if(hit3.distance != 0 && (hit3.distance >= hit4.distance)){
                m_rotation.y += (maxhitdistance-hit3.distance);
                m_effectiveSpeed -= (maxhitdistance-hit3.distance) * Time.fixedDeltaTime*3;
            }
            else if(hit3.distance != 0){
                m_rotation.y -= (maxhitdistance-hit3.distance);
                m_effectiveSpeed -= (maxhitdistance-hit4.distance) * Time.fixedDeltaTime*3;
            }*/
            
            if(Physics.Raycast(transform.position, transform.TransformDirection(new Vector3(0, 0.3f, 1)), out hit1, distanceStartDeceleration, layerMaskEnvironment)){
                float m_comparativeSpeed = m_speed*(Math.Max(hit1.distance-distanceMinFromWall, 0))/(distanceStartDeceleration-distanceMinFromWall);
                m_effectiveSpeed = Math.Min(m_effectiveSpeed, m_comparativeSpeed);
                //Debug.DrawLine(transform.position, transform.position+transform.TransformDirection(new Vector3(0, 0.3f, 1))* distanceStartDeceleration, Color.blue);
            }
            //else{Debug.DrawLine(transform.position, transform.position+transform.TransformDirection(new Vector3(0, 0.3f, 1))* distanceStartDeceleration);}

            if(Physics.Raycast(transform.position, transform.TransformDirection(new Vector3(0, -0.3f, 1)), out hit2, distanceStartDeceleration, layerMaskEnvironment)){
                float m_comparativeSpeed = m_speed*(Math.Max(hit2.distance-distanceMinFromWall, 0))/(distanceStartDeceleration-distanceMinFromWall);
                m_effectiveSpeed = Math.Min(m_effectiveSpeed, m_comparativeSpeed);
                //Debug.DrawLine(transform.position, transform.position+transform.TransformDirection(new Vector3(0, -0.3f, 1))* distanceStartDeceleration, Color.blue);
            }
            //else{Debug.DrawLine(transform.position, transform.position+transform.TransformDirection(new Vector3(0, -0.3f, 1))* distanceStartDeceleration);}

            if(Physics.Raycast(transform.position, transform.TransformDirection(new Vector3(0.3f, 0, 1)), out hit3, distanceStartDeceleration, layerMaskEnvironment)){
                float m_comparativeSpeed = m_speed*(Math.Max(hit3.distance-distanceMinFromWall, 0))/(distanceStartDeceleration-distanceMinFromWall);
                m_effectiveSpeed = Math.Min(m_effectiveSpeed, m_comparativeSpeed);
                //Debug.DrawLine(transform.position, transform.position+transform.TransformDirection(new Vector3(0.3f, 0, 1))* distanceStartDeceleration, Color.blue);
            }
            //else{Debug.DrawLine(transform.position, transform.position+transform.TransformDirection(new Vector3(0.3f, 0, 1))* distanceStartDeceleration);}

            if(Physics.Raycast(transform.position, transform.TransformDirection(new Vector3(-0.3f, 0, 1)), out hit4, distanceStartDeceleration, layerMaskEnvironment)){
                float m_comparativeSpeed = m_speed*(Math.Max(hit4.distance-distanceMinFromWall, 0))/(distanceStartDeceleration-distanceMinFromWall);
                m_effectiveSpeed = Math.Min(m_effectiveSpeed, m_comparativeSpeed);
                //Debug.DrawLine(transform.position, transform.position+transform.TransformDirection(new Vector3(-0.3f, 0, 1))* distanceStartDeceleration, Color.blue);
            }
            //else{Debug.DrawLine(transform.position, transform.position+transform.TransformDirection(new Vector3(-0.3f, 0, 1))* distanceStartDeceleration);}


            transform.localRotation = Quaternion.Euler(m_rotation);

            if(grabbed != null){
                m_effectiveSpeed *= grabbed.GetSpeedMultiplier();
            }
            m_Rigidbody.velocity = transform.forward * m_effectiveSpeed;
        }
        
        /*else if (grabbingSteps == 1){
            m_transform.position = Vector3.MoveTowards(m_transform.position, grabbingPosition, m_speed*Time.fixedDeltaTime);
            if(m_transform.position == grabbingPosition){
                grabbed = futureGrabbed;
                grabbed.GetTransform().parent = m_transform;
                grabbed.SetKinematic(true);
                grabbingSteps = 2;
                futureGrabbed = null;
                for(int i = 0; i < Math.Min(boidManager.GetBoids().Count, grabbed.getNumberToCarryMax()); i++){
                    boidManager.GetBoids()[i].StartGrabbing(grabbed);
                }
                grabbed.gameObject.layer = LayerMask.NameToLayer("picked");
            }
        }
        else if (grabbingSteps == 2){
            if(grabbed.GetCurrentlyHolding() >= grabbed.getNumberToCarryMin()){
                grabbingSteps = 3;
            }
            else{
                m_Rigidbody.velocity = new Vector3(0, 0, 0);
            }
        }

        if(grabbingSteps >= 2){//eating
            if(grabbed.GetIsFood() && food < maxFood){
                if(grabbed.GetFoodQuantity() > 0){
                    food += grabbed.Eat(Time.fixedDeltaTime);
                }
                else{
                    LetGoOfGrab();
                    Destroy(grabbed.gameObject);
                }
            }
        }*/


    }

    public float GetFood(){
        return food;
    }
}
