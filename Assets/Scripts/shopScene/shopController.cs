using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class shopController : MonoBehaviour
{
    [SerializeField] Image weaponType;
    [SerializeField] GameObject leftBtn;
    [SerializeField] GameObject rightBtn;
    [SerializeField] GameObject selectBtn;  
    [SerializeField] GameObject buyBtn;

    [SerializeField] Sprite weaponType1;
    [SerializeField] Sprite weaponType2;
    [SerializeField] Sprite weaponType3;
    [SerializeField] Sprite weaponType4;
    Sprite[] weaponTypes;
    static int currentIndex = 0;

    [SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] Image errorImg;

    void Start()
    {
        gameManager.gameManagerInstance.weaponType = 3;
        gameManager.gameManagerInstance.ownedWeapons = 3;
        gameManager.gameManagerInstance.SaveProgress();
        coinsText.text = gameManager.gameManagerInstance.coins.ToString();

        weaponTypes = new Sprite[] { weaponType1, weaponType2, weaponType3, weaponType4 };
        UpdateWeaponImage();
        UpdateArrowButtons();
        UpdateWeaponButton();  
    }

    private void UpdateWeaponImage()
    {
        weaponType.sprite = weaponTypes[currentIndex];
    }

    private void UpdateArrowButtons()
    {
        leftBtn.SetActive(currentIndex > 0);
        rightBtn.SetActive(currentIndex < weaponTypes.Length - 1);
    }

    public void rightBtnPressed()
    {
        if (currentIndex < weaponTypes.Length - 1)
        {
            currentIndex++;
            UpdateWeaponImage();
            UpdateArrowButtons();
            UpdateWeaponButton();  
        }
    }

    public void leftBtnPressed()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateWeaponImage();
            UpdateArrowButtons();
            UpdateWeaponButton();  
        }
    }

    public void backHomePressed()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("mainScene");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void UpdateWeaponButton()
    {
        int ownedWeapons = gameManager.gameManagerInstance.ownedWeapons;
        if (ownedWeapons == 0)
        {
            if (currentIndex == 0)
            {
                SetButtonState("use");
            }
            else
            {
                SetButtonState("buy");
            }
        }
        else if (ownedWeapons == 1)
        {
            if (currentIndex == 0 || currentIndex == 1)
            {
                SetButtonState("use");
            }
            else
            {
                SetButtonState("buy");
            }
        }
        else if (ownedWeapons == 2)
        {
            if (currentIndex == 0 || currentIndex == 1 || currentIndex == 2)
            {
                SetButtonState("use");
            }
            else
            {
                SetButtonState("buy");
            }
        }
        else if (ownedWeapons == 3)
        {
            SetButtonState("use");
        }
    }

    private void SetButtonState(string state)
    {
        if (state == "use")
        {
            selectBtn.SetActive(true);
            buyBtn.SetActive(false);
        }
        else if (state == "buy")
        {
            selectBtn.SetActive(false);
            buyBtn.SetActive(true);
        }
    }

    public void selectBtnPressed()
    {
        gameManager.gameManagerInstance.weaponType = currentIndex;
        gameManager.gameManagerInstance.SaveProgress();
    }

    public void buyBtnPressed()
    {
        if (currentIndex == 1)
        {
            updateCoins(200);
        } else if (currentIndex == 2)
        {
            updateCoins(350);
        } else if (currentIndex == 3){
            updateCoins(500);
        }
        gameManager.gameManagerInstance.ownedWeapons++;
        gameManager.gameManagerInstance.SaveProgress();
        buyBtn.SetActive(false);
        selectBtn.SetActive(true);
    }

    void updateCoins(int coins)
    {
        if (gameManager.gameManagerInstance.coins >= coins)
        {
            gameManager.gameManagerInstance.coins -= coins;
            gameManager.gameManagerInstance.SaveProgress();
            coinsText.text = gameManager.gameManagerInstance.coins.ToString();
        } else
        {
            errorImg.gameObject.SetActive(true);
        }
        
    }

    public void errorBtnPressed()
    {
        errorImg.gameObject.SetActive(false);
    }
}