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

    private void Awake()
    {
        movementController = GetComponent<MovementController>();

        if (ghostType == GhostType.red)
        {
            ghostNodesStates = GhostNodesStatesEnum.startNode;
            startingNode = ghostNodeStart;
        }
        else if (ghostType == GhostType.blue)
        {
            ghostNodesStates = GhostNodesStatesEnum.leftNode;
            startingNode = ghostNodeLeft;
        }
        else if (ghostType == GhostType.pink)
        {
            ghostNodesStates = GhostNodesStatesEnum.centerNode;
            startingNode = ghostNodeCenter;
        }
        else if (ghostType == GhostType.orange)
        {
            ghostNodesStates = GhostNodesStatesEnum.rightNode;
            startingNode = ghostNodeRight;
        }

        movementController.currentNode = startingNode;
    }

    public void ReachedCenterOfNode(NodeController nodeController)
    {
        if (ghostNodesStates == GhostNodesStatesEnum.movingInNodes)
        {
            //Determine next nodes to go to
        }
        else if (ghostNodesStates == GhostNodesStatesEnum.respawning)
        {
            //Determine quickest direction to home
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
                }
                else if (ghostNodesStates == GhostNodesStatesEnum.rightNode)
                {
                    ghostNodesStates = GhostNodesStatesEnum.centerNode;
                    movementController.SetDirection("left");
                }
                else if (ghostNodesStates == GhostNodesStatesEnum.centerNode)
                {
                    ghostNodesStates = GhostNodesStatesEnum.startNode;
                    movementController.SetDirection("up");
                }
                else if(ghostNodesStates == GhostNodesStatesEnum.startNode)
                {
                    ghostNodesStates = GhostNodesStatesEnum.movingInNodes;
                    movementController.SetDirection("left");
                }
            }
        }
    }
}
