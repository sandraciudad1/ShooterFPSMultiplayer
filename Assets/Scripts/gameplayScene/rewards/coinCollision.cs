using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class coinCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            UIcontroller ui = GameObject.Find("UIController").GetComponent<UIcontroller>();
            if (ui != null)
            {
                ui.playWinSound();
                ui.updateCoins(5);
                Destroy(gameObject);
            }
        }
    }
}
