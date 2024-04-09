using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [SerializeField]
    float playerSpeed = 2.0f,
        sprintScalar = 1,
        sneakScalar = 0.5f,
        rotationSpeed = 3,
        jumpHeight = 1.0f,
        pushPower = 2.0f;
    [SerializeField] Transform view;
    [SerializeField] float sensitivity = 10;
    [SerializeField, Range(-180, 180)] 
    float cameraDownMax = -45, 
        cameraUpMax = 45;
    private float pitch = 40, yaw = 0;
    [SerializeField] Animator characterAnimator;

    private void Start()
    {
       controller = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        characterAnimator.ResetTrigger("Jumping");
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            characterAnimator.SetBool("Falling", false);
        }

        characterAnimator.SetFloat("Speed", Input.GetAxis("Vertical") + (Mathf.Abs(Input.GetAxis("Horizontal") * 2)));
        if (Input.GetKey(KeyCode.LeftControl))
        {
            characterAnimator.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Vertical") + (Input.GetAxis("Horizontal") * 2)));
            characterAnimator.SetBool("Crouching", true);
        }
        else
        {
            characterAnimator.SetFloat("Speed", Input.GetAxis("Vertical") + (Mathf.Abs(Input.GetAxis("Horizontal") * 2)));
            characterAnimator.SetBool("Crouching", false);
        }

        Vector3 move = new(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        move = Vector3.ClampMagnitude(move, 1);
        move = Quaternion.Euler(0, view.rotation.eulerAngles.y, 0) * move;

        if (!Input.GetKey(KeyCode.LeftControl))
        {
            controller.Move((Input.GetKey(KeyCode.LeftShift) ? sprintScalar : 1) * playerSpeed * Time.deltaTime * move);
        }
        else
        {
            controller.Move(sneakScalar * playerSpeed * Time.deltaTime * move);
        }

        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;

        pitch = Mathf.Clamp(pitch, cameraDownMax, cameraUpMax);

        Quaternion qYaw = Quaternion.AngleAxis(yaw, Vector3.up);
        Quaternion qPitch = Quaternion.AngleAxis(pitch, Vector3.right);
        Quaternion qRotation = qYaw * qPitch;

        view.transform.rotation = qRotation;
        transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * sensitivity);

        // Changes the height position of the player..
        if (Input.GetButton("Jump") && groundedPlayer && !Input.GetKey(KeyCode.LeftControl))
        {
            characterAnimator.SetTrigger("Jumping");
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * Physics.gravity.y);
        }
        if (playerVelocity.y > 0 && !characterAnimator.GetBool("Jumping"))
        {
            characterAnimator.SetBool("Falling", true);
        }
        playerVelocity.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    // this script pushes all rigidbodies that the character touches
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
        {
            return;
        }

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        body.velocity = pushDir * pushPower;
    }


    private void OnTriggerEnter(Collider other)
    {
        // Starts push animation if pushable
        if (other.gameObject.CompareTag("Pushable"))
        {
            characterAnimator.SetBool("Pushing", true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Starts push animation if pushable
        if (other.gameObject.CompareTag("Pushable"))
        {
            characterAnimator.SetBool("Pushing", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Stops push animation if pushable
        if (other.gameObject.CompareTag("Pushable"))
        {
            characterAnimator.SetBool("Pushing", false);
        }
    }
}
