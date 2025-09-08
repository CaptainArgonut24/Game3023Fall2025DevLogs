using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed of the player
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Get the Rigidbody2D, Animator, and SpriteRenderer components
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Get input from the player (arrow keys or WASD)
        movement.x = Input.GetAxisRaw("Horizontal"); // Left and Right movement 
        movement.y = Input.GetAxisRaw("Vertical");   // Up and Down movement

        // Check if the player is moving
        bool isMoving = movement.x != 0 || movement.y != 0;

        // Update the animator parameter to switch between idle and walking animations
        animator.SetBool("isMoving", isMoving);

        // Flip sprite based on movement.x
        if (movement.x > 0)
        {
            spriteRenderer.flipX = false; // facing right
        }
        else if (movement.x < 0)
        {
            spriteRenderer.flipX = true; // facing left
        }
    }

    void FixedUpdate()
    {
        // Move the player based on input
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}