using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    public float speed = 5f; // Speed of the enemy bullet

    void Start()
    {
        // Correct the rotation to face left if needed
        transform.rotation = Quaternion.Euler(0f, 0f, 90f);
    }

    void Update()
    {
        // Move the bullet to the left
        transform.Translate(Vector2.left * speed * Time.deltaTime, Space.World);

        // Destroy if it moves off-screen
        if (transform.position.x < -10f || transform.position.x > 10f ||
            transform.position.y < -6f || transform.position.y > 6f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check for collision with the player
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject); // Destroy the bullet

            // Trigger player destruction animation
            Animator playerAnimator = collision.GetComponent<Animator>();
            if (playerAnimator != null)
            {
                playerAnimator.SetTrigger("Playergettingdestroyed"); // Trigger animation
            }

            // Destroy the player object after animation
            Destroy(collision.gameObject, 0.5f); // Adjust time to match animation length

            // End the game
            GameOver();
        }
    }

    void GameOver()
    {
        // Placeholder for game-over logic
        Debug.Log("Game Over");
        // Implement actual game-ending logic (e.g., load a game-over screen or stop gameplay)
    }
}
