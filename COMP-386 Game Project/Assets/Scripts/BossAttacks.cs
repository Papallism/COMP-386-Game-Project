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
        player.GetComponent<PlayerHP>().TakeDamage(30);
    }

    public void PerformJumpAttack()
    {
        player.GetComponent<PlayerHP>().TakeDamage(50);
    }
}
