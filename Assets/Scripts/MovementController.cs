using System;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [Header("Variables")]
    public GameObject currentNode;
    [SerializeField] private float speed = 4f;
    [SerializeField] private bool canWarp = true;

    [SerializeField] private string direction = "";
    public string lastMovingDirection = "";
    public bool isGhost = false;

    private void Awake()
    {
        //lastMovingDirection = "left";
    }

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
            if(isGhost)
            {
                GetComponent<EnemyController>().ReachedCenterOfNode(currentNodeController);
            }

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

    public Vector2 GetCurrentDirection()
    {
        if (direction == "left")
        {
            return Vector2.left;
        }
        else if (direction == "right")
        {
            return Vector2.right;
        }
        else if (direction == "up")
        {
            return Vector2.up;
        }
        else if (direction == "down")
        {
            return Vector2.down;
        }
        return Vector2.zero;
    }
    public string GetDirectionFromVector(Vector3 direction)
    {
        direction = direction.normalized;
        if (Vector3.Dot(direction, Vector3.up) > 0.9f) return "up";
        if (Vector3.Dot(direction, Vector3.down) > 0.9f) return "down";
        if (Vector3.Dot(direction, Vector3.left) > 0.9f) return "left";
        if (Vector3.Dot(direction, Vector3.right) > 0.9f) return "right";
        return "left"; //fallback
    }
}
