using System;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [Header("Variables")]
    [SerializeField] private GameObject currentNode;
    [SerializeField] private float speed = 4f;
    [SerializeField] private bool canWarp = true;

    [SerializeField] private string direction = "";
    public string lastMovingDirection = "";

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
            if(currentNodeController.isWarpLeftNode && canWarp)
            {
                currentNode = gameManager.rightWarpNode;
                direction = "left";
                lastMovingDirection = "left";
                transform.position = currentNode.transform.position;
                canWarp = false; 
            }
            else if(currentNodeController.isWarpRightNode && canWarp)
            {
                currentNode = gameManager.leftWarpNode;
                direction = "right";
                lastMovingDirection = "right";
                transform.position = currentNode.transform.position;
                canWarp = false;
            }
            else
            {
                GameObject newNode = currentNodeController.GetNodeFromDirection(direction);

                if (newNode != null)
                {
                    currentNode = newNode;
                    lastMovingDirection = direction;
                }
                else
                {
                    direction = lastMovingDirection;
                    newNode = currentNodeController.GetNodeFromDirection(direction);
                    if (newNode != null)
                    {
                        currentNode = newNode;
                    }
                }
            }
        }
        else
        {
            canWarp = true; 
        }
    }

    public void SetDirection(string newDirection)
    {
        direction = newDirection;
    }
}
