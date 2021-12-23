using UnityEngine;
using UnityEngine.AI;
using System.Collections;


public class EnemyPatrol : MonoBehaviour
{
    public Animator animator;
    public Transform[] patrolPoints;
    public float speed;
    private float distance;
    private int destinationPoint;
    public float idleWait;
    private float currentIdleWait;

    // Start is called before the first frame update
    void Start()
    {
        currentIdleWait = idleWait;
        destinationPoint = 0;
        transform.LookAt(patrolPoints[destinationPoint].position);
    }

    // Update is called once per frame
    void Update()
    {
        Patrol();
        distance = Vector3.Distance(transform.position, patrolPoints[destinationPoint].position);
        if (distance < 1f)
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

    // Function for enemy to move to patrol points
    private void Patrol()
    {
        animator.SetBool("is_walking", true);
        transform.position = Vector3.MoveTowards(transform.position, patrolPoints[destinationPoint].position, speed * Time.deltaTime);
    }

    // Function for selecting the next patrol point
    private void NextPatrolPoint()
    {
        destinationPoint = (destinationPoint + 1) % patrolPoints.Length;
        transform.LookAt(patrolPoints[destinationPoint].position);
    }
}