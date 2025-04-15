using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class networkManager : MonoBehaviourPunCallbacks
{
    public static networkManager networkManagerInstance { get; private set; }
    [SerializeField] GameObject playerPrefab;

    private void Awake()
    {
        if (networkManagerInstance == null)
        {
            networkManagerInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinOrCreateRoom("multiplayerRoom", null, null);
    }

    public override void OnJoinedRoom()
    {
        gameManager.gameManagerInstance.playersNum = PhotonNetwork.CurrentRoom.PlayerCount;
        gameManager.gameManagerInstance.SaveProgress();

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("GameStarted") &&
            (bool)PhotonNetwork.CurrentRoom.CustomProperties["GameStarted"])
        {
            waitingZoneUI waitingUI = GameObject.Find("networkManager").GetComponent<waitingZoneUI>();
            if (waitingUI != null)
            {
                waitingUI.StartGameLogic(); 
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"Jugador {newPlayer.ActorNumber} entró con nickname: {newPlayer.NickName}");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"Jugador {otherPlayer.ActorNumber} salió.");
    }
}