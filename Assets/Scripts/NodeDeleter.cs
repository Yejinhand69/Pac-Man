using UnityEngine;

public class NodeDeleter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object has the tag "Node"
        if (collision.CompareTag("Node"))
        {
            // Destroy the collided object
            Destroy(collision.gameObject);
        }
    }
}
