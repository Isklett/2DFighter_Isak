using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterScript : MonoBehaviour
{
    private float jumpSpeed;
    private float moveSpeed;
    [SerializeField] private int doubleJump;
    [SerializeField] private int jumpTimer;
    [SerializeField] Rigidbody rb;
    [SerializeField] private KeyCode right;
    [SerializeField] private KeyCode left;
    [SerializeField] private KeyCode slam;
    [SerializeField] private KeyCode jump;
    [SerializeField] private int dropTimer;
    [SerializeField] private bool isKeyboard;
    private Animator animator;
    [SerializeField] private bool isGrounded;
    AttackScript attackScript;

    void Start()
    {
        attackScript = GetComponentInChildren<AttackScript>();
        animator = GetComponentInChildren<Animator>();
        moveSpeed = 1.0f;
    }
    void FixedUpdate()
    {
        if (dropTimer > 0)
        {
            dropTimer--;
        }
        if (jumpTimer > 0)
        {
            jumpTimer--;
        }
        if (rb.velocity.x > 0.2 && !attackScript.isHit)
        {
            rb.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
        }
        else if (rb.velocity.x < -0.2 && !attackScript.isHit)
        {
            rb.rotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
        }

        if (isGrounded && rb.velocity.x > 0.01f || isGrounded && rb.velocity.x < -0.01f)
        {
            animator.SetBool("isWalking", true);
            if (rb.velocity.x > 0.5f || rb.velocity.x < -0.5f)
            {
                animator.SetBool("isRunning", true);
            }
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }

        animator.SetBool("isGrounded", isGrounded);

        //if (!attackScript.isHit)
        //{
        //    Move();
        //}

    }

    private void Update()
    {
        if (!attackScript.isHit)
        {
            Move();
        }
    }
    private void Move()
    {
        var gamepad = Gamepad.current;

        if (doubleJump <= 0)
        {
            jumpSpeed = 5.0f;
        }
        else if (doubleJump > 0)
        {
            jumpSpeed = 4.0f;
        }
        
        if (isKeyboard)
        {
            if (Input.GetKeyDown(jump) && doubleJump <= 1)
            {
                if (jumpTimer <= 0)
                {
                    animator.SetTrigger("jump");
                    rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
                    rb.velocity += jumpSpeed * Vector3.up;
                    doubleJump++;
                    jumpTimer = 5;
                }
            }
            if (Input.GetKey(left))
            {
                if (rb.velocity.x >= -2.0f)
                {
                    rb.velocity += moveSpeed * Vector3.left;
                }
            }
            if (Input.GetKeyDown(slam) && !isGrounded)
            {
                rb.velocity += -10.0f * Vector3.up;
            }
            if (Input.GetKey(right))
            {
                if (rb.velocity.x <= 2.0f)
                {
                    rb.velocity += moveSpeed * Vector3.right;
                }
            }
        }

        if (!isKeyboard)
        {
            if (gamepad.aButton.wasPressedThisFrame && doubleJump <= 1)
            {
                if (jumpTimer <= 0)
                {
                    animator.SetTrigger("jump");
                    rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
                    rb.velocity += jumpSpeed * Vector3.up;
                    doubleJump++;
                    jumpTimer = 5;
                }
            }
            if (rb.velocity.x >= -2.0f && gamepad.leftStick.ReadValue().x < 0.0f)
            {
                rb.velocity += new Vector3(gamepad.leftStick.ReadValue().x * moveSpeed, 0.0f, 0.0f);
            }
            if (rb.velocity.x <= 2.0f && gamepad.leftStick.ReadValue().x > 0.0f)
            {
                rb.velocity += new Vector3(gamepad.leftStick.ReadValue().x * moveSpeed, 0.0f, 0.0f);
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Platform") && transform.position.y < other.transform.position.y)
        {
            Physics.IgnoreCollision(other.gameObject.GetComponent<BoxCollider>(), gameObject.GetComponent<CapsuleCollider>(), true);
        }
        else if (other.gameObject.CompareTag("Platform") && transform.position.y > other.transform.position.y && dropTimer == 0)
        {
            Physics.IgnoreCollision(other.gameObject.GetComponent<BoxCollider>(), gameObject.GetComponent<CapsuleCollider>(), false);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Platform"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Ground") && rb.velocity.y <= 0.5 && rb.velocity.y >= -0.5 && jumpTimer <= 0)
        {
            doubleJump = 0;
            isGrounded = true;
        }
        if (other.gameObject.CompareTag("Platform") && rb.velocity.y <= 0.5 && rb.velocity.y >= -0.5 && jumpTimer <= 0)
        {
            doubleJump = 0;
            isGrounded = true;
        }

        if (other.gameObject.CompareTag("Platform"))
        {
            if (Input.GetKeyDown(slam))
            {
                Physics.IgnoreCollision(other.gameObject.GetComponent<BoxCollider>(), gameObject.GetComponent<CapsuleCollider>(), true);
                dropTimer = 60;
                rb.velocity += -1.0f * Vector3.up;
            }
        }
    }
}

