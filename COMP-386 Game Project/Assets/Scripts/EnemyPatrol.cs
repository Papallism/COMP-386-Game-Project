using UnityEngine;
using UnityEngine.AI;
using System.Collections;


public class EnemyPatrol : MonoBehaviour
{
    public Animator animator;
    public Transform[] patrolPoints;
    public float movementSpeed;
    public bool isPlayerInRange = false;
    private Transform player;
    public float distanceToPatrolPoint;
    public float distanceToPlayer;
    private int destinationPoint;
    public float idleWait;
    private float currentIdleWait;
    public AudioSource audioSource;
    public AudioClip attackClip;
    public float attackCooldown = 3;
    public float attackWait;
    public int attackDamage = 30;
    private GameObject playerObject;

    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.transform;
        currentIdleWait = idleWait;
        destinationPoint = 0;
        transform.LookAt(patrolPoints[destinationPoint].position);
        attackWait = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfPlayerIsInAggroRange();
        if (isPlayerInRange && playerObject.GetComponent<PlayerHP>().currentHP > 0)
        {
            animator.SetBool("is_walking", true);
            transform.LookAt(player.position);
            transform.position = Vector3.MoveTowards(transform.position, player.position, movementSpeed * Time.deltaTime);
            if (IsPlayerIsInMeleeRange())
            {
                if (attackWait <= 0)
                {
                    attackWait = attackCooldown;
                    animator.SetTrigger("attack");
                    audioSource.PlayOneShot(attackClip);
                    playerObject.GetComponent<PlayerHP>().TakeDamage(attackDamage);
                }
                else
                {
                    attackWait -= Time.deltaTime;
                }
            }
        }
        else
        {
            Patrol();
            distanceToPatrolPoint = Vector2.Distance(transform.position, patrolPoints[destinationPoint].position);
            if (distanceToPatrolPoint < 0.5f)
            {
                if (currentIdleWait <= 0)
                {
                    NextPatrolPoint();
                    Patrol();
                    currentIdleWait = idleWait;
                }
                else
                {
                    animator.SetBool("is_walking", false);
                    currentIdleWait -= Time.deltaTime;
                }
            }
        }
    }

    // Function to check if player is in melee range
    private bool IsPlayerIsInMeleeRange()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= 2f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Function to check if player is in aggro range
    private void CheckIfPlayerIsInAggroRange()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= 10f)
        {
            isPlayerInRange = true;
        }
        else
        {
            isPlayerInRange = false;
        }
    }

    // Function for enemy to move to patrol points
    private void Patrol()
    {
        animator.SetBool("is_walking", true);
        transform.LookAt(patrolPoints[destinationPoint].position);
        transform.position = Vector3.MoveTowards(transform.position, patrolPoints[destinationPoint].position, movementSpeed * Time.deltaTime);
    }

    // Function for selecting the next patrol point
    private void NextPatrolPoint()
    {
        destinationPoint = (destinationPoint + 1) % patrolPoints.Length;
        transform.LookAt(patrolPoints[destinationPoint].position);
    }
}