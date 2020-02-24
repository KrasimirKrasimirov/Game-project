using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.AddForce(new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)), ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (CoinManager.currentCoins != CoinManager.maxCoins)
            {
                CoinManager.currentCoins++;
                //play coin pickup sound
                //play coin pickup animation
                Destroy(gameObject);
            }
        }
    }

}
