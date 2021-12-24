using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip deathClip;
    public int totalHP = 100;
    private int currentHP;
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = totalHP;
        audioSource = GetComponent<AudioSource>();
    }

    // Function for taking damage and adjusting the HP accordingly
    public void TakeDamage(int damageTaken)
    {
        this.animator.SetTrigger("take_damage");
        currentHP -= damageTaken;
        slider.value = currentHP;
        if (currentHP <= 0)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(deathClip);
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
