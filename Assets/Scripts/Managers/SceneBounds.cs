using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SceneBounds : MonoBehaviour
{
    [SerializeField] private float width = 400f;
    [SerializeField] private float height = 40f;
    public Vector2 SceneBoundsMin { get; private set; }
    public Vector2 SceneBoundsMax { get; private set; }


    void Start()
    {
        UpdateBounds(width, height);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 center = new Vector3(transform.position.x, transform.position.y, 0f);
        Vector3 size = new Vector3(width, height, 0);
        Gizmos.DrawWireCube(center, size);
    }

    /// <summary>
    /// Updates the scene bounds based on the provided width and height.
    /// This method recalculates the minimum and maximum boundaries of the scene
    /// by adjusting them to the new dimensions centered around the GameObject's position.
    /// </summary>
    /// <param name="width">The new width of the scene. Represents the horizontal extent
    /// of the scene from the center point of this GameObject.</param>
    /// <param name="height">The new height of the scene. Represents the vertical extent
    /// of the scene from the center point of this GameObject.</param>
    /// <remarks>
    /// This method should be called whenever there is a need to dynamically change
    /// the size of the scene during runtime. The SceneBoundsMin and SceneBoundsMax
    /// properties will be updated to reflect the new dimensions.
    /// </remarks>
    /// <example>
    /// Example usage:
    /// <code>
    /// SceneBoundary sceneBoundary = GetComponent<SceneBoundary>();
    /// sceneBoundary.UpdateBounds(150f, 100f);
    /// </code>
    /// </example>
    private void UpdateBounds(float width, float height)
    {
        SceneBoundsMin = new Vector2(transform.position.x - width / 2, transform.position.y - height / 2);
        SceneBoundsMax = new Vector2(transform.position.x + width / 2, transform.position.y + height / 2);
    }
}
