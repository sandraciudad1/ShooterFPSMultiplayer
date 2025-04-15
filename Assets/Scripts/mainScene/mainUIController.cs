using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class mainUIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI firstScore;
    [SerializeField] TextMeshProUGUI secondScore;
    [SerializeField] TextMeshProUGUI thirdScore;
    [SerializeField] TextMeshProUGUI fourthScore;
    [SerializeField] TextMeshProUGUI fifthScore;
    [SerializeField] TextMeshProUGUI score;

    int minutesPoints = 500;
    int secondsPoints = 5;
    int deadPoints = 150;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        gameManager.gameManagerInstance.LoadProgress();
        float time = gameManager.gameManagerInstance.time;
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        gameManager.gameManagerInstance.points = (minutes * minutesPoints) + (seconds * secondsPoints) + (gameManager.gameManagerInstance.deadCounter * deadPoints);
        gameManager.gameManagerInstance.SaveProgress();

        score.text = gameManager.gameManagerInstance.points.ToString();
        updateRanking(gameManager.gameManagerInstance.points);
    }


    void updateRanking(int points)
    {
        int first = gameManager.gameManagerInstance.first;
        int second = gameManager.gameManagerInstance.second;
        int third = gameManager.gameManagerInstance.third;
        int fourth = gameManager.gameManagerInstance.fourth;
        int fifth = gameManager.gameManagerInstance.fifth;

        if (points > first)
        {
            gameManager.gameManagerInstance.fifth = fourth;
            gameManager.gameManagerInstance.fourth = third;
            gameManager.gameManagerInstance.third = second;
            gameManager.gameManagerInstance.second = first;
            gameManager.gameManagerInstance.first = points;
        }
        else if (points > second && points != first)
        {
            gameManager.gameManagerInstance.fifth = fourth;
            gameManager.gameManagerInstance.fourth = third;
            gameManager.gameManagerInstance.third = second;
            gameManager.gameManagerInstance.second = points;
        }
        else if (points > third && points != second && points != first)
        {
            gameManager.gameManagerInstance.fifth = fourth;
            gameManager.gameManagerInstance.fourth = third;
            gameManager.gameManagerInstance.third = points;
        }
        else if (points > fourth && points != third && points != second && points != first)
        {
            gameManager.gameManagerInstance.fifth = fourth;
            gameManager.gameManagerInstance.fourth = points;
        }
        else if (points > fifth && points != fourth && points != third && points != second && points != first)
        {
            gameManager.gameManagerInstance.fifth = points;
        }

        firstScore.text = gameManager.gameManagerInstance.first.ToString();
        secondScore.text = gameManager.gameManagerInstance.second.ToString();
        thirdScore.text = gameManager.gameManagerInstance.third.ToString();
        fourthScore.text = gameManager.gameManagerInstance.fourth.ToString();
        fifthScore.text = gameManager.gameManagerInstance.fifth.ToString();

        gameManager.gameManagerInstance.SaveProgress();
    }

    public void playBtn()
    {
        gameManager.gameManagerInstance.gameType = "one";
        gameManager.gameManagerInstance.SaveProgress();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("gameplayScene");
    }

    public void multiplayerBtn()
    {
        gameManager.gameManagerInstance.gameType = "multi";
        gameManager.gameManagerInstance.SaveProgress();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("multiplayerGameplayScene");
    }

    public void shopBtn()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("shopScene");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
