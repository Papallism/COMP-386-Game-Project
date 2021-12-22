using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public LayerMask layerMask;
    private Rigidbody rigidBody;

    public float runSpeed;
    public float jumpSpeed;
    private bool isOnGround;

    // Start is called before the first frame update
    void Start()
    {
        this.rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerGrounded();
        Jump();
        Move();
    }

    private void PlayerGrounded()
    {
        if (Physics.CheckSphere(this.transform.position + Vector3.down, 0.1f, layerMask))
        {
            this.isOnGround = true;
        }
        else
        {
            this.isOnGround = false;
        }

        this.animator.SetBool("jump", !isOnGround);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && this.isOnGround)
        {
            this.rigidBody.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        }
    }

    private void Move()
    {
        // Get user input
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Apply movement on character
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        transform.Translate(movement * runSpeed * Time.deltaTime);

        // Set movement input to animator parameters
        this.animator.SetFloat("horizontal", moveHorizontal);
        this.animator.SetFloat("vertical", moveVertical);
    }
}
