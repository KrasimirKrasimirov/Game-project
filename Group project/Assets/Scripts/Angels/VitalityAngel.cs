using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VitalityAngel : MonoBehaviour
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

                Player.maxHealth += 25;
                Player.currentHealth = PlayerController.maxHealth;


                _InteractionText.GetComponent<Text>().text = "Here, take my blessing! (Max health increased by 25 and fully healed)";
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

                Player.maxHealth += 25;
                Player.currentHealth = PlayerController.maxHealth;


                _InteractionText.GetComponent<Text>().text = "Here, take my blessing! (Max health increased by 25 and fully healed)";
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
