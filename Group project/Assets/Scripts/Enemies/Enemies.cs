﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    public Animator animator;

    public int maxHealth = 100;
    public int currentHealth;


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

    public float attackRange = 4.5f;
    public int attackDamage = 20;

    public int collisionDamage = 10;

    public float attackRate = 0.5f;
    float nextAttackTime = 0f;



    Rigidbody2D rigidBodyComp;
    GameObject player;

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

                //AttackPlayer();
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
            if(aiState == State.Chase)
            {
                PerformMovement(maxSpeed);
            }
            else if(aiState != State.Chase)
            {
                PerformMovement(patrolSpeed);
            }
        }

        if ((rigidBodyComp.velocity.x > 0 && !facingRight || rigidBodyComp.velocity.x < 0 && facingRight) && !this.animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            FlipTransform(horizontal);
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Chase"))
        {
            aiState = State.Chase;
        }


        if (currentHealth <= 0)
        {
            aiState = State.Dead;
        }
    }

    void PerformPatrol()
    {
        
    }

    void ChasePlayer()
    {
        float horizontalDifference = this.transform.position.x - player.transform.position.x;
        if (((maxSpeed > 0f) && (horizontalDifference > 0f)) || ((maxSpeed < 0f) && (horizontalDifference < 0f)))
        {
            speedMultiplier = 3f;
        }

        if(horizontalDifference > 0f && facingRight)
        {
            FlipMovement();
        }
        else if(horizontalDifference < 0f && !facingRight)
        {
            FlipMovement();
        }

       
        if ((this.transform.position - player.transform.position).magnitude <= attackRange){
            aiState = State.Attack;
            StartCoroutine(AttackPlayer());
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Patrol"))
        {
            aiState = State.Patrol;
            FlipMovement();
        }
    }

  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        float horizontal = Input.GetAxis("Horizontal");
       
        if (collision.gameObject == nextPatrolPoint && animator.GetCurrentAnimatorStateInfo(0).IsTag("Patrol"))
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            collision.collider.GetComponent<Player>().Hurt(collisionDamage);
            aiState = State.Attack;
            rigidBodyComp.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        if (collision.collider.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "Ground")
        {
            isGrounded = true;
        }
        if (collision.collider.tag == "Player")
        {
            //rigidBodyComp.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        float horizontal = Input.GetAxis("Horizontal");

        if (collision.collider.tag == "Ground")
        {
            isGrounded = false;
        }

        if(collision.collider.tag == "Player")
        {
           // rigidBodyComp.constraints = RigidbodyConstraints2D.None;
            rigidBodyComp.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }



    IEnumerator AttackPlayer()
    {
        if (!this.animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            //play the attack animation
            //animator.SetTrigger("Attack");
            animator.SetBool("Attack", true);
            rigidBodyComp.velocity = Vector2.zero;
            //detect enemies in range of attack
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);


            //damage enemy
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Player>().Hurt(attackDamage);
                if (enemy.GetComponent<Player>().transform.position.x < transform.position.x)
                {
                    enemy.GetComponent<Player>().knockFromRight = true;
                }
                else
                {
                    enemy.GetComponent<Player>().knockFromRight = false;
                }
            }



            yield return new WaitForSeconds(0.5f);

            animator.SetBool("Attack", false);

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
        animator.SetBool("Dead", true);

        GetComponent<Collider2D>().enabled = false;

        gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        rigidBodyComp.velocity = Vector2.zero;

        this.enabled = false;
    }
}
