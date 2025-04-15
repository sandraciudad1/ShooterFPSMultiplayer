using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class bulletRechargeCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            UIcontroller ui = GameObject.Find("UIController").GetComponent<UIcontroller>();
            if (ui != null)
            {
                ui.playWinSound();
            }
            raycast raycast = GameObject.Find("Player").GetComponent<raycast>();
            if (raycast != null)
            {
                raycast.bulletCounter = raycast.checkBulletCounter();
                raycast.updateBullets(raycast.bulletCounter);
                Destroy(gameObject);
            }
        }
    }

}
