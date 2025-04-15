using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rewardGenerator : MonoBehaviour
{
    GameObject[] rewardArray;
    [SerializeField] GameObject coin;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject life;
    [SerializeField] GameObject rewardMarkerPrefab;
    RectTransform minimapRect; 

    float coinProbability = 0.6f;
    float bulletProbability = 0.25f;
    float spawnInterval = 30f;
    float worldMinX = -25f, worldMaxX = 25f, worldMinZ = 0f, worldMaxZ = 85f;

    GameObject newReward;
    GameObject rewardMarker;
    bool activeTimer = false;
    float timer;

    bool hasInit = false;
    bool initRewardGenerator = false;

    void Start()
    {
        timer = 7.5f;
        rewardArray = new GameObject[] { coin, bullet, life };
    }

    void SpawnReward()
    {
        Vector3 spawnPosition = generatePosition();
        newReward = Instantiate(rewardArray[rewardType()], spawnPosition, Quaternion.identity);

        if (rewardMarkerPrefab != null && gameManager.gameManagerInstance.canStart == 1)
        {
            rewardMarker = Instantiate(rewardMarkerPrefab, minimapRect);
            SetMinimapMarkerPosition(spawnPosition);
        }
        activeTimer = true;
    }

    void Update()
    {
        gameManager.gameManagerInstance.LoadProgress();
        if (gameManager.gameManagerInstance.gameType.Equals("one") && !hasInit)
        {
            initRewardGenerator = true;
        }

        if (!hasInit && initRewardGenerator)
        {
            InvokeRepeating("SpawnReward", 10f, spawnInterval);
            hasInit = true;
        }

        if (activeTimer)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                if (newReward != null)
                {
                    Destroy(newReward);
                }
                if (rewardMarker != null)
                {
                    Destroy(rewardMarker);
                }

                timer = 7.5f;
                activeTimer = false;
            }
        }
    }

    void SetMinimapMarkerPosition(Vector3 worldPosition)
    {
        float normalizedX = Mathf.InverseLerp(worldMinX, worldMaxX, worldPosition.x);
        float normalizedZ = Mathf.InverseLerp(worldMinZ, worldMaxZ, worldPosition.z);

        float minimapX = Mathf.Lerp(-minimapRect.rect.width / 2, minimapRect.rect.width / 2, normalizedX);
        float minimapZ = Mathf.Lerp(-minimapRect.rect.height / 2, minimapRect.rect.height / 2, normalizedZ);

        rewardMarker.GetComponent<RectTransform>().localPosition = new Vector3(minimapX, minimapZ, 0f);
    }

    int rewardType()
    {
        float randomValue = Random.Range(0f, 1f);

        if (randomValue < coinProbability)
        {
            return 0;
        }
        else if (randomValue < coinProbability + bulletProbability)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }

    Vector3 generatePosition()
    {
        int randomZone = Random.Range(0, 5);
        return generateZone(randomZone);
    }

    Vector3 generateZone(int zone)
    {
        float xmin, xmax, zmin, zmax;
        if (zone == 0)
        {
            xmin = -8f;
            xmax = 0f;
            zmin = 2f;
            zmax = 63f;
        }
        else if (zone == 1)
        {
            xmin = -15f;
            xmax = -9f;
            zmin = 27f;
            zmax = 56f;
        }
        else if (zone == 2)
        {
            xmin = 0f;
            xmax = 6f;
            zmin = 43f;
            zmax = 64f;
        }
        else if (zone == 3)
        {
            xmin = 6f;
            xmax = 17f;
            zmin = 46f;
            zmax = 52f;
        }
        else
        {
            xmin = 18f;
            xmax = 24f;
            zmin = 38f;
            zmax = 48f;
        }
        Vector3 zonePos = new Vector3(Random.Range(xmin, xmax), 5f, Random.Range(zmin, zmax));
        return zonePos;
    }
}