using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class playerSetUp : MonoBehaviourPun
{
    void Start()
    {
        if (photonView.IsMine)
        {
            gameObject.name = $"Player_{PhotonNetwork.LocalPlayer.ActorNumber}";
        }
        else
        {
            gameObject.name = $"Player_{photonView.Owner.ActorNumber}";
        }
    }
}
