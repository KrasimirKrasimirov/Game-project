using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static int currentCoins;
    public static int maxCoins;
    [SerializeField]
    Text coinText;

    // Start is called before the first frame update
    void Start()
    {
        maxCoins = 20;
    }

    // Update is called once per frame
    void Update()
    {
        coinText.text = "Coins : " + currentCoins.ToString();
    }
}
