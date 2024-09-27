using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public List<GameObject> draggableObjects; // List of all draggable cups
    public float alignmentThreshold = 0.1f; // How close the object needs to be to align

    private bool all363OutlineDestroyed = false; // Check if all 363 outlines are destroyed

    void Start()
    {
        // Deactivate all draggable objects at the start
        draggableObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Draggable"));
        
        // Start recording when the game begins
        StartRecording();
    }

    void Update()
    {
        // Check alignment of draggable objects with the 363 outlines
        CheckAlignment("363Outline");
        
        // Start replay if all 363 outlines are destroyed
        if (all363OutlineDestroyed)
        {
            StartReplay();
        }
    }

    private void StartRecording()
    {
        foreach (GameObject draggable in draggableObjects)
        {
            CupMovement cupMovement = draggable.GetComponent<CupMovement>();
            if (cupMovement != null)
            {
                cupMovement.StartRecording();
            }
        }
    }

    private void CheckAlignment(string tag)
    {
        GameObject[] outlines = GameObject.FindGameObjectsWithTag(tag);
        all363OutlineDestroyed = outlines.Length == 0; // True if no outlines left

        foreach (GameObject draggable in draggableObjects)
        {
            foreach (GameObject outline in outlines)
            {
                // Check alignment
                if (Vector3.Distance(draggable.transform.position, outline.transform.position) < alignmentThreshold)
                {
                    Destroy(outline); // Destroy the outline
                    break; // Break after aligning with one outline
                }
            }
        }
    }

    public void StartReplay()
    {
        foreach (GameObject draggable in draggableObjects)
        {
            CupMovement cupMovement = draggable.GetComponent<CupMovement>();
            if (cupMovement != null)
            {
                cupMovement.StartReplay();
            }
        }
    }
}


