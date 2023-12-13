using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;

    private SceneBounds sceneBounds;

    [SerializeField] private float rightOffset;
    [SerializeField] private float topOffset;
    [SerializeField] private float bottomOffset;
    [SerializeField] private float leftOffset;

    private Camera mainCamera;

    [SerializeField] private float cameraFollowSpeed = 1f;

    private void Start()
    {
        mainCamera = Camera.main;

        sceneBounds = FindObjectOfType<SceneBounds>();
        if (sceneBounds == null)
        {
            Debug.LogError("${nameof(CameraFollow)}: No ${nameof(SceneBounds)} found in scene!");
            return;
        }
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

            // Calculate the boundary box
            float boundaryRight = 0.5f + rightOffset;
            float boundaryLeft = 0.5f - leftOffset;
            float boundaryTop = 0.5f + topOffset;
            float boundaryBottom = 0.5f - bottomOffset;

            // Horizontal movement
            if (cameraPosition.x > boundaryRight)
            {
                newPosition.x = Mathf.Lerp(newPosition.x, playerTransform.position.x - rightOffset, cameraFollowSpeed * Time.deltaTime);
            }
            else if (cameraPosition.x < boundaryLeft)
            {
                newPosition.x = Mathf.Lerp(newPosition.x, playerTransform.position.x + leftOffset, cameraFollowSpeed * Time.deltaTime);
            }

            // Vertical movement
            if (cameraPosition.y > boundaryTop)
            {
                newPosition.y = Mathf.Lerp(newPosition.y, playerTransform.position.y - topOffset, cameraFollowSpeed * Time.deltaTime);
            }
            else if (cameraPosition.y < boundaryBottom)
            {
                newPosition.y = Mathf.Lerp(newPosition.y, playerTransform.position.y + bottomOffset, cameraFollowSpeed * Time.deltaTime);
            }

            // Clamping camera position
            newPosition.x = Mathf.Clamp(newPosition.x, sceneBounds.SceneBoundsMin.x, sceneBounds.SceneBoundsMax.x);
            newPosition.y = Mathf.Clamp(newPosition.y, sceneBounds.SceneBoundsMin.y, sceneBounds.SceneBoundsMax.y);

            transform.position = newPosition;
        }
    }
}
