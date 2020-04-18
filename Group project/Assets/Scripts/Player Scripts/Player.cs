using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Rigidbody2D myRigidbody;

    public Animator myAnimator;
    public bool isGrounded;
    public static bool isHurt;

    float furtherJumpIfRunning;

    //bool dashAttack;
    public bool isDashAttacking;
    float dashAttackTimer = 0f;
    float maxDashAttackTime = 0.5f;
    float dashAttackSpeed = 15f;

    public bool isJumping;
    bool jumpKeyHeld;
    Vector2 counterJumpForce;

    [SerializeField]
    public float movementSpeed;
    public float jumpSpeed;
    Image healthBar;
    public GameObject groundCheck;

    public static float currentHealth;
    public static float maxHealth = 100;
    public Button restartButton;


    public Transform attackPoint;
    public LayerMask enemyLayers;

    public float attackRange = 0.5f;
    public int attackDamage = 40;

    public float attackRate = 2f;
    float nextAttackTime = 0f;

    private bool facingRight;

    public bool invincible;
    private float invincibilityTime = 3f;

    //check if we are sliding
    public bool isSliding = false;
    float slideTimer = 0f; //store the slide time
    public float maxSlideTime = 1.5f; //set the maximum time to slide
    public float slideSpeed = 10f;

    public bool keepSliding = false;



    public float knockback; //amount of force applied when the player gets knocked back
    public float knockbackLength; //how long the player gets knocked back for in terms of time
    public float knockbackCount; //counts down the knockbacck
    public bool knockFromRight;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        Time.timeScale = 1.0f;
        movementSpeed = 10.0f;
        jumpSpeed = 25.0f;
        isGrounded = false;
        facingRight = true;
        invincible = false;
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        counterJumpForce = new Vector2(0f, -30f);

        
    }

    public Animator getAnimator()
    {
        return myAnimator;
    }

    void Update()
    {
        
        //healthBar.fillAmount = currentHealth / 100;
        //Debug.Log(currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }


        float vertical = Input.GetAxis("Vertical");

        if(Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }



        if(myRigidbody.velocity.x != 0)
        {
            furtherJumpIfRunning = 1.05f;
        }
        else
        {
            furtherJumpIfRunning = 1f;
        }



        if(Input.GetKeyDown(KeyCode.F) && !isDashAttacking && isGrounded)
        {
            DashAttack();
        }

        if (isDashAttacking)
        {
            dashAttackTimer += Time.deltaTime;

            if (dashAttackTimer > maxDashAttackTime)
            {
                isDashAttacking = false;

               // myAnimator.SetBool("isDashAttacking", false);

                myRigidbody.velocity = Vector2.zero;
            }




            //detect enemies in range of attack
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            //damage enemy
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Enemies>().Hurt(attackDamage);

                isDashAttacking = false;
            }
        }

       
           
        
        






        //if (Input.GetKeyDown(KeyCode.W) && isGrounded && !isSliding)
        if (Input.GetKeyDown(KeyCode.W))
        {
            jumpKeyHeld = true;
            if(isGrounded && !isSliding)
            {
                isJumping = true;
                myRigidbody.AddForce(Vector2.up * jumpSpeed * myRigidbody.mass * furtherJumpIfRunning, ForceMode2D.Impulse);
            }
            //gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpSpeed), ForceMode2D.Impulse);

        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            jumpKeyHeld = false;
        }

        myAnimator.SetFloat("AirSpeed", Mathf.Abs(vertical));






        if(Input.GetButtonDown("Slide") && !isSliding && isGrounded)
        {
            slideTimer = 0f;
            myAnimator.SetBool("isSliding", true);
            //gameObject.transform.localScale = new Vector3(5.0f, 5.0f, 1.0f);
            //myRigidbody.AddForce(new Vector2(2f, 0f), ForceMode2D.Impulse);

            if (facingRight) {
                myRigidbody.AddForce(new Vector2(slideSpeed, 0f), ForceMode2D.Impulse);
                myRigidbody.velocity = new Vector2(slideSpeed, myRigidbody.velocity.y);
            }
            else
            {
                myRigidbody.AddForce(new Vector2(-slideSpeed, 0f), ForceMode2D.Impulse);
                myRigidbody.velocity = new Vector2(-slideSpeed, myRigidbody.velocity.y);
            }

            gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            
            //groundCheck.GetComponent<BoxCollider2D>().enabled = false;

            isSliding = true;
        }

        if(isSliding)
        {
            slideTimer += Time.deltaTime;

            if(slideTimer > maxSlideTime && !keepSliding)
            {
                isSliding = false;

                myAnimator.SetBool("isSliding", false);
                //gameObject.transform.localScale = new Vector3(10.0f, 10.0f, 1.0f);
                gameObject.GetComponent<PolygonCollider2D>().enabled = true;
                myRigidbody.velocity = Vector2.zero;
                //groundCheck.GetComponent<BoxCollider2D>().enabled = true;

            }
        }
        
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");

        //movement start
        HandleMovement(horizontal);

        Flip(horizontal);
        //movement end


        if (isJumping)
        {
            if (!jumpKeyHeld && Vector2.Dot(myRigidbody.velocity, Vector2.up) > 0)
            {
                myRigidbody.AddForce(counterJumpForce * myRigidbody.mass);
            }
            Debug.Log(counterJumpForce);
        }

    }

   
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////Player movement start
    private void HandleMovement(float horizontal)
    {
        if(!this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && !isSliding && !isDashAttacking)
        {
            if(knockbackCount <= 0) { 
            myRigidbody.velocity = new Vector2(horizontal * movementSpeed, myRigidbody.velocity.y);
            }
            else
            {
                if (knockFromRight)
                {
                    myRigidbody.velocity = new Vector2(-knockback, knockback/4);
                }
                if (!knockFromRight)
                {
                    myRigidbody.velocity = new Vector2(knockback, knockback/4);
                }
                knockbackCount -= Time.deltaTime;
            }
        }



        myAnimator.SetFloat("speed", Mathf.Abs(horizontal));
        
    }

 
    private void Flip(float horizontal)
    {
        if(((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight)) && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && !isSliding && !isDashAttacking)
        {
            facingRight = !facingRight;

            Vector3 theScale = transform.localScale;

            theScale.x *= -1;

            transform.localScale = theScale;
        }
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////Player movement finish

    void Attack()
    {
        //play the attack animation
        myAnimator.SetTrigger("Attack");
        myRigidbody.velocity = Vector2.zero;
        //detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);



        //damage enemy
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemies>().Hurt(attackDamage);
        }
    }

    void DashAttack()
    {
        dashAttackTimer = 0f;
        //myAnimator.SetBool("isDashAttacking", true);

        if (facingRight)
        {
            myRigidbody.AddForce(new Vector2(dashAttackSpeed, 0f), ForceMode2D.Impulse);
            myRigidbody.velocity = new Vector2(dashAttackSpeed, myRigidbody.velocity.y);
        }
        else
        {
            myRigidbody.AddForce(new Vector2(-dashAttackSpeed, 0f), ForceMode2D.Impulse);
            myRigidbody.velocity = new Vector2(-dashAttackSpeed, myRigidbody.velocity.y);
        }

        isDashAttacking = true;

        
    }

    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    void Die()
    {
        myAnimator.SetTrigger("Death");
        this.enabled = false;
        restartButton.gameObject.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void Hurt(int damage)
    {
        if (!invincible)
        {
            Debug.Log("Hurt");
            currentHealth -= damage;

            knockbackCount = knockbackLength;
           
            StartCoroutine(Invulnerability());
        }

        

        myAnimator.SetBool("Hurt", true);


        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator Invulnerability()
    {
        invincible = true;
        yield return new WaitForSeconds(invincibilityTime);
        invincible = false;
    }


    


  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().tag == "Ground" || collision.GetComponent<Collider2D>().tag == "Enemy" || collision.name == "Slope")
        {
            keepSliding = true;
       }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().tag == "Ground" || collision.GetComponent<Collider2D>().tag == "Enemy" || collision.name == "Slope")
        {
            keepSliding = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.GetComponent<Collider2D>().tag == "Ground" || collision.GetComponent<Collider2D>().tag == "Enemy") && collision.name != "Slope")
        {
            keepSliding = false;
        }
    }

}
