using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public static gameManager gameManagerInstance { get; private set; }

    public int points;
    public int first;
    public int second;
    public int third;
    public int fourth;
    public int fifth;

    public float time;
    public int deadCounter;
    public int coins;
    public int weaponType;
    public int ownedWeapons;

    public int canStart;
    public int playersNum;
    public string gameType;

    public string nickname;
    public float mouseSensitivity = 100f;
    public float playerSpeed = 5f;

    public int playerSelected;

    private void Awake()
    {
        if (gameManagerInstance == null)
        {
            gameManagerInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadProgress();
    }

    public void SaveProgress()
    {
        PlayerPrefs.SetInt("points", points);
        PlayerPrefs.SetInt("first", first);
        PlayerPrefs.SetInt("second", second);
        PlayerPrefs.SetInt("third", third);
        PlayerPrefs.SetInt("fourth", fourth);
        PlayerPrefs.SetInt("fifth", fifth);

        PlayerPrefs.SetFloat("time", time);
        PlayerPrefs.SetInt("deadCounter", deadCounter);
        PlayerPrefs.SetInt("coins", coins);
        PlayerPrefs.SetInt("weaponType", weaponType);
        PlayerPrefs.SetInt("ownedWeapons", ownedWeapons);

        PlayerPrefs.SetInt("canStart", canStart);
        PlayerPrefs.SetInt("playersNum", playersNum);
        PlayerPrefs.SetString("gameType", gameType);

        PlayerPrefs.SetString("nickname", nickname);
        PlayerPrefs.SetFloat("mouseSensitivity", mouseSensitivity);
        PlayerPrefs.SetFloat("playerSpeed", playerSpeed);

        PlayerPrefs.SetInt("playerSelected", playerSelected);

        PlayerPrefs.Save();
    }

    public void LoadProgress()
    {
        points = PlayerPrefs.GetInt("points", 0);
        first = PlayerPrefs.GetInt("first", 0);
        second = PlayerPrefs.GetInt("second", 0);
        third = PlayerPrefs.GetInt("third", 0);
        fourth = PlayerPrefs.GetInt("fourth", 0);
        fifth = PlayerPrefs.GetInt("fifth", 0);

        time = PlayerPrefs.GetFloat("time", 0);
        deadCounter = PlayerPrefs.GetInt("deadCounter", 0);
        coins = PlayerPrefs.GetInt("coins", 150);
        weaponType = PlayerPrefs.GetInt("weaponType", 0);
        ownedWeapons = PlayerPrefs.GetInt("ownedWeapons", 0);

        canStart = PlayerPrefs.GetInt("canStart", 0);
        playersNum = PlayerPrefs.GetInt("playersNum", 0);
        gameType = PlayerPrefs.GetString("gameType", "");
        nickname = PlayerPrefs.GetString("nickname", "");
        mouseSensitivity = PlayerPrefs.GetFloat("mouseSensitivity", 100f);
        playerSpeed = PlayerPrefs.GetFloat("playerSpeed", 5f);

        playerSelected = PlayerPrefs.GetInt("playerSelected", 0);
    }
}

