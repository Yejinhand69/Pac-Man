using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Controller")]
    MovementController movementController;

    [Header("Sprite Renderer Reference")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    [Header("Collision Detection")]
    [SerializeField] private bool hasCollided = false;
    void Awake()
    {
        movementController = GetComponent<MovementController>();
        movementController.lastMovingDirection = "left";
        hasCollided = false;
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
        if(collision.CompareTag("Enemy") && hasCollided == false)
        {
            //Game restart logic here
            hasCollided = true;
            Debug.Log("<color=red>Player collided with an enemy. Restarting game...</color>");
            SceneManager.LoadScene("Game");
        }
    }
}
