using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    PlayerSSUIController playerSSUI;

    [Header("Movement")]
    public float maxMoveSpeed;
    float moveSpeedX;
    float moveSpeedZ;
    Vector3 velocity;

    [Header("Camera")]
    public GameObject myCam;
    public float mouseSensX;
    public float mouseSenseXMax;
    public float mouseSenseXStart;
    public float mouseSensY;
    public float mouseSenseYMax;
    public float mouseSenseYStart;
    float mouseSpeedX;
    float mouseSpeedY;
    private Vector3 rotateValueX;
    private Vector3 rotateValueY;
    public float viewRangeUp;
    public float viewRangeDown;
    public int yInvert = -1;
    private float rotY;
    private float rotX;
    Quaternion localCamRotation;
    Quaternion localPlayRotation;
    public GameObject playerScreenStatic;
    public float shootingDistance;

    [Header("UI")]
    public Image fadeToBlack;

    [Header("Stats")]
    public int stageCount = 1;

    // Use this for initialization
    void Start()
    {
        playerSSUI = GameObject.FindGameObjectWithTag("SSUI").GetComponent<PlayerSSUIController>();
        Cursor.visible = false;
        playerSSUI.isPaused = true;
    }

    private void FixedUpdate()
    {
        Movement();
        CameraRotation();
        Shooting();
    }

    public void Movement()
    {
        if (playerSSUI.isPaused)
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");
            if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0 || Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical") < 0)
            {
                gameObject.GetComponent<Rigidbody>().velocity = transform.forward * moveZ * maxMoveSpeed;
                gameObject.GetComponent<Rigidbody>().velocity += transform.right * moveX * maxMoveSpeed;
            }
        }
    }

    public void CameraRotation()
    {
        if (playerSSUI.isPaused)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            rotY += mouseX * mouseSensX;
            rotX += (mouseY * (playerSSUI.invertYToggle.isOn ? -yInvert : yInvert)) * mouseSensY;

            rotX = Mathf.Clamp(rotX, viewRangeDown, viewRangeUp);

            localCamRotation = Quaternion.Euler(rotX, myCam.transform.rotation.y, myCam.transform.rotation.z);
            localPlayRotation = Quaternion.Euler(transform.rotation.x, rotY, transform.rotation.z);
            myCam.transform.localRotation = localCamRotation;
            transform.localRotation = localPlayRotation;
        }
    }

    public void Shooting()
    {
        if (playerSSUI.isPaused)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                RaycastShooting();
            }
        }
    }

    public void RaycastShooting()
    {
        if (playerSSUI.isPaused)
        {
            float x = Screen.width / 2;
            float y = Screen.height / 2;

            Ray ray = gameObject.GetComponentInChildren<Camera>().ScreenPointToRay(new Vector2(x, y));
            Debug.DrawRay(ray.origin, ray.direction * shootingDistance, Color.green);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, shootingDistance) && hit.collider.gameObject.GetComponent<TriggerEvent>())
            {
                TriggerEvent trigEv = hit.collider.gameObject.GetComponent<TriggerEvent>();
                PlayAudio();
                trigEv.TriggeredEvent();
                Debug.Log("Hit");
            }
        }
    }

    public void PlayAudio()
    {
        if (playerSSUI.isPaused)
        {
            this.gameObject.GetComponent<AudioSource>().Play();
        }
    }
}
