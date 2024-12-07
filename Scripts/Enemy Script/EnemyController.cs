using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3f; // Speed at which the enemy moves
    public GameObject EnemyBullet; // Assign enemy bullet prefab in the inspector
    public Transform firePoint; // Where the bullets are fired from
    public float fireRate = 1.5f; // Time interval between shots
    public Animator animator; // Reference to Animator for triggering destruction
    public float boundaryX = -12f; // X position to destroy the enemy when it leaves the screen

    private float fireTimer;
    private bool isDestroyed = false; // Prevent multiple destruction triggers

    void Start()
    {
        fireTimer = 0f;

        // Ensure the enemy is oriented correctly
        transform.rotation = Quaternion.Euler(0f, 0f, 90f); // Face upward (if needed)
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Only move and fire if not destroyed
        if (!isDestroyed)
        {
            MoveEnemy();
            HandleShooting();
        }

        // Destroy the enemy if it goes out of bounds
        if (transform.position.x <= boundaryX)
        {
            Destroy(gameObject);
        }
    }

    // Moves the enemy strictly from right to left
    void MoveEnemy()
    {
        // Move left in world space, ignoring local rotation
        transform.position += Vector3.left * speed * Time.deltaTime;
    }

    // Handles enemy shooting logic
    void HandleShooting()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            Fire();
            fireTimer = 0f;
        }
    }

    // Fires an enemy bullet
    void Fire()
    {
        // Instantiate bullet at the fire point
        Instantiate(EnemyBullet, firePoint.position, Quaternion.identity);
    }

    public AudioClip collisionSound;

    private AudioSource audioSource;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the enemy is hit by a player's bullet
        if (collision.CompareTag("Player Bullet") && !isDestroyed)
        {
            Destroy(collision.gameObject); // Destroy player's bullet
            TriggerDestruction(); // Trigger destruction animation

            if (audioSource != null && collisionSound != null)
            {
                audioSource.PlayOneShot(collisionSound);
            }
        }
    }

    private void OnDestroy()
    {
      if(ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(ScoreValue);
        }
    }

    public int ScoreValue = 10;
    // Triggers the destruction animation and removes the enemy
    void TriggerDestruction()
    {
        isDestroyed = true; // Mark as destroyed to prevent further logic
        animator.SetTrigger("DestroyTrigger"); // Play destroy animation
        Destroy(gameObject, 0.5f); // Destroy enemy after animation (adjust timing if needed)
    }
}
