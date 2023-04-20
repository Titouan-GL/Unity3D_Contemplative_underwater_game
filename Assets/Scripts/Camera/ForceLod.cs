using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceLod : MonoBehaviour
{
    private Transform m_MainCameraTransform;
    private LODGroup lod;
    [SerializeField] private Transform m_transform;
    [SerializeField] private int offset = 100;
    public float distance;
    // Start is called before the first frame update
    void Start()
    {
        m_MainCameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        lod = GetComponent<LODGroup>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        distance = Vector3.Distance(m_MainCameraTransform.position, m_transform.position);
        if(distance > RenderSettings.fogEndDistance+offset){
            for(int i = 0; i < transform.childCount; i++){
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        else{
            for(int i = 0; i < transform.childCount; i++){
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
}
