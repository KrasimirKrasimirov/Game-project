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
         
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        player.GetComponent<Player>().isGrounded = true;
        player.GetComponent<Enemies>().isGrounded = true;
        //Player.GetComponent<Player>().myAnimator.SetBool("Grounded", true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            player.GetComponent<Player>().isGrounded = false;
            player.GetComponent<Player>().myAnimator.SetBool("Grounded", false);
        }
    }
}
