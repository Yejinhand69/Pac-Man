using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IGameEventListener
{
    [Header("Movement Controller")]
    MovementController movementController;

    [Header("Sprite Renderer Reference")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    [Header("Collision Detection")]
    [SerializeField] private bool hasCollided = false;

    [Header("Boolean")]
    [SerializeField] private bool hasPowerUp = false;

    [Header("Game Event")]
    [SerializeField] private GameEvent frightenedListener;
    void Awake()
    {
        movementController = GetComponent<MovementController>();
        movementController.lastMovingDirection = "left";
        hasCollided = false;
        hasPowerUp = false;
    }

    private void OnEnable()
    {
        frightenedListener.RegisterListener(this);
    }
    private void OnDisable()
    {
        frightenedListener.UnregisterListener(this);
    }

    public void OnEventRaised(GameEvent gameEvent, Component sender, object[] args)
    {
        if (gameEvent == frightenedListener)
        {
            StartCoroutine(PowerUpCoroutine());
        }
    }

    private IEnumerator PowerUpCoroutine()
    {
        hasPowerUp = true;
        
        yield return new WaitForSeconds(3f);

        hasPowerUp = false;
    }
    void Update()
    {
        animator.SetBool("moving", true);

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movementController.SetDirection("left");
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            movementController.SetDirection("right");
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            movementController.SetDirection("up");
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            movementController.SetDirection("down");
        }

        bool flipX = false;
        bool flipY = false;

        if(movementController.lastMovingDirection == "left")
        {
            animator.SetInteger("direction", 0);    
        }
        else if (movementController.lastMovingDirection == "right")
        {
            animator.SetInteger("direction", 0);
            flipX = true;
        }
        else if (movementController.lastMovingDirection == "up")
        {
            animator.SetInteger("direction", 1);
        }
        else if (movementController.lastMovingDirection == "down")
        {
            animator.SetInteger("direction", 1);
            flipY = true;
        }

        spriteRenderer.flipX = flipX;
        spriteRenderer.flipY = flipY;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Player losing state
        if(collision.CompareTag("Enemy") && hasCollided == false && hasPowerUp == false)
        {
            //Game restart logic here
            hasCollided = true;
            SceneManager.LoadScene("Game");
        }
        else if(collision.CompareTag("Enemy") && hasCollided == false && hasPowerUp == true)
        {
            //Player defeated an enemy
            Debug.Log("<color=green>Player defeated an enemy!</color>");
            if(collision.gameObject != null)
            Destroy(collision.gameObject);
        }
    }
}
