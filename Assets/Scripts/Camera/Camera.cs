using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField]private Transform playerTransform;
    [SerializeField]private Transform childTransform;
    [SerializeField] private LayerMask layerMaskEnvironment;
    private Transform m_transform;
    private float sensitivity = FaunaSlider.Sensitivity;
    private Vector3 velocity = Vector3.zero;

    float rotationX = 0f;
    float rotationY = 0f;
    // Start is called before the first frame update
    void Start()
    {
        m_transform = GetComponentInChildren<Transform>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        sensitivity = FaunaSlider.Sensitivity;
        rotationY += Input.GetAxis("Mouse X") * sensitivity;
        rotationX += Input.GetAxis("Mouse Y") * sensitivity*-1;
        transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
        m_transform.position = Vector3.SmoothDamp(m_transform.position, playerTransform.position, ref velocity, 0.5f);
        RaycastHit hit;
        if(Physics.Raycast(childTransform.position, childTransform.TransformDirection(Vector3.forward), out hit, Vector3.Distance(childTransform.position, transform.position), layerMaskEnvironment)){
            childTransform.position = hit.point;
        }
        RenderSettings.fogColor = new Color(0, 1+(transform.position.y/1000), 1+(transform.position.y/1000), 1);
    }
}
