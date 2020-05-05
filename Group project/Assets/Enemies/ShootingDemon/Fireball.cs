using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 30;
    public int damage = 50;
    public Rigidbody2D rb;

    public GameObject demon;
    public ShootingDemon sd;


    // Start is called before the first frame update
    void Start()
    {
        this.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void Update()
    {
            rb.velocity = new Vector2(speed, 0);
        
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.Hurt(damage);
            }
        }
        Destroy(gameObject);
    }

    

}

