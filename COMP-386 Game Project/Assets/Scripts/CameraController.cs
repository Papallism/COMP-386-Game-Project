using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerController playerController;
    public float sensitivity = 200;
    public float angle = 40;

    private float horizontalRotation;
    private float verticalRotation;

    // Start is called before the first frame update
    void Start()
    {
        this.verticalRotation = this.transform.localEulerAngles.x;
        this.horizontalRotation = this.transform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        AdjustCamera();
    }

    // Adjust the main camera view
    private void AdjustCamera()
    {
        float horizontalMouseMovement = Input.GetAxis("Mouse X");
        float verticalMouseMovement = -Input.GetAxis("Mouse Y");

        this.horizontalRotation += horizontalMouseMovement * this.sensitivity * Time.deltaTime;
        this.verticalRotation += verticalMouseMovement * this.sensitivity * Time.deltaTime;
        this.verticalRotation = Mathf.Clamp(this.verticalRotation, -angle, +angle);

        this.transform.localRotation = Quaternion.Euler(this.verticalRotation, 0f, 0f);
        this.playerController.transform.rotation = Quaternion.Euler(0f, this.horizontalRotation, 0f);
    }
}
