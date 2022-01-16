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
    public int currentHP;
    private bool isDead = false;
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
        if (!isDead)
        {
            this.animator.SetTrigger("take_damage");
            currentHP -= damageTaken;
            slider.value = (float)currentHP / totalHP;
            if (currentHP <= 0)
            {
                isDead = true;
                audioSource.Stop();
                audioSource.PlayOneShot(deathClip);
                EnemyIsDead();
            }
        }
    }

    // Function for when the enemy dies
    private void EnemyIsDead()
    {
        if (this.tag == "Boss")
        {
            GameObject.FindWithTag("Boss Patrol Area").GetComponent<BossPatrol>().enabled = false;
        }
        else if (this.tag == "Skeleton Zombie")
        {
            GameObject.FindWithTag("Skeleton Zombie").GetComponent<EnemyPatrol>().enabled = false;
        }
        this.slider.gameObject.SetActive(false);
        this.animator.SetBool("is_dead", true);
    }
}
