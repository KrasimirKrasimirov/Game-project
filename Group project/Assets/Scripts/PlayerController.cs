using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpSpeed = 5f;
    public bool isGrounded = false;
    public static int lives = 3;
    public Button restartButton;

    void Start() {
        lives = 3;
        isGrounded = false;
        Time.timeScale = 1.0f;
    }

    void Update()
    {
        Jump();
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.position += movement * Time.deltaTime * moveSpeed;

        if (lives == 0)
        {
            Die();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.Rotate(0f, 180f, 0f);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpSpeed), ForceMode2D.Impulse);
        }
    }

    void Die()
    {
        restartButton.gameObject.SetActive(true);
        Time.timeScale = 0.0f;
    }

    
  

}
