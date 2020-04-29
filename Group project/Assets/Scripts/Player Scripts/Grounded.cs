using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grounded : MonoBehaviour
{
    GameObject player;
    GameObject transition;
    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.transform.parent.gameObject;
        transition = GameObject.Find("Canvas/TransitionScreen");
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

        if (collision.gameObject.tag == "Transition")
        {
           

            switch(collision.collider.name)
            {
                default:
                    break;
                case "Transition 1":
                    player.GetComponent<Player>().transform.position = new Vector2(200, -25);
                    break;
                case "Transition 2":
                    player.GetComponent<Player>().transform.position = new Vector2(340, -29.3f);
                    break;
                case "Transition 3":
                    player.GetComponent<Player>().transform.position = new Vector2(397.5f, 20.7f);
                    break;
                case "Transition 4":
                    player.GetComponent<Player>().transform.position = new Vector2(455.1f, 32.8f);
                    break;
                case "Transition 5":
                    player.GetComponent<Player>().transform.position = new Vector2(448.8f, -4.3f);
                    break;
                case "Transition 6":
                    player.GetComponent<Player>().transform.position = new Vector2(500.4f, 38.5f);
                    break;
                case "Transition 7":
                    player.GetComponent<Player>().transform.position = new Vector2(493.32f, 8.24f);
                    break;
                case "Transition 8":
                    player.GetComponent<Player>().transform.position = new Vector2(650.61f, 21.16f);
                    break;
            }
            StartCoroutine(FinishTranisiton());
        }
    }

    IEnumerator FinishTranisiton()
    {
        
        transition.GetComponent<Image>().enabled = true;
        float alpha = transition.GetComponent<Image>().color.a;

       


        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 1.0f)
        {
            Color newColor = new Color(0, 0, 0, Mathf.Lerp(alpha, 0.0f, t));
            transition.GetComponent<Image>().color = newColor;
            yield return null;
        }

        transition.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
        transition.GetComponent<Image>().enabled = false;

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
