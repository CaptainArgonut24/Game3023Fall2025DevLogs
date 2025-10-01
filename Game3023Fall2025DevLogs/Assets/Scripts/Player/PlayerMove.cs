using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed of the player
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // Trigger the encounter
    public LayerMask LayerM;

    public float encounterCooldownTime = 2f;
    private float encounterCooldown = 5f;

    [Header("Encounter Settings")]
    [Tooltip("Drag the scene here (by name). Make sure it’s added in Build Settings!")]
    public string encounterSceneName;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        bool isMoving = movement.x != 0 || movement.y != 0;
        animator.SetBool("isMoving", isMoving);

        if (movement.x > 0)
            spriteRenderer.flipX = false;
        else if (movement.x < 0)
            spriteRenderer.flipX = true;

        if (encounterCooldown > 0)
            encounterCooldown -= Time.deltaTime;

        EnemyEncounter(isMoving);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void EnemyEncounter(bool isMoving)
    {
        if (encounterCooldown <= 0 && isMoving &&
            Physics2D.OverlapCircle(transform.position, 0.2f, LayerM) != null)
        {
            if (Random.Range(1, 101) <= 1)
            {
                encounterCooldown = encounterCooldownTime;
                StartEncounter();
            }
        }
    }

    private void StartEncounter()
    {
        if (!string.IsNullOrEmpty(encounterSceneName))
        {
            SceneManager.LoadScene(encounterSceneName);
        }
        else
        {
            Debug.LogWarning("Encounter Scene is not set in the Inspector!");
        }
    }
}
