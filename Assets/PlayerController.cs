using UnityEngine;

public class PlayerController : MonoBehaviour
{
    MovementController movementController;
    void Start()
    {
        movementController = GetComponent<MovementController>();
    }
    void Update()
    {
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
    }
}
