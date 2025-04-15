using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class multiplayerMouseLook : MonoBehaviourPun
{
    public Transform playerBody;
    float mouseSensitivity = 100f;
    float xRotation = 0f;

    bool loadValue = false;

    void Start()
    {
        loadValue = false;
        if (photonView.IsMine)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void Update()
    {
        // Processes input for the local player
        if (!photonView.IsMine) return;
        if (gameManager.gameManagerInstance.canStart == 1)
        {
            if (!loadValue)
            {
                gameManager.gameManagerInstance.LoadProgress();
                mouseSensitivity = gameManager.gameManagerInstance.mouseSensitivity;
                loadValue = true;
            }
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -70f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
