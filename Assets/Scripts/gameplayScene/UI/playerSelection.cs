using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class playerSelection : MonoBehaviour
{
    [SerializeField] GameObject playerSelectionCanvas;
    [SerializeField] GameObject undefinedBorder;
    [SerializeField] GameObject womenBorder;
    [SerializeField] GameObject menBorder;

    [SerializeField] GameObject nicknameCanvas;
    [SerializeField] GameObject waitingZoneCanvas;

    [SerializeField] GameObject undefinedPlayerPrefab;
    [SerializeField] GameObject womanPlayerPrefab;
    [SerializeField] GameObject manPlayerPrefab;

    public void playerSelectionBtn()
    {
        playerSelectionCanvas.SetActive(true);
    }

    public void undefinedBtn()
    {
        SelectPlayer(0);
    }

    public void womenBtn()
    {
        SelectPlayer(1);
    }

    public void menBtn()
    {
        SelectPlayer(2);
    }

    public void SelectPlayer(int playerType)
    {
        gameManager.gameManagerInstance.playerSelected = playerType;
        gameManager.gameManagerInstance.SaveProgress();
        undefinedBorder.SetActive(playerType == 0);
        womenBorder.SetActive(playerType == 1);
        menBorder.SetActive(playerType == 2);
        StartCoroutine(wait1sec());

    }

    IEnumerator wait1sec()
    {
        yield return new WaitForSeconds(1f);
        createPlayer();
        playerSelectionCanvas.SetActive(false);
    }

    void createPlayer()
    {
        GameObject playerPrefab;
        int playerSelected = gameManager.gameManagerInstance.playerSelected;
        if (playerSelected == 0)
        {
            playerPrefab = undefinedPlayerPrefab;
        }
        else if (playerSelected == 1)
        {
            playerPrefab = womanPlayerPrefab;
        }
        else if (playerSelected == 2)
        {
            playerPrefab = manPlayerPrefab;
        }
        else
        {
            playerPrefab = undefinedPlayerPrefab;
        }

        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, randomPosition(), Quaternion.identity);
        waitingZoneCanvas.SetActive(true);

        postProcess postProcess = GameObject.Find("postProcessManager").GetComponent<postProcess>();
        if (postProcess != null)
        {
            postProcess.initializeValuesPostProcess();
        }
    }

    Vector3 randomPosition()
    {
        int randomLocation = Random.Range(0, 2);
        float randomX, randomZ;
        if (randomLocation == 0)
        {
            randomX = Random.Range(0f, -7f);
            randomZ = Random.Range(38f, 60f);
        } else
        {
            randomX = Random.Range(0f, 9f);
            randomZ = Random.Range(46f, 60f);
        }
        return new Vector3(randomX, 5.3f, randomZ);
    }
}