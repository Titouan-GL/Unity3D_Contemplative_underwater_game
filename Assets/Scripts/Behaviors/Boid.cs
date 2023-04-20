using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Boid : MonoBehaviour
{
    [SerializeField] private float zoneRepulsion = 20;
    [SerializeField] private float zoneAlignement = 40;
    [SerializeField] private float zoneAttraction = 70;
    [SerializeField] private float forceRepulsion = 50;
    [SerializeField] private float forceAlignement = 30;
    [SerializeField] private float forceAttraction = 30;
    public Transform target;
    public float forceTarget = 20;
    public bool goToTarget = true;
    public Vector3 velocity = new Vector3();
    public float maxSpeed = 20;
    public float minSpeed = 12;
    public bool drawGizmos = true;
    public bool drawLines = true;

    private int grabbingSteps = 0;
    private Pickable grabbed;
    private Vector3 grabbingPosition;
    private Quaternion grabbingRotation;
    private bool dead = false;
    private bool following = false; 

    [SerializeField] private LayerMask layerMaskEnvironment;
    [SerializeField] private LayerMask layerMaskPickup;
    [SerializeField] private GameObject anchorPoint;
    //private int distanceMinFromWall = 1;
    //private int distanceStartDeceleration = 10;
    private Rigidbody m_Rigidbody;
    [SerializeField] private Transform playerTranform;
    [SerializeField] private float followDistance;
    private bool canFollow = true;

    [SerializeField]private BoidManager boidManager;
    // Update is called once per frame

    public void SetFollow(bool b){
        following = b;
    }

    public void SetCanFollow(bool b){
        canFollow = b;
    }

    void Start(){
        m_Rigidbody = GetComponentInChildren<Rigidbody>();
        //layerMaskEnvironment += LayerMask.GetMask("Environment");
        //layerMaskEnvironment += LayerMask.GetMask("Pickable");
        //layerMaskPickup = LayerMask.GetMask("Pickable");
    }

    void FixedUpdate()
    {
        if(!dead){
            if(grabbingSteps == 0 && boidManager != null){
                Vector3 sumForces = new Vector3();
                Color colorDebugForce = Color.black;
                float nbForcesApplied = 0;
                int nbrOfBoids = 0;
                foreach (Boid otherBoid in boidManager.GetBoids())
                {
                    nbrOfBoids ++;
                    Vector3 vecToOtherBoid = otherBoid.transform.position - transform.position;
                    Vector3 forceToApply = new Vector3();
                    //Si on doit prendre en compte cet autre boid (plus grande zone de perception)
                    if (vecToOtherBoid.sqrMagnitude < zoneAttraction * zoneAttraction)
                    {
                        //Si on est entre attraction et alignement
                        if (vecToOtherBoid.sqrMagnitude > zoneAlignement * zoneAlignement)
                        {
                            //On est dans la zone d'attraction uniquement
                            forceToApply = vecToOtherBoid.normalized * forceAttraction;
                            float distToOtherBoid = vecToOtherBoid.magnitude;
                            float normalizedDistanceToNextZone = ((distToOtherBoid - zoneAlignement) / (zoneAttraction - zoneAlignement));
                            float boostForce = (4 * normalizedDistanceToNextZone);
                            if (!goToTarget) //Encore plus de cohésion si pas de target
                                boostForce *= boostForce;
                            forceToApply = vecToOtherBoid.normalized * forceAttraction * boostForce;
                            colorDebugForce += Color.green;
                        }
                        else
                        {
                            //On est dans alignement, mais est on hors de répulsion ?
                            if (vecToOtherBoid.sqrMagnitude > zoneRepulsion * zoneRepulsion)
                            {
                                //On est dans la zone d'alignement uniquement
                                forceToApply = otherBoid.velocity.normalized * forceAlignement;
                                colorDebugForce += Color.blue;
                            }
                            else
                            {
                                //On est dans la zone de repulsion
                                float distToOtherBoid = vecToOtherBoid.magnitude;
                                float normalizedDistanceToPreviousZone = 1 - (distToOtherBoid / zoneRepulsion);
                                float boostForce = (4 * normalizedDistanceToPreviousZone);
                                forceToApply = vecToOtherBoid.normalized * -1 * (forceRepulsion * boostForce);
                                colorDebugForce += Color.red;
                            }
                        }
                        sumForces += forceToApply;
                        nbForcesApplied++;
                    }
                }
                //On fait la moyenne des forces, ce qui nous rend indépendant du nombre de boids
                sumForces /= Math.Max(nbForcesApplied, 1);
                //Si on a une target, on l'ajoute
                if (goToTarget)
                {
                    Vector3 vecToTarget = target.position - transform.position;
                    if (vecToTarget.sqrMagnitude < 1)
                        goToTarget = true;
                    else
                    {
                        Vector3 forceToTarget = vecToTarget.normalized * forceTarget;
                        sumForces += forceToTarget;
                        colorDebugForce += Color.magenta;
                        nbForcesApplied++;
                        if (drawLines)
                            Debug.DrawLine(transform.position, target.position, Color.magenta);
                    }
                }
                //Debug
                /*if (drawLines)
                    Debug.DrawLine(transform.position, transform.position + sumForces, colorDebugForce / nbForcesApplied);*/

                //On freine
                velocity += -velocity * 10 * Vector3.Angle(sumForces, velocity) / 180.0f * Time.deltaTime;
                //on applique les forces
                velocity += sumForces * Time.deltaTime;
                //On limite la vitesse
                if (velocity.sqrMagnitude > maxSpeed * maxSpeed)
                    velocity = velocity.normalized * maxSpeed;
                if (velocity.sqrMagnitude < minSpeed * minSpeed)
                    velocity = velocity.normalized * minSpeed;
                //On regarde dans la bonne direction
                if (velocity.sqrMagnitude > 0)
                    transform.LookAt(transform.position + velocity);
                //Debug
                /*if (drawLines)
                    Debug.DrawLine(transform.position, transform.position + velocity, Color.blue);
                    //Deplacement du boid
                    RaycastHit hit;
                    if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, distanceStartDeceleration, layerMaskEnvironment)){
                        velocity *= (Math.Max(hit.distance-distanceMinFromWall, 0))/(distanceStartDeceleration-distanceMinFromWall);
                    }
                    if(!following && canFollow){
                        velocity /= 2;
                    }*/
                    transform.position += velocity * Time.deltaTime;
            }
            /*else if (grabbingSteps == 1){
                transform.position = Vector3.MoveTowards(transform.position, grabbingPosition, maxSpeed*Time.fixedDeltaTime);
                if(transform.position == grabbingPosition){
                    grabbed.AddFish();
                    grabbingSteps = 2;
                    grabbingPosition = transform.localPosition;
                    grabbingRotation = transform.localRotation;
                }
            }
            else{
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(0, 0, 0), maxSpeed*Time.fixedDeltaTime);
                Vector3 heading = -1 * transform.parent.localPosition;
                transform.localRotation = Quaternion.LookRotation(heading.normalized);
            }
            if(canFollow && playerTranform != null){
                if(Vector3.Distance(transform.position, playerTranform.position) <= followDistance && target != playerTranform){
                    following = true;
                    boidManager.LeaveGroup(this);
                    boidManager = playerTranform.gameObject.GetComponentInChildren<BoidManager>();
                    boidManager.JoinGroup(this);
                    target = playerTranform;
                }
            }*/
        }
    }

    public void SetBoidManager(BoidManager bm){
        boidManager = bm;
    }

    public void SetPlayerTransform(Transform ptransform){
        playerTranform = ptransform;
    }

    void OnDrawGizmosSelected()
    {
        if (drawGizmos)
        {
            // Répulsion
            Gizmos.color = new Color(1, 0, 0, 1.0f);
            Gizmos.DrawWireSphere(transform.position, zoneRepulsion);
            // Alignement
            Gizmos.color = new Color(0, 1, 0, 1.0f);
            Gizmos.DrawWireSphere(transform.position, zoneAlignement);
            // Attraction
            Gizmos.color = new Color(0, 0, 1, 1.0f);
            Gizmos.DrawWireSphere(transform.position, zoneAttraction);
        }
    }

    public void StartGrabbing(Pickable obj){
        grabbingSteps = 1;
        grabbed = obj;
        RaycastHit hit;
        if(Physics.Raycast(transform.position, obj.transform.position - transform.position, out hit, 200, layerMaskPickup)){
            grabbingPosition = hit.point;
            GameObject anchor = Instantiate(anchorPoint, grabbingPosition, Quaternion.identity);
            anchor.transform.parent = grabbed.GetTransform();
            transform.parent = anchor.transform;
        }
    }

    public void LetGo(){
        grabbingSteps = 0;
        grabbed = null;
        transform.parent = null;
    }


    public void Dies(){
        dead = true;
        gameObject.layer = LayerMask.NameToLayer("pickable");
        gameObject.AddComponent<Pickable>();
        GetComponent<Pickable>().InitAsFish();
        GetComponentInChildren<Rigidbody>().useGravity = true;
        GetComponentInChildren<Rigidbody>().isKinematic = false;
    }
}
