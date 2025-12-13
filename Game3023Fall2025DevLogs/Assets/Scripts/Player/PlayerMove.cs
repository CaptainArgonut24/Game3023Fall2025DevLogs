using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed of the player
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public AudioSource walkingSound;  // Reference to the walking sound
    public AudioClip walkingClip;     // Walking sound clip
    private bool isWalkingSoundPlaying = false;  // Flag to check if sound is already playing

    // Trigger the encounter
    public LayerMask LayerM;

    public float encounterCooldownTime = 2f;
    private float encounterCooldown = 5f;

    [Header("Encounter Settings")]
    [Tooltip("Drag the scene here (by name). Make sure it’s added in Build Settings!")]
    public string encounterSceneName;

    [Header("Position Logging")]
    [Tooltip("Enable periodic logging of the player's X, Y and Z position.")]
    public bool enablePositionLogging = true;
    [Tooltip("Time in seconds between position log entries.")]
    public float logInterval = 3f;
    private float logTimer = 0f;

    [Header("Position Receiver")]
    [Tooltip("Optional: drag an empty GameObject here to receive position updates. It will receive a SendMessage call to 'OnReceivePlayerPosition' with a Vector3 argument.")]
    public GameObject positionReceiver;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        logTimer = Mathf.Max(0.01f, logInterval);

        // Ensure the AudioSource component is set up
        if (walkingSound == null)
        {
            walkingSound = GetComponent<AudioSource>();
        }

        if (rb == null)
            Debug.LogWarning("PlayerMove: Rigidbody2D component not found on GameObject.");
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        bool isMoving = movement.x != 0 || movement.y != 0;

        if (animator != null)
            animator.SetBool("isMoving", isMoving);

        if (spriteRenderer != null)
        {
            if (movement.x > 0)
                spriteRenderer.flipX = false;
            else if (movement.x < 0)
                spriteRenderer.flipX = true;
        }

        if (encounterCooldown > 0f)
            encounterCooldown -= Time.deltaTime;

        EnemyEncounter(isMoving);

        // Position logging (logs X, Y and Z components) every logInterval seconds
        if (enablePositionLogging && logInterval > 0f)
        {
            logTimer -= Time.deltaTime;
            if (logTimer <= 0f)
            {
                Vector3 pos = transform.position;
                Debug.Log($"Player position - x: {pos.x:F3}, y: {pos.y:F3}, z: {pos.z:F3}");

                if (positionReceiver != null)
                {
                    positionReceiver.SendMessage("OnReceivePlayerPosition", pos, SendMessageOptions.DontRequireReceiver);
                }

                // Update GameData.playerPosition so the saved game data matches the logged XYZ
                if (GameData.Instance != null)
                {
                    GameData.Instance.playerPosition = pos;
                }
                else
                {
                    GameData gd = FindObjectOfType<GameData>();
                    if (gd != null)
                        gd.playerPosition = pos;
                }

                logTimer = logInterval;
            }
        }

        // Play/stop walking sound based on movement every frame (not tied to logging)
        if (walkingSound != null)
        {
            if (isMoving)
            {
                if (!isWalkingSoundPlaying)
                {
                    if (walkingClip != null)
                    {
                        walkingSound.clip = walkingClip;
                    }
                    walkingSound.loop = true;
                    walkingSound.Play();
                    isWalkingSoundPlaying = true;
                }
            }
            else
            {
                if (isWalkingSoundPlaying)
                {
                    walkingSound.Stop();
                    isWalkingSoundPlaying = false;
                }
            }
        }

        // Check if the C key is pressed
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (GameData.Instance != null)
            {
                // Use GameData's API to add points (safer than calling a non-existent method)
                GameData.Instance.AddPoints(25);
            }
            else
            {
                Debug.LogWarning("GameData.Instance is null. Cannot add XP.");
            }
        }
    }

    void FixedUpdate()
    {
        if (rb != null)
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void EnemyEncounter(bool isMoving)
    {
        if (encounterCooldown <= 0f && isMoving)
        {
            // use Vector2 for the overlap call
            if (Physics2D.OverlapCircle((Vector2)transform.position, 0.2f, LayerM) != null)
            {
                // 1% chance
                if (Random.Range(1, 101) <= 1)
                {
                    encounterCooldown = encounterCooldownTime;
                    StartEncounter();
                }
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