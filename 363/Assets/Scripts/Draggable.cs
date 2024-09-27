using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    private Vector3 _offset;
    private Camera _mainCamera;
    private CupMovement cupMovement; // Reference to the CupMovement script

    void Start()
    {
        // Get the main camera
        _mainCamera = Camera.main;

        // Get the CupMovement component on the same GameObject
        cupMovement = GetComponent<CupMovement>();
    }

    void OnMouseDown()
    {
        // Calculate the offset between the object's position and the mouse position
        _offset = transform.position - GetMouseWorldPosition();

        // Stop replaying when dragging starts
        if (cupMovement != null && cupMovement.IsReplaying) // Check if replay is in progress
        {
            cupMovement.StopReplay(); // Stop the current replay
        }
    }

    void OnMouseDrag()
    {
        // Move the object to the mouse position plus the offset
        transform.position = GetMouseWorldPosition() + _offset;
    }

    private Vector3 GetMouseWorldPosition()
    {
        // Convert the mouse position to world position
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = 0f; // Ensure we're staying on the same Z-plane (2D game)
        return _mainCamera.ScreenToWorldPoint(mousePoint);
    }
}

