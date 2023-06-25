using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
    [HideInInspector] public bool isCollected = false;
    [SerializeField] private Image keyCanvas;

    private void Awake()
    {
        keyCanvas.gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isCollected = true;
            keyCanvas.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
