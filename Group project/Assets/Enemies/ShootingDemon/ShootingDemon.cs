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



    GameObject player;

    public float _reloadSpeed = 3.2f;
    bool _canFire = true;
    bool _isReloading = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.Find("Player");
        facingRight = true;
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
        }



        if (player.transform.position.x < transform.position.x)
        {
            facingRight = false;

            if (_canFire == true)
            {
            AttackPlayer();
            }
        }
        else if (player.transform.position.x > transform.position.x)
        {
            facingRight = true;

            if (_canFire == true)
            {
                AttackPlayer();
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
        Debug.Log("hit");

       // animator.SetBool("Hurt", true);


        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        //animator.SetBool("Dead", true);


        GetComponent<BoxCollider2D>().enabled = false;
        weakSpot.GetComponent<BoxCollider2D>().enabled = false;

        this.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<Player>().isSliding)
            {
               // this.rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
    }
}
