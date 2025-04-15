using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class playerCameraSetUp : MonoBehaviourPun
{
    [SerializeField] private Camera playerCamera; 
    [SerializeField] private GameObject staticModelOther; 

    private void Start()
    {
        if (photonView.IsMine)
        {
            // Configures local player camera 
            SetCameraCullingMask(playerCamera, "mine", true);
            SetCameraCullingMask(playerCamera, "other", true);
            staticModelOther.SetActive(false);
        }
        else
        {
            // Configures other player cameras
            SetCameraCullingMask(playerCamera, "mine", false);
            SetCameraCullingMask(playerCamera, "other", true);
        }
    }

    private void SetCameraCullingMask(Camera camera, string layerName, bool include)
    {
        int layer = LayerMask.NameToLayer(layerName);
        if (layer == -1)
        {
            Debug.LogError($"La capa '{layerName}' no existe. Asegúrate de que esté configurada en el editor.");
            return;
        }

        if (include)
        {
            camera.cullingMask |= 1 << layer; // Adds layer
        }
        else
        {
            camera.cullingMask &= ~(1 << layer); // Removes layer
        }
    }
}
