using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.Rendering.PostProcessing;

public class waitingZoneUI : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject waitingCamera;
    [SerializeField] GameObject waitingCanvas;
    [SerializeField] TextMeshProUGUI playersNum;
    bool activeCamera = false, activeCanvas = false;

    void Start()
    {
        playersNum.text = "0";    
    }

    void Update()
    {
        gameManager.gameManagerInstance.LoadProgress();
        playersNum.text = gameManager.gameManagerInstance.playersNum.ToString();
    }

    public void startGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // Set the game as started
            ExitGames.Client.Photon.Hashtable gameProperties = new ExitGames.Client.Photon.Hashtable();
            gameProperties.Add("GameStarted", true);
            PhotonNetwork.CurrentRoom.SetCustomProperties(gameProperties);
        }
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("GameStarted") && (bool)propertiesThatChanged["GameStarted"])
        {
            StartGameLogic(); 
        }
    }

    public void StartGameLogic()
    {
        activeCameraAndCanvas(); 
        waitingCamera.SetActive(false); 
        waitingCanvas.SetActive(false); 

        gameManager.gameManagerInstance.canStart = 1; 
        gameManager.gameManagerInstance.SaveProgress(); 
    }

    void activeCameraAndCanvas()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            PhotonView playerPhotonView = player.GetComponent<PhotonView>();

            if (playerPhotonView != null && playerPhotonView.IsMine)
            {
                // Enables the local player's camera and canvas
                Camera playerCamera = player.GetComponentInChildren<Camera>(true);
                if (playerCamera != null)
                {
                    playerCamera.gameObject.SetActive(true);
                    PostProcessVolume volume = playerCamera.GetComponent<PostProcessVolume>();

                    if (volume != null)
                    {
                        postProcessManager.postProcessInstance.ApplyPostProcessSettings(volume);
                    }
                    activeCamera = true;

                }

                Canvas playerCanvas = player.GetComponentInChildren<Canvas>(true);
                if (playerCanvas != null)
                {
                    playerCanvas.gameObject.SetActive(true);
                    activeCanvas = true;

                    InitializePlayerUI(player);
                }
            }
            else
            {
                // Confirm that non-local player cameras and canvas are disabled
                Camera otherPlayerCamera = player.GetComponentInChildren<Camera>(true);
                if (otherPlayerCamera != null)
                {
                    otherPlayerCamera.gameObject.SetActive(false);
                }

                Canvas otherPlayerCanvas = player.GetComponentInChildren<Canvas>(true);
                if (otherPlayerCanvas != null)
                {
                    otherPlayerCanvas.gameObject.SetActive(false);
                }
            }
        }
    }

    void InitializePlayerUI(GameObject player)
    {
        UIcontroller uiController = player.GetComponentInChildren<UIcontroller>();
        if (uiController != null)
        {
            uiController.initializeUIValues();
        }

        raycast raycast = player.GetComponentInChildren<raycast>();
        if (raycast != null)
        {
            raycast.initializeRaycastValues();
        }
    }
}
