using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingDemon : MonoBehaviour
{
    public Rigidbody2D rb;

    public Transform firePoint;
    public GameObject fireballPrefab;
    public GameObject weakSpot;

    public Animator animator;

    public int maxHealth = 100;
    public int currentHealth;

    public bool facingRight = true;


    public Transform attackPoint;
    public LayerMask playerLayer;

    public int attackDamage = 20;

    public int collisionDamage = 10;

    bool died = false;



    GameObject player;

    public float _reloadSpeed = 3.2f;
    public bool _canFire = true;
    bool _isReloading = false;

    public bool playerNearby;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.Find("Player");
        facingRight = true;
        died = false;
        playerNearby = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_canFire)
        {
            animator.SetBool("canAttack", true);
        }
        else
        {
            animator.SetBool("canAttack", false);
        }


        if(!this.animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && !facingRight)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (!this.animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && facingRight)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }



        if (currentHealth <= 0)
        {
            Die();
            died = true;
        }


        if (playerNearby) { 
        if (player.transform.position.x < transform.position.x)
        {
            facingRight = false;

            if (_canFire == true && !died)
            {
            AttackPlayer();
            }
        }
        else if (player.transform.position.x > transform.position.x)
        {
            facingRight = true;

            if (_canFire == true && !died)
            {
                AttackPlayer();
            }
        }
        }

    }


    void AttackPlayer()
    {
            
            //play the attack animation
            // animator.SetTrigger("Attack");
            
        GameObject fb =  Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);

        if (facingRight)
        {
            fb.GetComponent<Fireball>().speed = fb.GetComponent<Fireball>().speed;
            fb.GetComponent<Fireball>().transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (!facingRight)
        {
            fb.GetComponent<Fireball>().speed = -fb.GetComponent<Fireball>().speed;
            fb.GetComponent<Fireball>().transform.localRotation = Quaternion.Euler(0, 180, 0);
        }

        StartCoroutine(WeaponReload());            
    }

    //waits an amount of seconds before allowing the weapon to be fired again
    IEnumerator WeaponReload()
    {
        _isReloading = true;
        _canFire = false;
        
        //play reload animation
        //play reload sounds
        yield return new WaitForSeconds(_reloadSpeed);


        _canFire = true;
        
        _isReloading = false;
    }

    

    public void Hurt(int damage)
    {
        currentHealth -= damage;

       // animator.SetBool("Hurt", true);


        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {

        //animator.SetBool("Dead", true);


        GetComponent<BoxCollider2D>().enabled = false;
        weakSpot.GetComponent<BoxCollider2D>().enabled = false;
        this.rb.constraints = RigidbodyConstraints2D.FreezeAll;

        this.enabled = false;
    }

   

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.name == "KeepSlidingCheck")
        {
            if (collision.gameObject.GetComponentInParent<Player>().isSliding)
            {
                StartCoroutine(playerSlided());
            }
        }

    }

    IEnumerator playerSlided()
    {
        
        this.enabled = false;
        yield return new WaitForSeconds(4);

        if (!died) { 
        this.enabled = true;
        }
    }







}
