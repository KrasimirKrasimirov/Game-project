using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletAngel : MonoBehaviour
{
    [SerializeField]
    Text _InteractionText;

    bool _canGiveBlessing = true;
    GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && _canGiveBlessing == true)
        {
            _InteractionText.text = "Press E to recieve the angel's blessing";
            if (Input.GetButtonDown("Interact"))
            {
                _canGiveBlessing = false;
                player.GetComponent<Musket>().maxAmmo += 12;
                Musket.currentAmmo = player.GetComponent<Musket>().maxAmmo;
                Musket.loadedAmmo = 1;
                _InteractionText.text = "";
                //Play sound
                //Play animation/particle effects
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        _InteractionText.text = "";   
    }
}
