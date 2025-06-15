using NUnit.Framework.Interfaces;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum GhostNodesStatesEnum
    {
        respawning,
        leftNode,
        rightNode,
        centerNode,
        startNode,
        movingInNodes
    }

    public GhostNodesStatesEnum ghostNodesStates;
    public GhostNodesStatesEnum respawnState;

    public enum GhostType
    {
        red,
        blue,
        pink,
        orange
    }

    public GhostType ghostType;

    public GameObject ghostNodeLeft;
    public GameObject ghostNodeRight;
    public GameObject ghostNodeCenter;
    public GameObject ghostNodeStart;

    public MovementController movementController;

    public GameObject startingNode;

    public bool readyToLeaveHome = false;

    [SerializeField] private GameManager gameManager;

    public bool testRespawn = false;

    private void Awake()
    {
        movementController = GetComponent<MovementController>();

        if (ghostType == GhostType.red)
        {
            ghostNodesStates = GhostNodesStatesEnum.startNode;
            respawnState = GhostNodesStatesEnum.centerNode;
            startingNode = ghostNodeStart;
            readyToLeaveHome = true;
        }
        else if (ghostType == GhostType.blue)
        {
            ghostNodesStates = GhostNodesStatesEnum.leftNode;
            startingNode = ghostNodeLeft;
            respawnState = GhostNodesStatesEnum.centerNode;
        }
        else if (ghostType == GhostType.pink)
        {
            ghostNodesStates = GhostNodesStatesEnum.centerNode;
            startingNode = ghostNodeCenter;
            respawnState = GhostNodesStatesEnum.leftNode;
        }
        else if (ghostType == GhostType.orange)
        {
            ghostNodesStates = GhostNodesStatesEnum.rightNode;
            startingNode = ghostNodeRight;
            respawnState = GhostNodesStatesEnum.rightNode;
        }

        movementController.currentNode = startingNode;
        transform.position = startingNode.transform.position;
    }

    private void Update()
    {
        if(testRespawn == true)
        {
            readyToLeaveHome = false;
            ghostNodesStates = GhostNodesStatesEnum.respawning;
            testRespawn = false;
        }
    }

    public void ReachedCenterOfNode(NodeController nodeController)
    {
        if (ghostNodesStates == GhostNodesStatesEnum.movingInNodes)
        {
            //Determine next nodes to go to
            if(ghostType == GhostType.red)
            {
                DetermineRedGhostDirection();
            }
            else if (ghostType == GhostType.pink)
            {
                DeterminePinkGhostDirection();
            }
            else if (ghostType == GhostType.blue)
            {
                DetermineBlueGhostDirection();
            }
            else if (ghostType == GhostType.orange)
            {
                DetermineOrangeGhostDirection();
            }
        }
        else if (ghostNodesStates == GhostNodesStatesEnum.respawning)
        {
            Debug.Log("<color=green>Respawning ghosts...</color>");

            string direction = "";

            //We have reached our start node, move to the center node
            if (Vector2.Distance(transform.position, ghostNodeStart.transform.position) < 0.25f)
            {
                direction = "down";
                Debug.Log("<color=red>Reached start node</color>");
            }

            //We have reached our center node, either finish respawning or move to the left or right node
            else if (transform.position.x == ghostNodeCenter.transform.position.x && transform.position.y == ghostNodeCenter.transform.position.y)
            {
                if(respawnState == GhostNodesStatesEnum.centerNode)
                {
                    ghostNodesStates = respawnState;
                }
                else if(respawnState == GhostNodesStatesEnum.leftNode)
                {
                    direction = "left";
                }
                else if (respawnState == GhostNodesStatesEnum.rightNode)
                {
                    direction = "right";
                }
            }
            else if(
                (transform.position.x == ghostNodeLeft.transform.position.x && transform.position.y == ghostNodeLeft.transform.position.y) ||
                (transform.position.x == ghostNodeRight.transform.position.x && transform.position.y == ghostNodeRight.transform.position.y)
                )
            {
                ghostNodesStates = respawnState;
            }
            else
            {
                //Determine quickest direction to home
                direction = GetClosestDirection(ghostNodeStart.transform.position);
            }

            movementController.SetDirection(direction);
        }
        else
        {
            //If we are ready to leave home, we can change the state of the ghost nodes
            if (readyToLeaveHome)
            {
                if (ghostNodesStates == GhostNodesStatesEnum.leftNode)
                {
                    ghostNodesStates = GhostNodesStatesEnum.centerNode;
                    movementController.SetDirection("right");
                    Debug.Log("<color=blue>Ghost is leaving home, moving to center node</color>");
                }
                else if (ghostNodesStates == GhostNodesStatesEnum.rightNode)
                {
                    ghostNodesStates = GhostNodesStatesEnum.centerNode;
                    movementController.SetDirection("left");
                    Debug.Log("<color=blue>Ghost is leaving home, moving to center node</color>");
                }
                else if (ghostNodesStates == GhostNodesStatesEnum.centerNode)
                {
                    ghostNodesStates = GhostNodesStatesEnum.startNode;
                    movementController.SetDirection("up");
                    Debug.Log("<color=blue>Ghost is leaving home, moving to start node</color>");
                }
                else if(ghostNodesStates == GhostNodesStatesEnum.startNode)
                {
                    ghostNodesStates = GhostNodesStatesEnum.movingInNodes;
                    movementController.SetDirection("left");
                    Debug.Log("<color=blue>Ghost is leaving home, moving to left node</color>");
                }
            }
        }
    }

    public void DetermineRedGhostDirection()
    {
        string direction = GetClosestDirection(gameManager.pacman.transform.position);
        movementController.SetDirection(direction);
    }

    public void DeterminePinkGhostDirection()
    {
    }

    public void DetermineBlueGhostDirection()
    {
    }

    public void DetermineOrangeGhostDirection()
    {
    }

    public string GetClosestDirection(Vector2 target)
    {
        float shortestDistance = 0;
        string lastMovingDirection = movementController.lastMovingDirection;
        string newDirection = "";

        NodeController nodeController = movementController.currentNode.GetComponent<NodeController>();

        if(nodeController.canMoveUp && lastMovingDirection != "down")
        {
            GameObject nodeUp = nodeController.nodeUp;

            float distance = Vector2.Distance(nodeUp.transform.position, target);

            if(distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection = "up";
            }
        }

        if (nodeController.canMoveDown && lastMovingDirection != "up")
        {
            GameObject nodeDown = nodeController.nodeDown;

            float distance = Vector2.Distance(nodeDown.transform.position, target);

            if (distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection = "down";
            }
        }

        if (nodeController.canMoveLeft && lastMovingDirection != "right")
        {
            GameObject nodeLeft = nodeController.nodeLeft;

            float distance = Vector2.Distance(nodeLeft.transform.position, target);

            if (distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection = "left";
            }
        }

        if (nodeController.canMoveRight && lastMovingDirection != "left")
        {
            GameObject nodeRight = nodeController.nodeRight;

            float distance = Vector2.Distance(nodeRight.transform.position, target);

            if (distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection = "right";
            }
        }

        return newDirection;
    }
}
