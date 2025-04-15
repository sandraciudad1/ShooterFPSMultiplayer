using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sightController : MonoBehaviour
{
    [SerializeField] private Camera camera;
    float normalFOV = 60f;
    float aimSpeed = 10f;

    void Update()
    {
        gameManager.gameManagerInstance.weaponType = 3;
        gameManager.gameManagerInstance.SaveProgress();
        if (Input.GetMouseButton(1) && gameManager.gameManagerInstance.weaponType >= 1) 
        {
            if (gameManager.gameManagerInstance.weaponType == 1) 
            {
                increaseFOV(50f);
            } 
            else if (gameManager.gameManagerInstance.weaponType == 2) 
            {
                increaseFOV(35f);
            } 
            else if (gameManager.gameManagerInstance.weaponType == 3)
            {
                increaseFOV(20f);
            }
        }
        else
        {
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, normalFOV, Time.deltaTime * aimSpeed);
        }
    }

    void increaseFOV(float fov)
    {
        camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, fov, Time.deltaTime * aimSpeed);
    }
}
