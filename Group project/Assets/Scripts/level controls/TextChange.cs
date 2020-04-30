using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextChange : MonoBehaviour
{
    public Text reserveAmmoText;

    // Update is called once per frame
    void Update()
    {
        reserveAmmoText.text = Musket.currentAmmo.ToString();
    }
}
