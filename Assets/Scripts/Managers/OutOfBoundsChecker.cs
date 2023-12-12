using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OutOfBoundsChecker : MonoBehaviour
{
    private Camera mainCamera;

    public bool IsOutOfBoundsLeft { get; private set; }
    public bool IsOutOfBoundsRight { get; private set; }
    public bool IsOutOfBoundsTop { get; private set; }
    public bool IsOutOfBoundsBottom { get; private set; }

    private Vector3 screenPoint;


    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfOutOfBounds();
    }

    private void CheckIfOutOfBounds()
    {
        screenPoint = mainCamera.WorldToViewportPoint(transform.position);

        this.IsOutOfBoundsLeft = screenPoint.x < 0;
        this.IsOutOfBoundsRight = screenPoint.x > 1;
        this.IsOutOfBoundsTop = screenPoint.y > 1;
        this.IsOutOfBoundsBottom = screenPoint.y < 0;
    }
}
