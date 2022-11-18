using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    public float health;
    private float jumpSpeed;
    private float moveSpeed;
    [SerializeField] private int doubleJump;
    [SerializeField] private int jumpTimer;
    [SerializeField] Rigidbody rb;
    [SerializeField] private KeyCode right;
    [SerializeField] private KeyCode left;
    [SerializeField] private KeyCode slam;
    [SerializeField] private KeyCode jump;
    [SerializeField] private KeyCode lightAttack;
    [SerializeField] private KeyCode heavyAttack;
    [SerializeField] private KeyCode specialAttack;
    private float attackPower;
    [SerializeField] private int dropTimer;
    [SerializeField] private Animator animator;
    [SerializeField] private bool isGrounded;
    private int punchTimer;

    private void Start()
    {
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
        if (rb.velocity.x > 0.2)
        {
            rb.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
        }
        else if (rb.velocity.x < -0.2)
        {
            rb.rotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
        }

        if (punchTimer > 0)
        {
            punchTimer--;
        }

        if (punchTimer <= 0)
        {
            animator.SetBool("isPunching", false);
        }

        if (isGrounded && rb.velocity.x > 0.05 || isGrounded && rb.velocity.x < -0.05)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
        
    }

    private void Update()
    {
        Move();
    }
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

        if (Input.GetKeyDown(jump) && doubleJump <= 1)
        {
            if (jumpTimer <= 0)
            {
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

        if (Attack() && punchTimer <= 0)
        {
            animator.SetBool("isPunching", true);
            punchTimer = 25;
        }

    }

    private bool Attack()
    {
        bool didAttack = false;
        if (Input.GetKeyDown(lightAttack))
        {
            attackPower = 1.0f;
            didAttack = true;
        }
        else if (Input.GetKeyDown(heavyAttack))
        {
            attackPower = 1.5f;
            didAttack = true;
        }
        else if (Input.GetKeyDown(specialAttack))
        {
            attackPower = 2.0f;
            didAttack = true;
        }

        return didAttack;
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
        if (other.gameObject.CompareTag("Player"))
        {
            if (Attack() && punchTimer <= 0)
            {
                Vector3 dir = other.transform.position - transform.position;
                if (dir.y < 0.2 && dir.y >= 0)
                {
                    dir = new Vector3(dir.x, dir.y + (0.2f - dir.y), dir.z);
                }
                else if(dir.y > -0.2 && dir.y < 0)
                {
                    dir = new Vector3(dir.x, dir.y - (0.2f - dir.y), dir.z);
                }
                other.GetComponent<Rigidbody>().velocity += (other.gameObject.GetComponent<CharacterScript>().health / 100 + 1) * attackPower * dir.normalized;
                float randIncrease = Random.Range(1, 10);
                other.gameObject.GetComponent<CharacterScript>().health += attackPower / randIncrease;
            }
        }

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

