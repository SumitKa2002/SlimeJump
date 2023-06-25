using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CoinCollector : MonoBehaviour
{
    private static int totalCoins = 0;
    [SerializeField] private TextMeshProUGUI coinText;

    private void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            totalCoins = 0;
            PlayerPrefs.SetInt("TotalCoins", totalCoins);
            UpdateCoinText();
        }
        else
        {
            totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
            UpdateCoinText();
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coins"))
        {
            Destroy(collision.gameObject);
            totalCoins++;
            PlayerPrefs.SetInt("TotalCoins", totalCoins);
            UpdateCoinText();
        }
    }

    private void UpdateCoinText()
    {
        if(coinText != null)
        {
            int playerPrefsValue = PlayerPrefs.GetInt("TotalCoins", 0);
            coinText.text = playerPrefsValue.ToString("D3");
        }
    }

}
