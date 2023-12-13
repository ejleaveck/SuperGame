using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Transform cameraTransform;
    public float parallaxMultiplier;
    private Vector3 previousCameraPosition;

    // Start is called before the first frame update
    void Start()
    {
        previousCameraPosition = cameraTransform.position;
            }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
       Vector3 deltaMovement = cameraTransform.position - previousCameraPosition;
        transform.position += deltaMovement * parallaxMultiplier;
        previousCameraPosition = cameraTransform.position;
    }
}
