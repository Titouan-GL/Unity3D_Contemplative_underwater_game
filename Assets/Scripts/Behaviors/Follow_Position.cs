using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_Position : MonoBehaviour
{
    [SerializeField] Transform position_to_follow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = position_to_follow.position;
    }
}
