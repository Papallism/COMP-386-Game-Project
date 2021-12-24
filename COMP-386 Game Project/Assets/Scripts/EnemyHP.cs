using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public Animator animator;

    public int totalHP = 100;
    private int currentHP;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = totalHP;
    }

    // Function for taking damage and adjusting the HP accordingly
    public void TakeDamage(int damageTaken)
    {
        this.animator.SetTrigger("take_damage");
        currentHP -= damageTaken;

        if (currentHP <= 0)
        {
            EnemyIsDead();
        }
    }

    // Function for when the enemy dies
    private void EnemyIsDead()
    {
        this.animator.SetBool("is_dead", true);
        this.GetComponent<EnemyPatrol>().enabled = false;
        this.enabled = false;
    }
}
