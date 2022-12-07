using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    public bool rightStickTurn;
    public bool isKeyboard;
    private Animator animator;
    [SerializeField] private bool isGrounded;
    AttackScript attackScript;
    public int health;
    [SerializeField] private PlayerInput playerInput;

    //Input
    private Vector2 movementInput;
    private bool jumpButton_state_toggled = false;

    public void OnMove(InputAction.CallbackContext ctx) => movementInput = ctx.ReadValue<Vector2>();
    public void OnJump(InputAction.CallbackContext ctx) => jumpButton_state_toggled = ctx.action.WasPressedThisFrame();


    void Start()
    {
        attackScript = GetComponentInChildren<AttackScript>();
        animator = GetComponentInChildren<Animator>();
        moveSpeed = 1.0f;
        health = 3;
    }
    void FixedUpdate()
    {
        //Timers, rotation beroende på riktning samt animationer
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

        if (transform.position.y < -20.0f)
        {
            Respawn();
        }

    }

    private void Update()
    {
        //Kan inte röra sig om man precis blivit träffad
        if (!attackScript.isHit)
        {
            Move();
        }
    }

    //Rörelsekontroller
    private void Move()
    {
        if (doubleJump <= 0)
        {
            jumpSpeed = 5.0f;
        }
        else if (doubleJump > 0)
        {
            jumpSpeed = 4.0f;
        }
        if (jumpButton_state_toggled && doubleJump <= 1)
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
        if (rb.velocity.x >= -2.0f && movementInput.x < 0.0f)
        {
            rb.velocity += new Vector3(movementInput.x * moveSpeed, 0.0f, 0.0f);
        }
        if (rb.velocity.x <= 2.0f && movementInput.x > 0.0f)
        {
            rb.velocity += new Vector3(movementInput.x * moveSpeed, 0.0f, 0.0f);
        }
        if (movementInput.y < -0.7 && !isGrounded && dropTimer <= 0)
        {
            rb.velocity += -5.0f * Vector3.up;
            dropTimer = 20;
        }
    }

    private void Respawn()
    {
        if (health > 1)
        {
            GetComponentInChildren<AttackScript>().health = 0.0f;
            transform.position = new Vector3(0, 10, 0);
            rb.velocity = Vector3.zero;
            health--;
        }
        else
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    //Gör så att spelaren kan hoppa igenom platformar underifrån samt landa på dom
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

        //Gör så spelaren kan hoppa ner genom en platform via knapptryck
        if (other.gameObject.CompareTag("Platform"))
        {
            if (movementInput.x < 0.3 && movementInput.x > -0.3 && movementInput.y < -0.7 && dropTimer <= 0)
            {
                Physics.IgnoreCollision(other.gameObject.GetComponent<BoxCollider>(), gameObject.GetComponent<CapsuleCollider>(), true);
                dropTimer = 60;
                rb.velocity += -1.0f * Vector3.up;
            }
        }
    }
}

