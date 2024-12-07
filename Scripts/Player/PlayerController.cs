using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float min_Y, max_Y;

    [SerializeField]
    private GameObject player_Bullet;

    [SerializeField]
    private Transform attact_Point;

    public float attack_Timer = 0.35f;
    private float current_Attack_Timer;
    private bool canAttack;

    public Animator animator; // Reference to the Animator for destruction animation
    private bool isDestroyed = false; // Prevent multiple destruction triggers

    public AudioClip fireSound;  // The sound to play when the player fires
    private AudioSource audioSource;  // The AudioSource component
    void Start()
    {
        current_Attack_Timer = attack_Timer;
        audioSource = GetComponent<AudioSource>();

        if (canAttack)
        {
            canAttack = false;
            attack_Timer = 0f;
            Instantiate(player_Bullet, attact_Point.position, Quaternion.identity);

            // Debug log to check if the method is being triggered
            Debug.Log("Player Bullet Fired, Playing Sound!");

            // Play the fire sound
            audioSource.PlayOneShot(fireSound);
        }

        // Get the AudioSource component
    }


    void Update()
    {
        if (!isDestroyed)
        {
            
            MovePlayer();
            Attack();
        }
    }

    void MovePlayer()
    {
        if (Input.GetAxisRaw("Vertical") > 0f)
        {
            Vector3 temp = transform.position;
            temp.y += speed * Time.deltaTime;

            if (temp.y > max_Y)
                temp.y = max_Y;

            transform.position = temp;
        }
        else if (Input.GetAxisRaw("Vertical") < 0f)
        {
            Vector3 temp = transform.position;
            temp.y -= speed * Time.deltaTime;

            if (temp.y < min_Y)
                temp.y = min_Y;

            transform.position = temp;
        }
    }

    void Attack()
    {
        attack_Timer += Time.deltaTime;
        if (attack_Timer > current_Attack_Timer)
        {
            canAttack = true;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            if (canAttack)
            {
                canAttack = false;
                attack_Timer = 0f;
                Instantiate(player_Bullet, attact_Point.position, Quaternion.identity);
                Debug.Log("Player Bullet Fired");  // Check if bullet is fired

                // Check if AudioSource and FireSound are valid
                if (audioSource != null)
                {
                    if (fireSound != null)
                    {
                        audioSource.PlayOneShot(fireSound);
                        Debug.Log("Sound Played");
                    }
                    else
                    {
                        Debug.LogError("FireSound is not assigned!");
                    }
                }
                else
                {
                    Debug.LogError("AudioSource is not assigned!");
                }
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if hit by enemy bullet or collides with enemy
        if ((collision.CompareTag("Enemy Bullet") || collision.CompareTag("Enemy")) && !isDestroyed)
        {
            Destroy(collision.gameObject); // Destroy the enemy bullet or enemy
            TriggerDestruction(); // Trigger destruction animation and end game
        }
    }

    void TriggerDestruction()
    {
        isDestroyed = true; // Prevent further input or collisions
        animator.SetTrigger("Playergettingdestroyed"); // Trigger destruction animation
        Destroy(gameObject, 1f); // Destroy player after animation (adjust timing to match animation length)

        // End game after destruction
        Invoke("EndGame", 1f); // Adjust time to match animation length
    }

    void EndGame()
    {
        // End the game (you can implement a game over screen or reload the scene)
        Debug.Log("Game Over!");
        // Uncomment the line below to reload the scene
        // UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
