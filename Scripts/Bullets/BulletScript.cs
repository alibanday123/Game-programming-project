using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 5f; // Speed of the bullet
    public float deactivate_Timer = 3f; // Time after which the bullet deactivates

    void Start()
    {
        Invoke("DeactivateGameObject", deactivate_Timer); // Deactivate bullet after a timer
    }

    void Update()
    {
        Move(); // Continuously move the bullet
    }

    void Move()
    {
        Vector3 temp = transform.position;
        temp.x += speed * Time.deltaTime; // Move to the right
        transform.position = temp;
    }

    void DeactivateGameObject()
    {
        Destroy(gameObject); // Destroy the bullet after a set time
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the bullet hits an enemy
        if (collision.CompareTag("Enemy"))
        {
            // Trigger enemy destruction animation
            Animator enemyAnimator = collision.GetComponent<Animator>();
            if (enemyAnimator != null)
            {
                enemyAnimator.SetTrigger("DestroyTrigger"); // Trigger enemy destruction animation
            }

            // Destroy the enemy object after the animation
            Destroy(collision.gameObject, 0.5f); // Adjust delay to match animation length

            // Destroy the bullet
            Destroy(gameObject);
        }
    }
}
