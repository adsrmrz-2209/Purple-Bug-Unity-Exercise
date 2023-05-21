using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    Animator anim;
    [HideInInspector]public CharacterController controller;
    PlayerHealth playerHealth;
    float defmoveSpeed;
    float moveSpeed = 4f;
    float jumpForce = 0.8f; //default is 0.7f
    float gravity = -18f; //default is -9.81f

    Vector3 velocity;
    Vector3 movement;
    RaycastHit hitDown;
    RaycastHit hitUp;
    float hitUpLength = 0.01f;

    [SerializeField] Transform hitUpPos;

    Vector3 LastPos;

    [SerializeField] Joystick joystickLeft;
    [SerializeField] Joystick joystickRight;

    [SerializeField] AudioSource jumpSound;
    [SerializeField] AudioSource breakableSound;
    [SerializeField] AudioSource slimeSound;



    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        playerHealth = GetComponent<PlayerHealth>();
        defmoveSpeed = moveSpeed;
        //joystick = GetComponent<Joystick>();

    }

    void Update()
    {
        controller.stepOffset = isGrounded() ? 0.29f : 0.00f;

        Movement();

        if (Physics.Raycast(transform.position, Vector3.down, out hitDown, 0.4f))
        {
            EnemyScript enemyScript = hitDown.collider.GetComponent<EnemyScript>();
            if (hitDown.collider.isTrigger && hitDown.collider.gameObject.CompareTag("enemy"))
            {
                velocity.y = -0.5f;
                Jump();
                enemyScript?.Die();
                slimeSound.Play();
            }
        }

        if (Physics.Raycast(hitUpPos.position, Vector3.up, out hitUp, hitUpLength))
        {
            BreakableBlocks breakableBlocks = hitUp.collider.GetComponent<BreakableBlocks>();
            if (hitUp.collider.CompareTag("breakable"))
            {
                velocity.y = -0.5f;
                breakableBlocks?.TakeDamage();
                breakableSound.Play();
            }
        }
    }

    private bool isGrounded()
    {
        if (controller.isGrounded)
        {
            Vector3 currentPos = transform.position;
            LastPos = currentPos;
            moveSpeed = defmoveSpeed;
            return true;
        }
        else
        {
            moveSpeed = 2.4f;
            return false;
        }
    }

    private void Movement()
    {
        bool spaceBar = Input.GetKey(KeyCode.Space);
        //movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, 0.0f).normalized; //for keyboard
        movement = new Vector3(joystickLeft.Horizontal, 0.0f, 0.0f).normalized; //for mobile
        if (movement.x != 0) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.6f); //the higher the value the faster the rotation (0.01 - 1.00)
        
        anim.SetBool("isRunning", (movement.x != 0 && isGrounded()));
        if (isGrounded())
        {
            velocity.y = -0.5f;
            if (joystickRight.Vertical >= 0.5f || joystickRight.Vertical <= -0.5f)
            {
                Jump();
            }
        }

        anim.SetBool("jumpTrigger", !isGrounded());
        velocity.y += gravity * Time.deltaTime;
        if(controller.enabled)
        {
            controller.Move(movement * Time.deltaTime * moveSpeed);
            controller.Move(velocity * Time.deltaTime);
        }      
    }

    public void Jump()
    {
        jumpSound.Play();
        velocity.y += Mathf.Sqrt(jumpForce * -5.0f * gravity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(hitUpPos.position, Vector3.up * hitUpLength);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * 0.4f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("killZone"))
        {
            StartCoroutine(ResetPos());
            playerHealth.numOflives--;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("pole")) FindObjectOfType<UIScript>().NextLevel();
    }


    IEnumerator ResetPos()
    {
        controller.enabled = false;
        transform.position = new Vector3(LastPos.x, LastPos.y, 0);
        yield return new WaitForSeconds(0.05f);
        controller.enabled = true;
    }

}
