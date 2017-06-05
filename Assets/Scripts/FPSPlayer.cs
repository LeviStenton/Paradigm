using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class FPSPlayer : MonoBehaviour {

    // Camera variables
    [HideInInspector]
    public Camera firstPersonCamera;
    Transform firstPersonCameraTransform;
    [Header("Camera")]
    public float cameraHeight = 1.6f;
    public float mouseSensitivityX = 3f;
    public float mouseSensitivityY = 3f;
    public bool smoothMouse = true;
    public int mouseSmoothingFrames = 5;
    float[] mouseSmoothingX;
    float[] mouseSmoothingY;
    public float cameraTilt = 0.2f;
    public float fieldOfView = 80f;
    public float fieldOfViewVerticalMultiplier = 0.7f;
    [HideInInspector]
    public float fieldOfViewDelta = 0f;
    float curFOVDelta = 0f;
    public float FOVShiftSpeed = 10f;
    public float cameraMinAngle = -75f;
    public float cameraMaxAngle = 85f;
    float mouseX = 0f;
    float mouseY = 0f;
    float mouseInputX = 0f;
    float mouseInputY = 0f;
    float mouseYRotation = 0f;
    public bool invertMouseY = false;
    [Space(5)]

    // Size variables
    [Header("Size")]
    public float colliderHeight = 1.8f;
    public float colliderRadius = 0.35f;
    CapsuleCollider myCollider;
    [Space(5)]

    // Movement variables
    [Header("Movement")]
    public float forwardsSpeed = 1.5f;
    public float sidewaysSpeedMultiplier = 0.8f;
    public bool canSprint = false;
    public float sprintSpeedMultiplier = 2f;
    public float sprintFOVKick = 8f;
    public bool canJump = false;
    public float jumpForce = 1.3f;
    public float jumpForwardForce = 2f;
    public float jumpSprintForce = 3f;
    public float jumpSprintVerticalForceMultiplier = 0.7f;
    public bool noYAxisDrag = true;
    Transform myTransform;
    Rigidbody myRigidbody;
    Vector2 movementInput = new Vector2();
    bool grounded;
    bool sprinting;
    bool onWall;
    bool jumpInput;
    [Space(5)]

    // WallClimb variables
    [Header("Freerunning")]
    public bool canWallClimb = false;
    public float wallClimbFeetHeight = 0.3f;
    public float wallClimbShoulderHeight = 1.5f;
    public float wallClimbSpeed = 1.3f;
    public float wallClimbMaxDistance = 0.1f;
    public float wallClimbMouseXMultiplier = 0.5f;
    public float wallClimbMouseYMultiplier = 0.5f;
    public float wallClimbFinishJumpForce;
    public float minimumYVelocity = -3f;
    bool wallClimbedSinceGrounded = false;
    bool wallclimbing = false;

    // WallRun variables
    public bool canWallRun = false;


    // Initialisation
    void Start () {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (!firstPersonCamera) firstPersonCamera = Camera.main;
        firstPersonCameraTransform = firstPersonCamera.transform;
        firstPersonCameraTransform.localPosition = Vector3.up * cameraHeight;
        firstPersonCamera.fieldOfView = fieldOfView;

        myCollider = GetComponent<CapsuleCollider>();
        myCollider.radius = colliderRadius;
        myCollider.height = colliderHeight;
        myCollider.center = Vector3.up * colliderHeight / 2;

        myTransform = transform;
        myRigidbody = GetComponent<Rigidbody>();

        if (smoothMouse) {
            mouseSmoothingX = new float[mouseSmoothingFrames];
            mouseSmoothingY = new float[mouseSmoothingFrames];
        }
	}

    void FixedUpdate () {
        CheckGrounded();
        CheckSprinting();
        MouseInput();
        CheckWallClimb();
        Movement();
        CameraUpdate();
        CheckJump();
	}

    void Update () {
        mouseInputX += Input.GetAxis("Mouse X");
        mouseInputY += Input.GetAxis("Mouse Y");
        jumpInput = jumpInput || Input.GetButtonDown("Jump");
    }

    // Mouse input and smoothing
    void MouseInput () {
        if (smoothMouse) {
            for (int i = mouseSmoothingFrames - 1; i > 0; --i) {
                mouseSmoothingX[i] = mouseSmoothingX[i - 1];
                mouseSmoothingY[i] = mouseSmoothingY[i - 1];
            }
            mouseSmoothingX[0] = mouseInputX * mouseSensitivityX;
            mouseSmoothingY[0] = mouseInputY * mouseSensitivityY;

            foreach (float input in mouseSmoothingX)
                mouseX += input;
            mouseX /= mouseSmoothingFrames;

            foreach (float input in mouseSmoothingY)
                mouseY += input;
            mouseY /= mouseSmoothingFrames;
        }

        else {
            mouseX = mouseInputX * mouseSensitivityX;
            mouseY = mouseInputY * mouseSensitivityY;
        }

        mouseX *= (1 / Time.fixedDeltaTime) / 60;
        mouseY *= (1 / Time.fixedDeltaTime) / 60;

        mouseInputX = 0;
        mouseInputY = 0;
    }

    // Camera rotation and FOV handling
    void CameraUpdate () {

        if (invertMouseY)
            mouseY = -mouseY;
        myTransform.Rotate(myTransform.up * mouseX);
        mouseYRotation = Mathf.Clamp(mouseYRotation + mouseY, cameraMinAngle, cameraMaxAngle);
        firstPersonCameraTransform.localRotation = Quaternion.Euler((Vector3.left * mouseYRotation) + (Vector3.forward * mouseX * cameraTilt) + (Vector3.forward * -movementInput.x * 6 * cameraTilt));

        curFOVDelta = Mathf.Lerp(curFOVDelta, fieldOfViewDelta, Time.fixedDeltaTime * FOVShiftSpeed);

        firstPersonCamera.fieldOfView = Mathf.Lerp((fieldOfView + curFOVDelta) * fieldOfViewVerticalMultiplier, fieldOfView + curFOVDelta, Vector3.Dot(myTransform.forward, firstPersonCameraTransform.forward));
    }

    // Basic lateral movement
    void Movement () {

        movementInput.x = Input.GetAxis("Horizontal");
        movementInput.y = Input.GetAxis("Vertical");

        if (movementInput.sqrMagnitude > 1f) {
            movementInput = movementInput.normalized;
        }

        myRigidbody.velocity += myTransform.forward * forwardsSpeed * movementInput.y
            * ((1 / Time.fixedDeltaTime) / 60) * (sprinting ? sprintSpeedMultiplier : 1f);

        myRigidbody.velocity += myTransform.right * forwardsSpeed * movementInput.x
            * sidewaysSpeedMultiplier * ((1 / Time.fixedDeltaTime) / 60) * (sprinting ? sprintSpeedMultiplier * sidewaysSpeedMultiplier : 1f);

        if (noYAxisDrag) {
            myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, myRigidbody.velocity.y / (1f - Time.fixedDeltaTime * myRigidbody.drag), myRigidbody.velocity.z);
        }
    }

    // Check if the player is on the ground
    void CheckGrounded () {
        grounded = Physics.Raycast(myTransform.position + Vector3.up * 0.03f, Vector3.down, 0.06f, ~((1 << 2) + (1 << 8)));
    }

    // Sprinting
    void CheckSprinting () {
        sprinting = (Input.GetButton("Sprint") && canSprint && (movementInput.y > 0) && !wallclimbing) && (sprinting || grounded);
        fieldOfViewDelta = (sprinting ? sprintFOVKick : 0f);
    }

    // Jumping
    void CheckJump () {
        if (jumpInput && canJump && grounded) {
            myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, 0f, myRigidbody.velocity.z);
            if(sprinting) {
                myRigidbody.AddForce(Vector3.up * jumpForce * myRigidbody.mass * -Physics.gravity.y * jumpSprintVerticalForceMultiplier);
                myRigidbody.AddForce(myTransform.forward * jumpForce * myRigidbody.mass * myRigidbody.drag * jumpSprintForce);
            } else {
                myRigidbody.AddForce(Vector3.up * jumpForce * myRigidbody.mass * -Physics.gravity.y);
                myRigidbody.AddForce(myTransform.forward * jumpForce * myRigidbody.mass * myRigidbody.drag * jumpForwardForce * movementInput.y);
            }
        }
        jumpInput = false;
    }

    // Wall climbing
    void CheckWallClimb () {

        wallClimbedSinceGrounded = wallClimbedSinceGrounded && !grounded;

        if (!grounded && ((myRigidbody.velocity.y >= 0) || (myRigidbody.velocity.y >= minimumYVelocity && !wallClimbedSinceGrounded)) && Vector3.Dot(myRigidbody.velocity, myTransform.forward) >= 0) {
            if (Physics.Raycast((myTransform.position + Vector3.up * wallClimbFeetHeight) + myTransform.forward * (colliderRadius - 0.02f), myTransform.forward, 0.02f + wallClimbMaxDistance, ~((1 << 2) + (1 << 8)))) {
                if (!Physics.Raycast((myTransform.position + Vector3.up * wallClimbShoulderHeight) + myTransform.forward * (colliderRadius - 0.02f), myTransform.forward, 0.02f + wallClimbMaxDistance, ~((1 << 2) + (1 << 8)))) {
                    myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, wallClimbSpeed, myRigidbody.velocity.z);
                    myRigidbody.velocity += myTransform.forward * forwardsSpeed * ((1 / Time.fixedDeltaTime) / 60);
                    mouseX *= wallClimbMouseXMultiplier;
                    mouseY *= wallClimbMouseYMultiplier;
                    wallclimbing = true;
                    wallClimbedSinceGrounded = true;
                }

            } else if (wallclimbing) {
                wallclimbing = false;
                myRigidbody.AddForce(Vector3.up * wallClimbFinishJumpForce * myRigidbody.mass * -Physics.gravity.y);
            }
        }
    }
}