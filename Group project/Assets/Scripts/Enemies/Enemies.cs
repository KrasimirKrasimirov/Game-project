using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    public Animator animator;

    public int maxHealth = 100;
    int currentHealth;


    public float patrolSpeed = 2f;
    public float maxSpeed = 8f;
    public float triggerRange = 7.5f;
    public bool facingRight = true;
    private float speedMultiplier = 1f;
    bool movementAllowed = true;

    private enum State { Patrol = 0, Chase = 1, Dead = 2, Idle = 3 , Attack = 4, Hurt = 5};
    private State aiState = State.Patrol;


    public Transform attackPoint;
    public LayerMask playerLayer;

    public float attackRange = 2.5f;
    public int attackDamage = 20;

    public float attackRate = 0.5f;
    float nextAttackTime = 0f;



    Rigidbody2D rigidBodyComp;
    GameObject player;

    public GameObject groundCheck;
    float groundRadius = 0.2f;
    public bool isGrounded;
    public LayerMask whatIsGround;


    public GameObject patrolPoint1;
    public GameObject patrolPoint2;

    public GameObject nextPatrolPoint;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.Find("Player");
        facingRight = true;
        rigidBodyComp = GetComponent<Rigidbody2D>();
        isGrounded = false;
        nextPatrolPoint = patrolPoint2;
    }

    public void StopBots()
    {
        maxSpeed = 0f;
        patrolSpeed = 0f;
        triggerRange = 0f;
        movementAllowed = false;
        Rigidbody2D rigidBodyComp = this.GetComponent<Rigidbody2D>();
        rigidBodyComp.velocity = new Vector2(0f, rigidBodyComp.velocity.y);
        animator.SetFloat("Speed", 0f);
        aiState = State.Idle;
    }

    void FixedUpdate()
    {

        BaseMovementAndAnimation();

        switch (aiState)
        {
            case State.Chase:
                ChasePlayer();
                break;
            case State.Patrol:
                PerformPatrol();
                break;
            case State.Attack:
                AttackPlayer();
                break;
            case State.Dead:
                Die();
                break;
            case State.Idle:
                break;
            case State.Hurt:
               // Hurt();
                break;
            default:
                break;
        }
    }

    private void BaseMovementAndAnimation()
    {
        float horizontal = Input.GetAxis("Horizontal");


        if (movementAllowed)
        {
            PerformMovement(aiState == State.Chase ? maxSpeed : patrolSpeed);
        }

        if ((rigidBodyComp.velocity.x > 0 && !facingRight || rigidBodyComp.velocity.x < 0 && facingRight) && !this.animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            FlipTransform(horizontal);
        }


        if (currentHealth <= 0)
        {
            aiState = State.Dead;
        }
    }

    void PerformPatrol()
    {
        Vector3 offset = new Vector3(patrolSpeed, 0f);
       // bool grounded = Physics2D.OverlapCircle(groundCheck.transform.position + offset, groundRadius, whatIsGround);
        //if (!isGrounded)
        //{
       //     FlipMovement();
       // }


        if ((this.transform.position - player.transform.position).magnitude < triggerRange)
        {
            aiState = State.Chase;
        }
    }

    void ChasePlayer()
    {
        float horizontalDifference = this.transform.position.x - player.transform.position.x;
        if (((maxSpeed > 0f) && (horizontalDifference > 0f)) || ((maxSpeed < 0f) && (horizontalDifference < 0f)))
        {
            speedMultiplier = 3f;
            FlipMovement();
        }

        if((this.transform.position - player.transform.position).magnitude <= attackRange){
            Debug.Log("Can attack");
            aiState = State.Attack;
        }

        if ((this.transform.position - player.transform.position).magnitude > triggerRange * 1.5f)
        {
            aiState = State.Patrol;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
         {
             aiState = State.Attack;
         }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        float horizontal = Input.GetAxis("Horizontal");
       
        if (collision.gameObject == nextPatrolPoint)
        {
            FlipMovement();

            if ((rigidBodyComp.velocity.x > 0 && !facingRight || rigidBodyComp.velocity.x < 0 && facingRight) && !this.animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            {
                FlipTransform(horizontal);
            }


            if (nextPatrolPoint == patrolPoint1)
            {
                nextPatrolPoint = patrolPoint2;
            }
            else if (nextPatrolPoint == patrolPoint2)
            {
                nextPatrolPoint = patrolPoint1;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "Ground")
        {
            isGrounded = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        float horizontal = Input.GetAxis("Horizontal");
        aiState = State.Chase;

        if (collision.collider.tag == "Ground")
        {
            isGrounded = false;
        }
    }

    void AttackPlayer()
    {
        if (Time.time >= nextAttackTime)
        {
            if (!this.animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            {
                //play the attack animation
                animator.SetTrigger("Attack");
                rigidBodyComp.velocity = Vector2.zero;
                //detect enemies in range of attack
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);


                //damage enemy
                foreach (Collider2D enemy in hitEnemies)
                {
                    enemy.GetComponent<Player>().Hurt(attackDamage);
                    if(enemy.GetComponent<Player>().transform.position.x < transform.position.x)
                    {
                        enemy.GetComponent<Player>().knockFromRight = true;
                    }
                    else
                    {
                        enemy.GetComponent<Player>().knockFromRight = false;
                    }
                    
                }
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void FlipMovement()
    {
        patrolSpeed *= -1;
        maxSpeed *= -1;
    }

    void FlipTransform(float horizontal)
    {
            speedMultiplier = 2f;

            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
    }

    void PerformMovement(float speed)
    {
        float absSpeed = Mathf.Abs(speed);

        float newXVelocity = rigidBodyComp.velocity.x + speed * speedMultiplier * Time.fixedDeltaTime;
        rigidBodyComp.velocity = new Vector2(Mathf.Clamp(newXVelocity, -absSpeed, absSpeed), rigidBodyComp.velocity.y);
        animator.SetFloat("Speed", speed / maxSpeed);
    }

    



    public void Hurt(int damage)
    {
        currentHealth -= damage;


        animator.SetBool("Hurt", true);


        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        animator.SetBool("Dead", true);

        
        GetComponent<Collider2D>().enabled = false;

        gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        rigidBodyComp.velocity = new Vector2(0.0f,0.0f);

        this.enabled = false;
    }
}
