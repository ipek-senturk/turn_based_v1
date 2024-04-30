using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;

    public InputManager playerInput;

    private Vector2 movementInput;
    private Rigidbody2D rb;
    private Animator animator;
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (playerInput.state == InputManager.ControllerState.Movable)
        {
            movementInput.x = playerInput.GetMovement().x;
            movementInput.y = playerInput.GetMovement().y;

            if (movementInput != Vector2.zero)
            {
                animator.SetFloat("XInput", movementInput.x);
                animator.SetFloat("YInput", movementInput.y);
                
                bool success = TryMove(movementInput);

                if (!success)
                {
                    // Try left and right
                    success = TryMove(new Vector2(movementInput.x, 0));
                    if (!success)
                    {
                        // Try up and down
                        success = TryMove(new Vector2(0, movementInput.y));
                    }
                }
                animator.SetBool("isWalking", success);
            }
            else
            {
                animator.SetBool("isWalking", false);
            }
        } 
        else
        {
            animator.SetBool("isWalking", false);
        }
    }
   
    private bool TryMove(Vector2 direction)
    {
        // Check for potential collisions
        int count = rb.Cast(
            direction, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
            movementFilter, // The settings that determine where a collision can occur on such as layers to collide with
            castCollisions, // List of collisions to store the found collisions into after the Cast is finished
            movementSpeed * Time.fixedDeltaTime + collisionOffset); // The amount to cast equal to the movement plus an offset
        if(count == 0)
        {
            Vector2 moveVector = movementSpeed * Time.fixedDeltaTime * direction;

            // No collisions
            rb.MovePosition(rb.position + moveVector);
            return true;
        } 
        else
        {
            // Collision happened
            return false;
        }
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision");
    }
    */
}
