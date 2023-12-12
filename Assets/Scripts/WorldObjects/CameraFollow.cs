using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class CameraFollow : MonoBehaviour
{
    private Transform playerTransform;
    public Vector2 sceneBoundsMin;
    public Vector2 sceneBoundsMax;

    [SerializeField] private float rightOffset;
    [SerializeField] private float topOffset;
    [SerializeField] private float bottomOffset;
    [SerializeField] private float leftOffset;

    private Camera mainCamera;

    private void OnDrawGizmos()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (mainCamera != null)
        {
            Gizmos.color = Color.blue;

            float totalWidth = rightOffset + leftOffset;
            float totalHeight = topOffset + bottomOffset;

            Vector3 cameraCenter = mainCamera.ViewportToWorldPoint(new Vector3(.5f, .5f, 0f));
            Vector3 offsetPosition = new Vector3(cameraCenter.x + (rightOffset - leftOffset) / 2f, cameraCenter.y + (topOffset - bottomOffset) / 2f, cameraCenter.z);

            Gizmos.DrawWireCube(offsetPosition, new Vector3(totalWidth, totalHeight, 1f));
        }
        else
        {
            Debug.Log($"{nameof(CameraFollow)} > {nameof(OnDrawGizmos)}: No main camera found.");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    mainCamera = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayer(Transform player)
    {
        playerTransform = player;
    }

    private void LateUpdate()
    {
        if (playerTransform != null)
        {
            Vector3 cameraPosition = mainCamera.WorldToViewportPoint(playerTransform.position);
            Vector3 newPosition = transform.position;

            // Horizontal camera movement
            if (cameraPosition.x > 0.5f + rightOffset)
            {
                newPosition.x = playerTransform.position.x - rightOffset;
            }
            else if (cameraPosition.x < 0.5f - leftOffset)
            {
                newPosition.x = playerTransform.position.x + leftOffset;
            }

            //Vertical camera movement
            if(cameraPosition.y > 0.5f + topOffset)
            {
                newPosition.y = playerTransform.position.y - topOffset;
            }
            else if (cameraPosition.y < 0.5f - bottomOffset)
            {
                newPosition.y = playerTransform.position.y + bottomOffset;
            }

            newPosition.x = Mathf.Clamp(newPosition.x, sceneBoundsMin.x, sceneBoundsMax.x);
            newPosition.y = Mathf.Clamp(newPosition.y, sceneBoundsMin.y, sceneBoundsMax.y);

            transform.position = newPosition;

            Debug.Log($"{nameof(CameraFollow)} > {nameof(LateUpdate)}: Camera position = {transform.position}.");
        }
        else
        {
            Debug.LogWarning($"{nameof(CameraFollow)} > {nameof(LateUpdate)}: No player transform to follow.");
        }
    }
}
