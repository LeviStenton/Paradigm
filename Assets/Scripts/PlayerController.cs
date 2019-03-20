using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float maxMoveSpeed;
    float moveSpeedX;
    float moveSpeedZ;
    Vector3 velocity;
    float gravity = -20;   

    public GameObject myCam;
    public float mouseSensX;
    public float mouseSensY;
    private Vector3 rotateValueX;
    private Vector3 rotateValueY;

    public float shootingDistance;

    // Use this for initialization
    void Start()
    {        
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        Movement();
        CameraRotation();
        Shooting();

    }

    public void Movement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0 || Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical") < 0)
        {
            gameObject.GetComponent<Rigidbody>().velocity = transform.forward * moveZ * maxMoveSpeed;
            gameObject.GetComponent<Rigidbody>().velocity += transform.right * moveX * maxMoveSpeed;
        }
    }

    public void CameraRotation()
    {
        float mouseSpeedX = Input.GetAxis("Mouse Y") * mouseSensY;
        float mouseSpeedY = Input.GetAxis("Mouse X") * mouseSensX;
        rotateValueX = new Vector3(mouseSpeedX * -1, 0, 0);
        myCam.transform.eulerAngles = myCam.transform.eulerAngles + rotateValueX;
        rotateValueY = new Vector3(0, mouseSpeedY * +1, 0);
        transform.eulerAngles = transform.eulerAngles + rotateValueY;
    }

    public void Shooting()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastShooting();
        }
    }

    public void RaycastShooting()
    {
        float x = Screen.width / 2;
        float y = Screen.height / 2;

        Ray ray = gameObject.GetComponentInChildren<Camera>().ScreenPointToRay(new Vector2(x, y));
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, shootingDistance))
        {
            Debug.Log("Hit");
        }
    }
}
