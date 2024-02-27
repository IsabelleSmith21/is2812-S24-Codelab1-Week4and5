using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToPlayground : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
            
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            SceneManager.LoadScene("PlaygroundScreen");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
