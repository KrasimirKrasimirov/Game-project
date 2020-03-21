using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public Slider MasterVolSlider;
    public Text MasterVolText;
    public float currentMasterVolume;
    // Start is called before the first frame update
    void Start()
    {
        ////Master Volume
        currentMasterVolume = PlayerPrefs.GetFloat("MasterVolume");
        MasterVolText.text = (PlayerPrefs.GetFloat("MasterVolume") * 100).ToString();
        MasterVolSlider.value = PlayerPrefs.GetFloat("MasterVolume");
    }

    public void MasterVol()
    {
        currentMasterVolume = MasterVolSlider.value;
        AudioListener.volume = currentMasterVolume;
        MasterVolText.text = (currentMasterVolume * 100).ToString();
        PlayerPrefs.SetFloat("MasterVolume", currentMasterVolume);
    }
}
