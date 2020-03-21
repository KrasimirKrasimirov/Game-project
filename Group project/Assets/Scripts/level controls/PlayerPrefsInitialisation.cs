using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsInitialisation : MonoBehaviour
{
    public float currentMasterVolume;
    // Start is called before the first frame update
    void Start()
    {
        ////Master Volume
        currentMasterVolume = PlayerPrefs.GetFloat("MasterVolume");
    }
}
