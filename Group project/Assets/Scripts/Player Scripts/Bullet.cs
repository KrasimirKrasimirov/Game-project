using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 30;
    public int damage = 50;
    public Rigidbody2D rb;

    public Player player;


    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();

        if(player.transform.localScale.x < 0)
        {
            speed = -speed;
        }

        
    }

    private void Update()
    {
        rb.velocity = new Vector2(speed, 0);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.name == "HeavyBandit")
        {
            Enemies enemy = hitInfo.GetComponent<Enemies>();
            if (enemy != null)
            {
                enemy.Hurt(damage);
            }
        }

        if (hitInfo.name == "WeakSpot")
        {
            ShootingDemon sd = hitInfo.GetComponent<ShootingDemon>();
            if (sd != null)
            {
                sd.Hurt(damage);
            }
        }





        Destroy(gameObject);
        
    }

 

}
