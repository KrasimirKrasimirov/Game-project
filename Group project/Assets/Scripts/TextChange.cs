using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextChange : MonoBehaviour
{
    public Text txt;


    // Use this for initialization
    void Start()
    {
       
        txt.text = "Lives : " + PlayerController.lives;
    }

    // Update is called once per frame
    void Update()
    {
        txt.text = "Lives : " + PlayerController.lives;
    }
}
