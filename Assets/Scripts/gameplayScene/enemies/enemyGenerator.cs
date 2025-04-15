using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyGenerator : MonoBehaviour
{
    [SerializeField] enemyPool enemyPool;
    public float spawnInterval = 10f;

    public GameObject enemyMarkerPrefab; 
    private List<GameObject> activeMarkers = new List<GameObject>();

    bool initEnemyGenerator = false, hasInit = false;

    void Update()
    {
        gameManager.gameManagerInstance.LoadProgress();
        if (gameManager.gameManagerInstance.gameType.Equals("one") && !hasInit)
        {
            initEnemyGenerator = true;
        }

        if (!hasInit && initEnemyGenerator)
        {
            InvokeRepeating("SpawnEnemy", 2f, spawnInterval);
            hasInit = true;
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-17f, 17f), 5f, Random.Range(2f, 82f));
        GameObject newEnemy = enemyPool.getObject(spawnPosition);

        if (gameManager.gameManagerInstance.canStart==1)
        {
            // Creates a marker in minimap for each enemy position
            GameObject newMarker = Instantiate(enemyMarkerPrefab, minimap.instance.transform);
            activeMarkers.Add(newMarker);
            newEnemy.GetComponent<enemyMovement>().SetMinimapMarker(newMarker);
        }
    }

    public void RemoveMarker(GameObject marker)
    {
        activeMarkers.Remove(marker);
        Destroy(marker);
    }
}
