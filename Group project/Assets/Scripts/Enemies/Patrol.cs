﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public float jumpSpeed = 5f;
    public bool isGrounded = false;

    public float returnToX;
    public float returnToY;

    int damage = 20;

    void Start()
    {
    }

    void Update()
    {
        if (isGrounded)
        {
            //gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpSpeed), ForceMode2D.Impulse);


            Vector3 movement = new Vector3(returnToX, returnToY, 0f);
            transform.position = movement;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            isGrounded = true;
           
        }

        if(collision.collider.tag == "Player")
        {
            Vector3 movement = new Vector3(returnToX, returnToY, 0f);
            transform.position = movement;

            
            collision.gameObject.GetComponent<Player>().Hurt(damage);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            isGrounded = false ;
           
        }
    }
}
