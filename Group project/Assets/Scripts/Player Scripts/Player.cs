using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D myRigidbody;

    public Animator myAnimator;
    public bool isGrounded;
    public static bool isHurt;

    [SerializeField]
    public float movementSpeed;
    public float jumpSpeed;
    Image healthBar;

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
        jumpSpeed = 22.0f;
        isGrounded = false;
        facingRight = true;
        invincible = false;
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        
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



        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpSpeed), ForceMode2D.Impulse);
            
            
        }

        myAnimator.SetFloat("AirSpeed", Mathf.Abs(vertical));

        
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");

        //movement start
        HandleMovement(horizontal);

        Flip(horizontal);
        //movement end


    }

   
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////Player movement start
    private void HandleMovement(float horizontal)
    { 
        if(!this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
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
        if(horizontal > 0 && !facingRight || horizontal < 0 && facingRight && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
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



}
