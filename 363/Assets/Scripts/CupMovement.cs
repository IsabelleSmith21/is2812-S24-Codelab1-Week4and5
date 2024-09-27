using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupMovement : MonoBehaviour
{
    private List<Vector3> recordedPositions = new List<Vector3>();
    public bool IsReplaying { get; private set; } = false;

    void OnMouseDown()
    {
        // Stop replaying when dragging starts
        if (IsReplaying)
        {
            StopReplay();
        }
    }

    void OnMouseDrag()
    {
        // Move the object to the mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Ensure we're staying on the same Z-plane
        transform.position = mousePosition;
    }

    public void StartRecording()
    {
        recordedPositions.Clear(); // Clear previous recordings
        recordedPositions.Add(transform.position); // Record the initial position
    }

    public void StartReplay()
    {
        IsReplaying = true;
        StartCoroutine(ReplayMovement());
    }

    private IEnumerator ReplayMovement()
    {
        foreach (Vector3 position in recordedPositions)
        {
            // Move towards the recorded position
            while (Vector3.Distance(transform.position, position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, position, Time.deltaTime);
                yield return null;
            }
        }
        IsReplaying = false; // Mark replay as finished
    }

    public void StopReplay()
    {
        IsReplaying = false; // Stop replay logic
        StopAllCoroutines(); // Stop any running coroutines
    }
}
