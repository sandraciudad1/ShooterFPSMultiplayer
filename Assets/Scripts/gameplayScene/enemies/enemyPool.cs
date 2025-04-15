using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyPool : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    GameObject[] pool;
    private int poolSize = 10;
    int currentIndex = 0;

    private void Awake()
    {
        pool = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            pool[i] = Instantiate(enemyPrefab);
            pool[i].SetActive(false);
        }
    }

    public GameObject getObject(Vector3 spawnPosition)
    {
        if (currentIndex >= poolSize)
        {
            currentIndex = 0;
        }
        GameObject obj = pool[currentIndex];
        currentIndex++;

        obj.transform.position = spawnPosition; 
        obj.SetActive(true);
        return obj;
    }

    public void returnObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}
