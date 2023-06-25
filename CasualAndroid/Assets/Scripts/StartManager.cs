using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{

    [SerializeField] private GameObject coinCanvas;
    [SerializeField] private GameObject pauseButton;

    private void Start()
    {
        gameObject.SetActive(true);
        coinCanvas.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(false);
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        gameObject.SetActive(false);
        coinCanvas.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(true);
        Time.timeScale = 1f;
    }

}
