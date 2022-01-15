using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttacks : MonoBehaviour
{
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    public void PerformSwipe()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= 2f)
        {
            player.GetComponent<PlayerHP>().TakeDamage(20);
        }
    }
}
