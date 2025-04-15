using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class minimap : MonoBehaviour
{
    public static minimap instance;
    public Transform player;
    public Image playerMarker;
    public Camera minimapCamera;
    public Vector2 offset;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        offset = new Vector2(minimapCamera.transform.position.x, minimapCamera.transform.position.z);
    }

    public Vector2 WorldToMinimapPosition(Vector3 worldPosition)
    {
        Vector3 offset = worldPosition - minimapCamera.transform.position;
        return new Vector2(offset.x, offset.z);
    }

    void Update()
    {
        if (player != null && playerMarker != null)
        {
            Vector2 minimapPos = WorldToMinimapPosition(player.position);
            playerMarker.rectTransform.localPosition = minimapPos;
        }
    }
}
