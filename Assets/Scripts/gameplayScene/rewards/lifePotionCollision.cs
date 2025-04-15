using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lifePotionCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            UIcontroller ui = GameObject.Find("UIController").GetComponent<UIcontroller>();
            if (ui != null)
            {
                ui.playWinSound();
                ui.modifyLife(0.5f);
                Destroy(gameObject);
            }
        }
    }
}
