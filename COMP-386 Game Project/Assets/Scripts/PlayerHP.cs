using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    public Animator animator;
    public int totalHP = 200;
    private int currentHP;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = totalHP;
    }

    // Function for when the player takes damage
    public void TakeDamage(int damageTaken)
    {
        animator.SetTrigger("take_damage");
        currentHP -= damageTaken;
        if (currentHP <= 0)
        {
            PlayerIsDead();
        }
    }

    // Function for when the player dies
    private void PlayerIsDead()
    {
        animator.SetBool("is_dead", true);
        this.enabled = false;
    }
}
