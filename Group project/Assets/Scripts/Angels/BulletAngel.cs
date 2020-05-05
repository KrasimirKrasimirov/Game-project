using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletAngel : MonoBehaviour
{
   

    bool _canGiveBlessing = true;
    GameObject player;

    GameObject _InteractionText;

    private void Start()
    {
        player = GameObject.Find("Player");
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
                player.GetComponent<Musket>().maxAmmo += 12;
                Musket.currentAmmo = player.GetComponent<Musket>().maxAmmo;
                Musket.loadedAmmo = 1;
                _InteractionText.GetComponent<Text>().text = "There you go friend, take these as a gift from us.. (ammunitions clip increased by 12 and all ammo regained)";
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
                player.GetComponent<Musket>().maxAmmo += 12;
                Musket.currentAmmo = player.GetComponent<Musket>().maxAmmo;
                Musket.loadedAmmo = 1;
                _InteractionText.GetComponent<Text>().text = "There you go friend, take these as a gift from us.. (ammunitions clip increased by 12 and all ammo regained)";
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
