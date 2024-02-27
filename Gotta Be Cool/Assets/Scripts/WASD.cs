using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASD : MonoBehaviour
{
    public float movementSpeed =5f; //Speed the player is moving
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         //getting player input for WASD movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        //Creating a Vector3 to store normalized movement direction 
        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f).normalized;
        
        //calculate the movement direction 
        transform.Translate(movement * movementSpeed * Time.deltaTime);

    }
}
