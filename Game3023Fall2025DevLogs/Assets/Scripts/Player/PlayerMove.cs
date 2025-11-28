using UnityEngine;
using UnityEngine.SceneManagement;
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
        logTimer = logInterval;
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

        // Position logging (logs X, Y and Z components) every logInterval seconds
        if (enablePositionLogging && logInterval > 0f)
        {
            logTimer -= Time.deltaTime;
            if (logTimer <= 0f)
            {
                Vector3 pos = transform.position;
                Debug.Log($"Player position - x: {pos.x:F3}, y: {pos.y:F3}, z: {pos.z:F3}");
                // If a receiver GameObject is assigned, send the Vector3 via SendMessage.
                if (positionReceiver != null)
                {
                    // The receiver should implement a method like:
                    // void OnReceivePlayerPosition(Vector3 pos) { ... }
                    positionReceiver.SendMessage("OnReceivePlayerPosition", pos, SendMessageOptions.DontRequireReceiver);
                }

                // Update GameData.playerPosition so the saved game data matches the logged XYZ
                if (GameData.Instance != null)
                {
                    GameData.Instance.playerPosition = pos;
                }
                else
                {
                    // Fallback: try to find a GameData in the scene and update it
                    GameData gd = FindObjectOfType<GameData>();
                    if (gd != null)
                        gd.playerPosition = pos;
                }

                logTimer = logInterval;
            }
        }
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