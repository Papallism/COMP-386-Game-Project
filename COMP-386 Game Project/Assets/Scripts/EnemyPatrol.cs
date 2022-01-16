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
    public int attackDamage = 50;
    private float hitDelayTime = 0.75f;
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
        IsPlayerInAggroRange();
        if (isPlayerInRange && playerObject.GetComponent<PlayerHP>().currentHP > 0)
        {
            Vector3 playerDirection = player.transform.position;
            playerDirection.y = transform.position.y;
            transform.LookAt(playerDirection);
            if (!IsPlayerInMeleeRange())
            {
                animator.SetBool("is_walking", true);
                transform.position = Vector3.MoveTowards(transform.position, player.position, movementSpeed * Time.deltaTime);
            }
            if (IsPlayerInMeleeRange())
            {
                this.GetComponent<Rigidbody>().velocity = Vector3.zero;
                if (attackWait <= 0)
                {
                    attackWait = attackCooldown;
                    animator.SetTrigger("attack");
                    StartCoroutine(HitDelay());
                }
                else
                {
                    attackWait -= Time.deltaTime;
                }
            }
        }
        else
        {
            distanceToPatrolPoint = Vector2.Distance(transform.position, patrolPoints[destinationPoint].position);
            if (distanceToPatrolPoint > 1f)
            {
                Patrol();
            }
            if (distanceToPatrolPoint <= 1f)
            {
                if (currentIdleWait <= 0)
                {
                    NextPatrolPoint();
                    Patrol();
                    currentIdleWait = idleWait;
                }
                else
                {
                    this.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    animator.SetBool("is_walking", false);
                    currentIdleWait -= Time.deltaTime;
                }
            }
        }
    }

    private IEnumerator HitDelay()
    {
        yield return new WaitForSeconds(hitDelayTime);

        audioSource.PlayOneShot(attackClip);
    }

    // Function to check if player is in melee range
    private bool IsPlayerInMeleeRange()
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
    private void IsPlayerInAggroRange()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= 15f)
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
        Vector3 lookDirection = patrolPoints[destinationPoint].position;
        lookDirection.y = transform.position.y;
        transform.LookAt(lookDirection);
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