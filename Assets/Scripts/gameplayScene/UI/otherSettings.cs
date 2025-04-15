using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class otherSettings : MonoBehaviour
{
    [SerializeField] Slider volume_slider;
    [SerializeField] Slider playerSpeed_slider;
    [SerializeField] Slider mouseSensitivity_slider;

    float mouseSensitivity = 100f;
    float speed = 5f;
    float volume = 0.5f;

    void Start()
    {
        volume_slider.onValueChanged.AddListener(updateVolume);
        playerSpeed_slider.onValueChanged.AddListener(UpdatePlayerSpeed);
        mouseSensitivity_slider.onValueChanged.AddListener(UpdateMouseSensitivity);

        gameManager.gameManagerInstance.mouseSensitivity = 100;
        gameManager.gameManagerInstance.playerSpeed = 5;
        gameManager.gameManagerInstance.SaveProgress();
    }

    public void updateVolume(float newVolume)
    {
        volume = Mathf.Lerp(0f, 1f, newVolume);
        AudioListener.volume = volume;
    }

    public void UpdateMouseSensitivity(float newSensitivity)
    {
        mouseSensitivity = Mathf.Lerp(50f, 150f, newSensitivity);
        gameManager.gameManagerInstance.mouseSensitivity = mouseSensitivity;
        gameManager.gameManagerInstance.SaveProgress();
    }

    public void UpdatePlayerSpeed(float newSpeed)
    {
        speed = Mathf.Lerp(3f, 7f, newSpeed);
        gameManager.gameManagerInstance.playerSpeed = speed;
        gameManager.gameManagerInstance.SaveProgress();
    }
}
