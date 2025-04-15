using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class UIcontroller : MonoBehaviourPunCallbacks
{
    [SerializeField] TextMeshProUGUI deadText;
    [SerializeField] Image progressBar;
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] TextMeshProUGUI coins;
    [SerializeField] AudioSource hitAudioSource;
    [SerializeField] AudioSource winAudioSource;

    [SerializeField] Image weaponImg;
    [SerializeField] Sprite weapon1;
    [SerializeField] Sprite weapon2;
    [SerializeField] Sprite weapon3;
    [SerializeField] Sprite weapon4;

    static int deadCounter = 0;
    float totalTime;
    static bool initialized = false;

    private PhotonView photonView;
    [SerializeField] TextMeshProUGUI actorNum;
    [SerializeField] GameObject finalCanvas;
    [SerializeField] TextMeshProUGUI finalText;


    void Start()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "IsAlive", true } });
        initialized = false;
        gameManager.gameManagerInstance.SaveProgress();
        gameManager.gameManagerInstance.LoadProgress();
    }


    public void initializeUIValues()
    {
        gameManager.gameManagerInstance.LoadProgress();
        changeImg();
        progressBar.fillAmount = 1f;
        deadText.text = deadCounter.ToString();
        timer.text = "00:00";
        coins.text = gameManager.gameManagerInstance.coins.ToString();
        actorNum.text = "Player " + PhotonNetwork.LocalPlayer.ActorNumber.ToString();
        initialized = true;
    }

    void Update()
    {
        if (gameManager.gameManagerInstance.gameType.Equals("one") && !initialized)
        {
            initializeUIValues();
            gameManager.gameManagerInstance.canStart = 1;
            gameManager.gameManagerInstance.SaveProgress();
        }
        if (gameManager.gameManagerInstance.gameType.Equals("multi"))
        {
            photonView = GetComponent<PhotonView>();
            if (photonView == null)
            {
                photonView = gameObject.AddComponent<PhotonView>();
            }
        }

        if (initialized)
        {
            totalTime += Time.deltaTime;
            gameManager.gameManagerInstance.time = totalTime;
            gameManager.gameManagerInstance.SaveProgress();

            int minutes = Mathf.FloorToInt(totalTime / 60f);
            int seconds = Mathf.FloorToInt(totalTime % 60f);
            timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

    }

    void changeImg()
    {
        gameManager.gameManagerInstance.LoadProgress();
        if (weaponImg != null)
        {
            if (gameManager.gameManagerInstance.weaponType == 0)
            {
                weaponImg.sprite = weapon1;
            }
            else if (gameManager.gameManagerInstance.weaponType == 1)
            {
                weaponImg.sprite = weapon2;
            }
            else if (gameManager.gameManagerInstance.weaponType == 2)
            {
                weaponImg.sprite = weapon3;
            }
            else
            {
                weaponImg.sprite = weapon4;
            }
        }
    }

    public void increaseDeadCounter()
    {
        deadCounter++;
        deadText.text = deadCounter.ToString();
        gameManager.gameManagerInstance.deadCounter = deadCounter;
        gameManager.gameManagerInstance.SaveProgress();
    }

    [PunRPC]
    void TakeDamage(float amount)
    {
        modifyLife(-amount);
    }

    bool hasDied = false;

    public void modifyLife(float amount)
    {
        if (amount < 0)
        {
            hitAudioSource.Play();
        }

        progressBar.fillAmount = Mathf.Clamp(progressBar.fillAmount + amount, 0f, 1f);
        Canvas.ForceUpdateCanvases();

        if (progressBar.fillAmount <= 0 && !hasDied)
        {
            hasDied = true;

            if (photonView.IsMine)
            {
                photonView.RPC("OnPlayerEliminated", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
            }
        }
    }

    [PunRPC]
    public void OnPlayerEliminated(int actorNumber)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            finalText.text = "Player " + actorNumber + " has been eliminated";
            photonView.RPC("RedirectAllPlayers", RpcTarget.All);
        }
    }

    [PunRPC]
    public void RedirectAllPlayers()
    {
        finalCanvas.SetActive(true);
        StartCoroutine(waitBeforeEnd());
    }

    IEnumerator waitBeforeEnd()
    {
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene("mainScene");
    }

    public void updateCoins(int numCoins)
    {
        gameManager.gameManagerInstance.coins += numCoins;
        gameManager.gameManagerInstance.SaveProgress();
        coins.text = gameManager.gameManagerInstance.coins.ToString();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void playWinSound()
    {
        winAudioSource.Play();
    }
}
