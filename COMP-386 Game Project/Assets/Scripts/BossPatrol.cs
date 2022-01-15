using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPatrol : MonoBehaviour
{
    public GameObject boss;
    public GameObject player;
    public Animator bossAnimator;
    public float movementSpeed = 4f;
    public float distanceToPlayer;
    public bool isPlayerInPatrolArea;
    private float attackWait = 0f;
    private float attackCooldown = 3f;

    // Start is called before the first frame update
    void Start()
    {
        isPlayerInPatrolArea = false;
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = CalculateDistanceToPlayer();

        if (isPlayerInPatrolArea)
        {
            BossLooksAtPlayer();
        }
        if (isPlayerInPatrolArea && distanceToPlayer > 2.5f)
        {
            bossAnimator.SetBool("Run", true);
            boss.transform.position += boss.transform.forward * movementSpeed * Time.deltaTime;
            //boss.transform.position = Vector3.MoveTowards(boss.transform.position, player.transform.position, movementSpeed * Time.deltaTime);
        }
        if (isPlayerInPatrolArea && distanceToPlayer <= 2.5f)
        {
            if (attackWait <= 0)
            {
                int attackChoice = Random.Range(1, 3);
                Debug.Log(attackChoice);
                switch (attackChoice)
                {
                    case 1:
                        attackWait = attackCooldown;
                        bossAnimator.SetTrigger("JumpAttack");
                        //StartCoroutine(HitDelay());
                        break;
                    case 2:
                        attackWait = attackCooldown;
                        bossAnimator.SetTrigger("Attack");
                        break;
                }
            }
            else
            {
                attackWait -= Time.deltaTime;
            }
        }
    }

    private void BossLooksAtPlayer()
    {
        Vector3 playerDirection = player.transform.position;
        playerDirection.y = boss.transform.position.y;
        boss.transform.LookAt(playerDirection);
    }

    private float CalculateDistanceToPlayer()
    {
        return Vector3.Distance(boss.transform.position, player.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerInPatrolArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerInPatrolArea = false;
            bossAnimator.SetBool("Run", false);
        }
    }
}
