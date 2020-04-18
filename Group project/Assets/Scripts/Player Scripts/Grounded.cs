using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounded : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.transform.parent.gameObject;
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground")
        {
            player.GetComponent<Player>().isGrounded = true;
            player.GetComponent<Player>().myAnimator.SetBool("Grounded", true);
            player.GetComponent<Player>().isJumping = false;


            if (collision.collider.name == "Slope")
            {
                Debug.Log(collision.collider.name);
                player.GetComponent<Player>().myRigidbody.velocity = new Vector2(2f, -2f);
                player.GetComponent<Player>().isSliding = true;
                player.GetComponent<Player>().keepSliding = true;
            }
        }

        if (collision.gameObject.tag == "Enemy")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
            Debug.Log("hello");
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        player.GetComponent<Player>().isGrounded = true;
        player.GetComponent<Enemies>().isGrounded = true;
        //Player.GetComponent<Player>().myAnimator.SetBool("Grounded", true);

        if (collision.collider.name == "Slope")
        {
            Debug.Log(collision.collider.name);
            player.GetComponent<Player>().myRigidbody.velocity = new Vector2(2f, -2f);
            player.GetComponent<Player>().isSliding = true;
            player.GetComponent<Player>().keepSliding = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            player.GetComponent<Player>().isGrounded = false;
            player.GetComponent<Player>().myAnimator.SetBool("Grounded", false);


            if (collision.collider.name == "Slope")
            {
                player.GetComponent<Player>().isSliding = false;
                player.GetComponent<Player>().keepSliding = false;
            }
        }

        
    }
}
