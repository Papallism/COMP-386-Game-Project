using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttacks : MonoBehaviour
{
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PerformAttack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= 2f)
        {
            player.GetComponent<PlayerHP>().TakeDamage(30);
        }
    }

    public void PerformJumpAttack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= 2.5f)
        {
            player.GetComponent<PlayerHP>().TakeDamage(50);
        }
    }
}
