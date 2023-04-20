using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
public class BoidManager : MonoBehaviour
{
    /*private static BoidManager instance = null;
    public static BoidManager sharedInstance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<BoidManager>();
            }
        return instance;
        }
    }*/
    public GameObject prefabBoid;
    public float nbBoids = 100;
    public float startSpeed = 1;
    public float startSpread = 10;
    private List<Boid> boids = new List<Boid>();
    [SerializeField] private Transform target;
    [SerializeField] private bool follow = false;
    [SerializeField] private bool canFollow = true;
    [SerializeField] private Transform playerTranform;
    
    public void LeaveGroup(Boid b){
        boids.Remove(b);
    }

    public void JoinGroup(Boid b){
        boids.Add(b);
    }
    public List<Boid> GetBoids(){
        return boids;
    }

    public ReadOnlyCollection<Boid> roBoids
    {
        get { return new ReadOnlyCollection<Boid>(boids); }
    }
    void Start()
    {
        for (int i = 0; i < nbBoids * FaunaSlider.faunaDensity; i++)
        {
            GameObject go = GameObject.Instantiate<GameObject>(prefabBoid, transform.position, Quaternion.identity);
            Boid b = go.GetComponentInChildren<Boid>();
            Vector3 positionBoid = transform.position + Random.insideUnitSphere * startSpread;
            b.velocity = (positionBoid - transform.position).normalized * startSpeed;
            go.transform.parent = this.transform;
            b.maxSpeed *= Random.Range(0.95f, 1.05f);
            b.SetFollow(follow);
            b.SetCanFollow(canFollow);
            b.SetBoidManager(this);
            if(canFollow){
                b.SetPlayerTransform(playerTranform);
            }
            boids.Add(b);
        }
        foreach (Boid b2 in boids)
        {
            b2.target = target.transform;
            b2.goToTarget = true;
        }

    }

    public void KillFish()
    {
        boids[0].Dies();
        boids.RemoveAt(0);
    }
}