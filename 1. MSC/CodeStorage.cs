   float GetPlayerMoveDirection()
   {
       float direction = 0f;

        leftButtonPressed = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
        rightButtonPressed = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);

     
      if (rightButtonPressed)
       {
           direction = 1f;
       }
       else if (leftButtonPressed)
       {
           direction = -1f;
       }
       else
       {
           direction = 0f;
       }

       return direction;

   }

   CameraFollow 12-12-13:11 "Before updating with new gippity convo"

   using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;
    public Vector2 sceneBoundsMin;
    public Vector2 sceneBoundsMax;

    [SerializeField] private float rightOffset;
    [SerializeField] private float topOffset;
    [SerializeField] private float bottomOffset;
    [SerializeField] private float leftOffset;

    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
    mainCamera = Camera.main;

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
