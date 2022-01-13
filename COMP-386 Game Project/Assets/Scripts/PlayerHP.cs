using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip deathClip;
    public int totalHP = 200;
    public int currentHP;
    private bool isDead = false;
    public Slider hpSlider;
    public Text gameOverText;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = totalHP;
        audioSource = GetComponent<AudioSource>();
        Cursor.visible = false;
    }

    // Function for when the player takes damage
    public void TakeDamage(int damageTaken)
    {
        if (!isDead)
        {
            animator.SetTrigger("take_damage");
            currentHP -= damageTaken;
            hpSlider.value = (float)currentHP / totalHP;
            if (currentHP <= 0)
            {
                isDead = true;
                PlayerIsDead();
            }
        }
    }

    // Function for when the player dies
    private void PlayerIsDead()
    {
        this.enabled = false;
        this.GetComponent<PlayerController>().enabled = false;
        animator.SetBool("is_dead", true);
        audioSource.PlayOneShot(deathClip);
        gameOverText.text = "GAME OVER";
        Cursor.visible = true;
    }
}
