using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    public float collisionOffset = 0.1f;
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
            direction,
            movementFilter,
            castCollisions,
            movementSpeed * Time.fixedDeltaTime + collisionOffset);

        if (count == 0)
        {
            Vector2 moveVector = movementSpeed * Time.fixedDeltaTime * direction;
            rb.MovePosition(rb.position + moveVector);
            return true;
        }
        else
        {
            // If collision happens, make small adjustments to avoid getting stuck
            for (int i = 0; i < castCollisions.Count; i++)
            {
                RaycastHit2D hit = castCollisions[i];
                Vector2 hitNormal = hit.normal;
                Vector2 adjustment = hitNormal * (collisionOffset + 0.01f);
                rb.MovePosition(rb.position + adjustment);
            }
            return false;
        }
    }

}
