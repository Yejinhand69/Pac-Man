using UnityEngine;

public class NodeDeleter : MonoBehaviour
{
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
