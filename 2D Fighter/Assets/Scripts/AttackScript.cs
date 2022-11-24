using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    [SerializeField] private KeyCode lightAttack;
    [SerializeField] private KeyCode heavyAttack;
    [SerializeField] private KeyCode block;
    [SerializeField] GameObject male;
    [SerializeField] GameObject female;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator maleAnimator;
    [SerializeField] private Animator femaleAnimator;
    private Rigidbody rb;
    public float health;
    private float attackPower;
    private bool secondAttack;
    private int punchTimer;
    private int blockTimer;
    public bool isHit;
    private bool gotHit;
    [SerializeField] private int hitTimer;
    [SerializeField] private int heavyTimer;
    private bool isHeavyAttack;
    private BoxCollider boxHitbox;
    [SerializeField] List<Collider> colliders;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        boxHitbox = GetComponent<BoxCollider>();
        for (int i = 0; i < colliders.Count; i++)
        {
            Physics.IgnoreCollision(boxHitbox, colliders[i], true);
        }
        if (male.activeSelf)
        {
            animator = male.GetComponent<Animator>();
        }
        else if (female.activeSelf)
        {
            animator = female.GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (punchTimer > 0)
        {
            punchTimer--;
        }

        if (blockTimer > 0)
        {
            blockTimer--;
        }

        if (heavyTimer > 0)
        {
            heavyTimer--;
        }

        if (heavyTimer > 15)
        {
            animator.SetFloat("heavySpeed", 0.2f);
        }
        else
        {
            animator.SetFloat("heavySpeed", 0.8f);
        }

        if (rb.velocity.magnitude >= 5)
        {
            isHit = true;
            if (hitTimer <= 0)
            {
                hitTimer = 20;
            }
        }

        if (gotHit)
        {
            hitTimer = 20;
            gotHit = false;
        }

        if (hitTimer <= 0)
        {
            isHit = false;
        }
        else
        {
            isHit = true;
            hitTimer--;
        }
    }

    private void Update()
    {
        if (!isHit)
        {
            Move();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Hitbox"))
        {
            if (punchTimer <= 0)
            {
                if (Attack())
                {
                    if (!isHeavyAttack)
                    {
                        Damage(other);
                    }
                }
            }
            if (heavyTimer <= 12 && heavyTimer >= 8)
            {
                print("HIT");
                Damage(other);
            }
        }
    }

    private void Move()
    {
        if (punchTimer <= 0)
        {
            if (Attack())
            {
                if (secondAttack)
                {
                    secondAttack = false;
                    animator.SetBool("secondAttack", false);
                }
                else
                {
                    secondAttack = true;
                    animator.SetBool("secondAttack", true);
                }
                if (isHeavyAttack)
                {
                    punchTimer = 40;
                    heavyTimer = 40;
                }
                else
                {
                    punchTimer = 25;
                }
            }
        }
        if (Block() && blockTimer <= 0)
        {
            blockTimer = 20;
        }
    }

    private bool Attack()
    {
        bool didAttack = false;
        if (Input.GetKeyDown(lightAttack))
        {
            isHeavyAttack = false;
            attackPower = 1.2f;
            animator.SetTrigger("lightAttack");
            didAttack = true;
        }
        else if (Input.GetKeyDown(heavyAttack))
        {
            isHeavyAttack = true;
            attackPower = 1.4f;
            animator.SetTrigger("heavyAttack");
            didAttack = true;
        }

        return didAttack;
    }

    private bool Block()
    {
        bool temp = false;
        if (Input.GetKeyDown(block))
        {
            animator.SetTrigger("block");
            temp = true;
        }

        return temp;
    }

    private void Damage(Collider other)
    {
        other.GetComponent<AttackScript>().gotHit = true;
        Vector3 dir = other.transform.position - transform.position;
        if (dir.y < 0.5 && dir.y >= 0)
        {
            dir = new Vector3(dir.x, dir.y + (0.5f - dir.y), dir.z);
        }
        else if (dir.y > -0.5 && dir.y < 0)
        {
            dir = new Vector3(dir.x, dir.y - (0.5f - dir.y), dir.z);
        }
        other.GetComponentInParent<Rigidbody>().velocity = new Vector3(1.0f, 1.0f, 1.0f);
        other.GetComponentInParent<Rigidbody>().velocity += ((other.gameObject.GetComponent<AttackScript>().health / 100) + 1) * attackPower * dir.normalized;
        float randIncrease = Random.Range(1, 10);
        other.gameObject.GetComponent<AttackScript>().health += attackPower / randIncrease;
    }
}
