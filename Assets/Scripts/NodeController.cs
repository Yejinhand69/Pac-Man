using UnityEngine;

public class NodeController : MonoBehaviour
{
    [Header("Game Manager")]
    public GameManager gameManager;

    [Header("Node Movement Flags")]
    [SerializeField] private bool canMoveLeft = false;
    [SerializeField] private bool canMoveRight = false;
    [SerializeField] private bool canMoveUp = false;
    [SerializeField] private bool canMoveDown = false;

    [Header("Node References")]
    [SerializeField] private GameObject nodeLeft;
    [SerializeField] private GameObject nodeRight;
    [SerializeField] private GameObject nodeUp;
    [SerializeField] private GameObject nodeDown;

    [Header("Warp Nodes")]
    public bool isWarpRightNode = false;
    public bool isWarpLeftNode = false;

    [Header("Pellet Node")]
    public bool isPelletNode = false;
    public bool hasPellet = false;

    [Header("Pellet Sprite")]
    public SpriteRenderer pelletSprite;
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if(transform.childCount > 0)
        {
            hasPellet = true;
            isPelletNode = true;
            pelletSprite = GetComponentInChildren<SpriteRenderer>();
        }

        //Down
        RaycastHit2D[] hitsDown;
        hitsDown = Physics2D.RaycastAll(transform.position, -Vector2.up);

        for(int i = 0; i < hitsDown.Length; i++)
        {
            float distance = Mathf.Abs(hitsDown[i].point.y - transform.position.y);
            if(distance < 0.4f)
            {
                canMoveDown = true;
                nodeDown = hitsDown[i].collider.gameObject;
            }
        }

        //Up
        RaycastHit2D[] hitsUp;
        hitsUp = Physics2D.RaycastAll(transform.position, Vector2.up);

        for (int i = 0; i < hitsUp.Length; i++)
        {
            float distance = Mathf.Abs(hitsUp[i].point.y - transform.position.y);
            if (distance < 0.4f)
            {
                canMoveUp = true;
                nodeUp = hitsUp[i].collider.gameObject;
            }
        }

        //Right
        RaycastHit2D[] hitsRight;
        hitsRight = Physics2D.RaycastAll(transform.position, Vector2.right);

        for (int i = 0; i < hitsRight.Length; i++)
        {
            float distance = Mathf.Abs(hitsRight[i].point.x - transform.position.x);
            if (distance < 0.4f)
            {
                canMoveRight = true;
                nodeRight = hitsRight[i].collider.gameObject;
            }
        }

        //Left
        RaycastHit2D[] hitsLeft;
        hitsLeft = Physics2D.RaycastAll(transform.position, -Vector2.right);

        for (int i = 0; i < hitsLeft.Length; i++)
        {
            float distance = Mathf.Abs(hitsLeft[i].point.x - transform.position.x);
            if (distance < 0.4f)
            {
                canMoveLeft = true;
                nodeLeft = hitsLeft[i].collider.gameObject;
            }
        }
    }

    public GameObject GetNodeFromDirection(string direction)
    {
        if(direction == "left" && canMoveLeft)
        {
            return nodeLeft;
        }
        else if (direction == "right" && canMoveRight)
        {
            return nodeRight;
        }
        else if (direction == "up" && canMoveUp)
        {
            return nodeUp;
        }
        else if (direction == "down" && canMoveDown)
        {
            return nodeDown;
        }
        else
        {
            return null;
        }
    }

    public (string direction, GameObject node)[] GetAvailableDirections()
    {
        var directions = new System.Collections.Generic.List<(string, GameObject)>();

        if (canMoveLeft && nodeLeft != null)
            directions.Add(("left", nodeLeft));
        if (canMoveRight && nodeRight != null)
            directions.Add(("right", nodeRight));
        if (canMoveUp && nodeUp != null)
            directions.Add(("up", nodeUp));
        if (canMoveDown && nodeDown != null)
            directions.Add(("down", nodeDown));

        return directions.ToArray();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && hasPellet)
        {
            hasPellet = false;
            pelletSprite.enabled = false;
            gameManager.CollectedPellet(this);
        }
    }
}
