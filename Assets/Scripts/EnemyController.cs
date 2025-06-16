using NUnit.Framework.Constraints;
using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour, IGameEventListener
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
    public enum GhostType
    {
        red,
        blue,
        pink,
        orange
    }

    [Header("GhostNodesStates/Type")]
    public GhostNodesStatesEnum ghostNodesStates;
    public GhostType ghostType;

    [Header("GhostNodes")]
    public GameObject ghostNodeLeft;
    public GameObject ghostNodeRight;
    public GameObject ghostNodeCenter;
    public GameObject ghostNodeStart;

    [Header("Movement Controller")]
    public MovementController movementController;

    [Header("Variables")]
    public GameObject startingNode;
    public bool readyToLeaveHome = false;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform redGhostTransform;

    [Header("Game Events")]
    [SerializeField] private GameEvent frightenedListener;

    [Header("Color Changing")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float colorChangeInterval = 0.2f;
    private Coroutine colorChangeCoroutine;

    [Header("Developer Testing Kit")]
    public bool forceRespawn = false;

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
        transform.position = startingNode.transform.position;
    }

    private void OnEnable()
    {
        frightenedListener.RegisterListener(this);
    }
    private void OnDisable()
    {
        frightenedListener.UnregisterListener(this);
        StopColorChange();
    }

    public void OnEventRaised(GameEvent gameEvent, Component sender, object[] args)
    {
        if (gameEvent == frightenedListener)
        {
            StartColorChange();
        }
    }
    private void StartColorChange()
    {
        if(colorChangeCoroutine != null)
        {
            StopCoroutine(colorChangeCoroutine);
        }

        colorChangeCoroutine = StartCoroutine(ColorChangeCoroutine(3f));
    }

    private void StopColorChange()
    {
        if (colorChangeCoroutine != null)
        {
            StopCoroutine(colorChangeCoroutine);
            colorChangeCoroutine = null;
        }
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
        }
    }

    private IEnumerator ColorChangeCoroutine(float duration)
    {
        Coroutine loop = StartCoroutine(ColorLoop());

        yield return new WaitForSeconds(duration);

        StopCoroutine(loop);
        
        if(ghostType == GhostType.red)
        {
            spriteRenderer.color = new Color(250f / 255f, 0f / 255f, 0f / 255f); //red
        }
        else if (ghostType == GhostType.blue)
        {
            spriteRenderer.color = new Color(37f / 255f, 73f / 255f, 255f / 255f); //blue
        }
        else if (ghostType == GhostType.pink)
        {
            spriteRenderer.color = new Color(255f / 255f, 57f / 255f, 183f / 255f); //pink
        }
        else if (ghostType == GhostType.orange)
        {
            spriteRenderer.color = new Color(255f / 255f, 107f / 255f, 0f / 255f); //orange
        }

        colorChangeCoroutine = null;
    }

    private IEnumerator ColorLoop()
    {
        bool toggle = false;

        while (true)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = toggle ? Color.blue : Color.white;
                toggle = !toggle;
            }

            yield return new WaitForSeconds(colorChangeInterval);
        }
    }

    private void Update()
    {
        if(forceRespawn)
        {
            ghostNodesStates = GhostNodesStatesEnum.respawning;
            forceRespawn = false;
        }
    }

    public void ReachedCenterOfNode(NodeController nodeController)
    {
        if (ghostNodesStates == GhostNodesStatesEnum.movingInNodes)
        {
            //Determine next nodes to go to
            Vector3 targetPosition = GetTargetPosition();
            NodeController currentNodeController = movementController.currentNode.GetComponent<NodeController>();
            var neighbours = currentNodeController.GetAvailableDirections();

            float shortestDistance = Mathf.Infinity;
            string bestDirection = movementController.lastMovingDirection;

            foreach (var (dir, nodeObj) in neighbours)
            {
                if(IsOppositeDirection(dir, movementController.lastMovingDirection))
                {
                    continue; 
                }
                float distance = Vector3.Distance(nodeObj.transform.position, targetPosition); 

                if(distance < shortestDistance)
                {
                    shortestDistance = distance;
                    bestDirection = dir;
                }
            }

            movementController.SetDirection(bestDirection);

        }
        else if (ghostNodesStates == GhostNodesStatesEnum.respawning)
        {
            //Determine quickest direction to home

            //Check if reached home
            if(movementController.currentNode == startingNode)
            {
                Debug.Log("<color=green>Reached home node for ghost: </color>" + ghostType);
                ghostNodesStates = GhostNodesStatesEnum.startNode;
                movementController.SetDirection("up");
                return;
            }

            //Move towards home
            Vector3 targetPosition = startingNode.transform.position;
            NodeController currentNodeController = movementController.currentNode.GetComponent<NodeController>();
            var neighbours = currentNodeController.GetAvailableDirections();

            float shortestDistance = Mathf.Infinity;
            string bestDirection = movementController.lastMovingDirection;

            foreach (var (dir, nodeObj) in neighbours)
            {
                if (IsOppositeDirection(dir, movementController.lastMovingDirection))
                {
                    continue;
                }

                float distance = Vector3.Distance(nodeObj.transform.position, targetPosition);
                
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    bestDirection = dir;
                }
            }

            movementController.SetDirection(bestDirection);
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

    private Vector3 GetTargetPosition()
    {
        switch(ghostType)
        {
            case GhostType.red:
                return playerTransform.position;

            case GhostType.pink:
                return playerTransform.position + (Vector3)(movementController.GetCurrentDirection().normalized * 4f);

            case GhostType.blue:
                if(redGhostTransform != null)
                {
                    Vector3 pacManOffset = playerTransform.position + (Vector3)(movementController.GetCurrentDirection().normalized * 2f);
                    Vector3 diff = pacManOffset - redGhostTransform.position;
                    return redGhostTransform.position + diff * 2;
                }
                return playerTransform.position;

            case GhostType.orange:
                float distance = Vector3.Distance(transform.position, playerTransform.position);
                if(distance > 8f)
                {
                    return playerTransform.position;
                }
                else
                {
                    return ghostNodeRight.transform.position;
                }
        }
        return playerTransform.position;
    }

    private bool IsOppositeDirection(string a, string b)
    {
        return (a == "left" && b == "right")
            || (a == "right" && b == "left")
            || (a == "up" && b == "down")
            || (a == "down" && b == "up");
    }
}
