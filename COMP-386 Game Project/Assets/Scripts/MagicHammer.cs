using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MagicHammer : MonoBehaviour
{
    public Text hammerText;
    public AudioSource audioSource;
    public AudioClip hammerPickUpClip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0.2f, 0.2f, 0);

        if (Vector3.Distance(GameObject.FindWithTag("Player").transform.position, this.transform.position) < 3f ||
            Vector3.Distance(GameObject.FindWithTag("Player").transform.position, this.transform.position) < -3f)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                hammerText.text = "";
                audioSource.PlayOneShot(hammerPickUpClip);
                GameObject.FindWithTag("Hammer").SetActive(false);
                GameObject.FindWithTag("Player").GetComponent<PlayerController>().solvedPuzzle = true;
            }
        }
    }
}
