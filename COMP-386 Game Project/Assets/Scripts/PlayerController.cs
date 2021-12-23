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

    private void PlayerGrounded()
    {
        if (Physics.CheckSphere(this.transform.position + Vector3.down, 0.1f, floorLayer))
        {
            this.isOnGround = true;
        }
        else
        {
            this.isOnGround = false;
        }

        this.animator.SetBool("jump", !isOnGround);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && this.isOnGround)
        {
            this.rigidBody.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            audioSource.PlayOneShot(jumpClip);
        }
    }

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

        if (Time.time >= footstepCooldown && (moveHorizontal != 0 || moveVertical != 0))
        {
            footstepCooldown = Time.time + footstepRate;
            audioSource.PlayOneShot(footstepsClip);
        }
    }

    private void ThrowPunch()
    {
        if (Time.time >= attackCooldown)
        {
            if (Input.GetMouseButtonDown(0))
            {
                attackCooldown = Time.time + attackRate;
                this.animator.SetTrigger("punch");
                DealDamage();
                audioSource.PlayOneShot(punchClip);
            }
        }
    }

    private void ThrowKick()
    {
        if (Time.time >= attackCooldown)
        {
            if (Input.GetMouseButtonDown(1))
            {
                attackCooldown = Time.time + attackRate;
                this.animator.SetTrigger("kick");
                DealDamage();
                audioSource.PlayOneShot(kickClip);
            }
        }
    }

    private void DealDamage()
    {
        // Detect enemies that are hit
        Collider[] enemiesHit = Physics.OverlapSphere(attackArea.position, attackRange, enemyLayer);

        // Damage hit enemies
        foreach (var enemy in enemiesHit)
        {
            enemy.GetComponent<EnemyHP>().TakeDamage(ATTACK_DAMAGE);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackArea == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackArea.position, attackRange);
    }
}
