using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private GameObject currentNode;
    [SerializeField] private float speed = 4f;

    [SerializeField] private string direction = "";
    [SerializeField] private string lastMovingDirection = "";

    void Update()
    {
        NodeController currentNodeController = currentNode.GetComponent<NodeController>();

        transform.position = Vector2.MoveTowards(transform.position, currentNode.transform.position, speed * Time.deltaTime);

        bool reverseDirection = false;
        if (
            (direction == "left" && lastMovingDirection == "right")
            || (direction == "right" && lastMovingDirection == "left")
            || (direction == "up" && lastMovingDirection == "down")
            || (direction == "down" && lastMovingDirection == "up")
            )
            { reverseDirection = true; }

        if (transform.position.x == currentNode.transform.position.x && transform.position.y == currentNode.transform.position.y || reverseDirection)
        {
            GameObject newNode = currentNodeController.GetNodeFromDirection(direction);

            if(newNode != null)
            {
                currentNode = newNode;
                lastMovingDirection = direction;
            }
            else
            {
                direction = lastMovingDirection;
                newNode = currentNodeController.GetNodeFromDirection(direction);
                if(newNode != null)
                {
                    currentNode = newNode;
                }
            }
        }
    }

    public void SetDirection(string newDirection)
    {
        direction = newDirection;
    }
}
