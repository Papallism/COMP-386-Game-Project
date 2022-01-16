using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip deathClip;
    public AudioClip healthPickUpClip;
    public int totalHP = 200;
    public int currentHP;
    private bool isDead = false;
    public Slider hpSlider;
    public Text gameOverText;
    public Text objective;
    public int healthPickUp = 50;
    public GameObject skeleton;
    public GameObject boss;
    private static bool gamePaused = false;
    public GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = totalHP;
        audioSource = GetComponent<AudioSource>();
        Cursor.visible = false;
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        int skeletonHP = skeleton.GetComponent<EnemyHP>().currentHP;
        int bossHP = boss.GetComponent<EnemyHP>().currentHP;
        if (skeletonHP <= 0)
        {
            objective.text = "Find the magic hammer to open the gate";
        }
        if (skeletonHP <= 0 && this.GetComponent<PlayerController>().solvedPuzzle)
        {
            objective.text = "Kill the Maw";
        }
        if (skeletonHP <= 0 && bossHP <= 0)
        {
            Cursor.visible = true;
            gameOverText.text = "YOU WON!";
            objective.text = "Objectives completed";
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Menu();
        }
    }

    public void Menu()
    {
        if (!gamePaused)
        {
            gamePaused = true;
            Cursor.visible = true;
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            gamePaused = false;
            Cursor.visible = false;
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void RestartGame()
    {
        gamePaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "HealthPickUp")
        {
            if (currentHP < totalHP)
            {
                PickUpHealth();
                collision.gameObject.GetComponent<HealthPickUp>().enabled = false;
                collision.gameObject.SetActive(false);
            }
        }
    }

    public void PickUpHealth()
    {

        currentHP += healthPickUp;
        audioSource.PlayOneShot(healthPickUpClip);
        hpSlider.value = (float)currentHP / totalHP;

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
        pauseMenu.SetActive(true);
    }
}
