using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealingAngel : MonoBehaviour
{
    

    bool _canGiveBlessing = true;

    GameObject _InteractionText;

    private void Start()
    {
        _InteractionText = GameObject.Find("InteractionText");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && _canGiveBlessing == true)
        {
            _InteractionText.GetComponent<Text>().text = "Press E to recieve the angel's blessing";
            if (Input.GetButtonDown("Interact"))
            {
                _canGiveBlessing = false;
                Player.currentHealth = Player.maxHealth;
                _InteractionText.GetComponent<Text>().text = "Let me help you little soldier! (Health regained)";
                //Play sound
                //Play animation/particle effects
            }
        }
    }


    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && _canGiveBlessing == true)
        {
            _InteractionText.GetComponent<Text>().text = "Press E to recieve the angel's blessing";
            if (Input.GetButtonDown("Interact"))
            {
                _canGiveBlessing = false;
                Player.currentHealth = Player.maxHealth;
                _InteractionText.GetComponent<Text>().text = "Let me help you little soldier! (Health regained)";
                //Play sound
                //Play animation/particle effects
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        _InteractionText.GetComponent<Text>().text = "";   
    }
}
