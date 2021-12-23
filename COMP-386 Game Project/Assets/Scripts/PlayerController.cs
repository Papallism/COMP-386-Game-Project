using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public LayerMask floorLayer;
    private Rigidbody rigidBody;

    public float runSpeed;
    public float jumpSpeed;
    private bool isOnGround;

    public Transform attackArea;
    public float attackRange = 0.5f;
    public LayerMask enemyLayer;

    private const int ATTACK_DAMAGE = 30;
    private float attackRate = 1f;
    private float attackCooldown = 0f;

    public AudioSource audioSource;
    public AudioClip footstepsClip;
    public AudioClip jumpClip;
    public AudioClip punchClip;
    public AudioClip kickClip;
    private float footstepRate = 1f;
    private float footstepCooldown = 0f;

    // Start is called before the first frame update
    void Start()
    {
        this.rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerGrounded();
        Jump();
        Move();
        ThrowPunch();
        ThrowKick();
    }

    // Function to identify if the player character is on the ground
    private void PlayerGrounded()
    {
        // Create a sphere on the players feet and check if it touches the floor layer mask
        if (Physics.CheckSphere(this.transform.position + Vector3.down, 0.1f, floorLayer))
        {
            this.isOnGround = true;
        }
        else
        {
            this.isOnGround = false;
        }

        // Set the jump animation parameter accordingly
        this.animator.SetBool("jump", !isOnGround);
    }

    // Function for when the player character jumps
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && this.isOnGround)
        {
            this.rigidBody.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            // Play the jump audio clip
            audioSource.PlayOneShot(jumpClip);
        }
    }

    // Function for the player character movement
    private void Move()
    {
        // Get user input
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Apply movement on character
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        transform.Translate(movement * runSpeed * Time.deltaTime);

        // Set movement input to animator parameters
        this.animator.SetFloat("horizontal", moveHorizontal);
        this.animator.SetFloat("vertical", moveVertical);

        // Play the footsteps audio clip with a cooldown
        if (Time.time >= footstepCooldown && (moveHorizontal != 0 || moveVertical != 0))
        {
            footstepCooldown = Time.time + footstepRate;
            audioSource.PlayOneShot(footstepsClip);
        }
    }

    // Function for the player character punches
    private void ThrowPunch()
    {
        // Check the punch cooldown
        if (Time.time >= attackCooldown)
        {
            // If Left Mouse Button is clicked, play animation and sound and deal damage
            if (Input.GetMouseButtonDown(0))
            {
                attackCooldown = Time.time + attackRate;
                this.animator.SetTrigger("punch");
                DealDamage();
                audioSource.PlayOneShot(punchClip);
            }
        }
    }

    // Function for the player character kicks
    private void ThrowKick()
    {
        // Check the kick cooldown
        if (Time.time >= attackCooldown)
        {
            // If Right Mouse Button is clicked, play animation and sound and deal damage
            if (Input.GetMouseButtonDown(1))
            {
                attackCooldown = Time.time + attackRate;
                this.animator.SetTrigger("kick");
                DealDamage();
                audioSource.PlayOneShot(kickClip);
            }
        }
    }

    // Function for when the player character deals damage
    private void DealDamage()
    {
        // Detect all enemies that are hit
        Collider[] enemiesHit = Physics.OverlapSphere(attackArea.position, attackRange, enemyLayer);

        // Damage damage to all hit enemies
        foreach (var enemy in enemiesHit)
        {
            enemy.GetComponent<EnemyHP>().TakeDamage(ATTACK_DAMAGE);
        }
    }
}
