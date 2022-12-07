using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    //private Rigidbody rb;
    public float health;
    private float attackPower;
    private bool secondAttack;
    private int punchTimer;
    private int blockTimer;
    public bool isBlock;
    public bool isHit;
    private bool gotHit;
    public bool hitAnim;
    private bool onlyOneHitAnim;
    [SerializeField] private int attackTimer;
    [SerializeField] private int hitTimer;
    [SerializeField] private int heavyTimer;
    private bool lightTest;
    private bool isHeavyAttack;
    //[SerializeField] List<Collider> colliders;

    //Inputs
    private bool lightAttack_state_toggled = false;
    private bool heavyAttack_state_toggled = false;
    private bool block_state_toggled = false;

    public void OnLightAttack(InputAction.CallbackContext ctx) => lightAttack_state_toggled = ctx.action.WasPressedThisFrame();
    public void OnHeavyAttack(InputAction.CallbackContext ctx) => heavyAttack_state_toggled = ctx.action.WasPressedThisFrame();
    public void OnBlock(InputAction.CallbackContext ctx) => block_state_toggled = ctx.action.WasPressedThisFrame();


    // Start is called before the first frame update
    void Start()
    {
        //for (int i = 0; i < colliders.Count; i++)
        //{
        //    Physics.IgnoreCollision(boxHitbox, colliders[i], true);
        //}
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

        if (attackTimer > 0)
        {
            attackTimer--;
        }

        if (hitTimer > 0)
        {
            hitTimer--;
        }

        if (punchTimer <= 0)
        {
            lightTest = false;
        }

        if (hitTimer <= 0)
        {
            isHit = false;
        }
        else
        {
            isHit = true;
        }

        if (gotHit)
        {
            hitTimer = 20;
            heavyTimer = 0;
            gotHit = false;
        }

        if (heavyTimer > 15)
        {
            animator.SetFloat("heavySpeed", 0.2f);
        }
        else
        {
            animator.SetFloat("heavySpeed", 0.8f);
        }

        if (hitAnim)
        {
            animator.SetTrigger("gotHit");
            hitAnim = false;
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
            if (attackTimer > 0)
            {
                if (!isHeavyAttack)
                {
                    Damage(other);
                    attackTimer = 0;
                }
            }
            if (heavyTimer <= 12 && heavyTimer >= 8)
            {
                Damage(other);
            }
        }
    }

    private void Move()
    {
        
        if (punchTimer <= 0 && !lightTest)
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
                if (isHeavyAttack && heavyTimer <= 0)
                {
                    punchTimer = 60;
                    heavyTimer = 40;
                }
                else
                {
                    punchTimer = 30;
                }
                lightTest = true;
                attackTimer = 15;
            }
        }
        if (blockTimer >= 30)
        {
            isBlock = true;
        }
        else
        {
            isBlock = false;
            if (Block())
            {
                animator.SetTrigger("block");
                blockTimer = 50;
            }
        }
        //if (blockTimer <= 0)
        //{
        //    if (Block())
        //    {
        //        animator.SetTrigger("block");
        //        blockTimer = 50;
        //    }
        //}

       
    }

    public IEnumerator BlockCR(Collider other)
    {
        onlyOneHitAnim = true;
        other.GetComponentInParent<AttackScript>().hitAnim = true;
        yield return new WaitForSeconds(0.1f);
        onlyOneHitAnim = false;
    }

    private bool Attack()
    {
        bool didAttack = false;

        if (lightAttack_state_toggled)
        {
            isHeavyAttack = false;
            attackPower = 4.5f;
            animator.SetTrigger("lightAttack");
            didAttack = true;
        }
        else if (heavyAttack_state_toggled)
        {
            isHeavyAttack = true;
            attackPower = 1.5f;
            animator.SetTrigger("heavyAttack");
            didAttack = true;
        }
        return didAttack;
    }

    private bool Block()
    {
        bool temp = false;
        if (block_state_toggled)
        {
            temp = true;
        }
        return temp;
    }

    private void Damage(Collider other)
    {
        if (!onlyOneHitAnim)
        {
            if (other != null)
            {
                StartCoroutine(BlockCR(other));
            }
        }
        if (!other.GetComponentInParent<AttackScript>().isBlock)
        {
            Vector3 dir = other.transform.position - transform.position;
            if (dir.y < 1.0f && dir.y >= 0)
            {
                dir = new Vector3(dir.x, dir.y + (1.0f - dir.y), dir.z);
            }
            else if (dir.y > -1.0f && dir.y < 0)
            {
                dir = new Vector3(dir.x, dir.y - (1.0f - dir.y), dir.z);
            }
            //other.GetComponentInParent<Rigidbody>().velocity = new Vector3(other.GetComponentInParent<Rigidbody>().velocity.x / 10, other.GetComponentInParent<Rigidbody>().velocity.y / 10, other.GetComponentInParent<Rigidbody>().velocity.z / 10);
            other.GetComponentInParent<Rigidbody>().velocity += ((other.gameObject.GetComponentInParent<AttackScript>().health / 100) + 1) * attackPower * dir.normalized;
            float randIncrease = Random.Range(1, 10);
            other.gameObject.GetComponentInParent<AttackScript>().health += attackPower / randIncrease * 5;
            other.GetComponentInParent<AttackScript>().gotHit = true;

        }
    }
}
