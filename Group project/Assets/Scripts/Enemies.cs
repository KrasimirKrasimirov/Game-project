using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    public Animator animator;

    public int maxHealth = 100;
    int currentHealth;

    [SerializeField]
    GameObject coin;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

  
    public void TakeDamage(int damage)
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

        //create coin(s) on death


        int amountOfCoins = Random.Range(1, 4);
        for(; amountOfCoins > 0; amountOfCoins--)
        {
            Instantiate(coin, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.Euler(0, 0, Random.Range(1, 360)));
        }

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}
