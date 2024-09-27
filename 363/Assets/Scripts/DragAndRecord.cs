using UnityEngine;
using System.Collections.Generic;

public class DragAndRecord : MonoBehaviour
{
    private Vector3 _offset;
    private Camera _mainCamera;
    private bool _isRecording = false;
    private bool _isReplaying = false;

    public float replaySpeed = 1f; // Speed for replaying
    public float alignmentThreshold = 0.1f; // How close the object needs to be to align

    public List<GameObject> sixStackOutlines; // List of 6StackOutlines objects to activate
    public List<GameObject> finaleObjects; // List of Finale objects to activate

    private List<Vector3> recordedPositions = new List<Vector3>(); // Store recorded positions
    private int replayIndex = 0;

    private bool all363OutlineDestroyed = false;
    private bool all6StackDestroyed = false;

    void Start()
    {
        _mainCamera = Camera.main;

        // Deactivate the 6StackOutlines and Finale objects at the start
        SetActiveState(sixStackOutlines, false);
        SetActiveState(finaleObjects, false);
    }

    void Update()
    {
        // Check for alignment with `363Outline` if they're not all destroyed yet
        if (!all363OutlineDestroyed)
        {
            CheckAlignment("363Outline", () =>
            {
                // Once all 363Outline objects are destroyed
                all363OutlineDestroyed = true;
                SetActiveState(sixStackOutlines, true); // Activate 6StackOutlines
            });
        }
        // Check for alignment with `6Stack` after 363Outline objects are destroyed
        else if (!all6StackDestroyed)
        {
            CheckAlignment("6Stack", () =>
            {
                // Once all 6Stack objects are destroyed
                all6StackDestroyed = true;
                SetActiveState(finaleObjects, true); // Activate Finale objects
            });
        }
        // Check for alignment with `Finale` objects after 6Stack objects are destroyed
        else
        {
            CheckAlignment("Finale", () =>
            {
                // Start the replay after all Finale objects are destroyed
                if (!_isReplaying && recordedPositions.Count > 0)
                {
                    StartReplay();
                }
            });
        }

        // Replay movement if replay is in progress
        if (_isReplaying)
        {
            ReplayMovement();
        }
    }

    void OnMouseDown()
    {
        if (!_isReplaying)
        {
            _offset = transform.position - GetMouseWorldPosition();
            _isRecording = true;
            recordedPositions.Clear(); // Clear the previous recording
        }
    }

    void OnMouseDrag()
    {
        if (!_isReplaying)
        {
            Vector3 newPosition = GetMouseWorldPosition() + _offset;
            transform.position = newPosition;

            if (_isRecording)
            {
                recordedPositions.Add(newPosition); // Record the position
            }
        }
    }

    void OnMouseUp()
    {
        if (!_isReplaying)
        {
            _isRecording = false; // Stop recording when the drag ends
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = 0f;
        return _mainCamera.ScreenToWorldPoint(mousePoint);
    }

    private void StartReplay()
    {
        _isReplaying = true;
        replayIndex = 0;
    }

    private void ReplayMovement()
    {
        if (replayIndex < recordedPositions.Count)
        {
            transform.position = Vector3.MoveTowards(transform.position, recordedPositions[replayIndex], replaySpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, recordedPositions[replayIndex]) < 0.01f)
            {
                replayIndex++;
            }
        }
        else
        {
            _isReplaying = false; // End replay when all positions are played back
        }
    }

    private void CheckAlignment(string tag, System.Action onAllDestroyed)
    {
        GameObject[] objectsToAlign = GameObject.FindGameObjectsWithTag(tag);

        if (objectsToAlign.Length == 0)
        {
            return;
        }

        foreach (GameObject obj in objectsToAlign)
        {
            // Check if the object is close enough to align
            if (Vector3.Distance(transform.position, obj.transform.position) < alignmentThreshold)
            {
                Destroy(obj); // Destroy the object when aligned
            }
        }

        // If all objects with the tag are destroyed, call the provided callback
        if (GameObject.FindGameObjectsWithTag(tag).Length == 0)
        {
            onAllDestroyed?.Invoke();
        }
    }

    private void SetActiveState(List<GameObject> objects, bool state)
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(state);
        }
    }
}


