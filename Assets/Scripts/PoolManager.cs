using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instancePM;

    [SerializeField] private GameObject[] obsticlePrefabs;
    [SerializeField] private GameObject[] collectiblePrefabs;
    [SerializeField] public List<GameObject> pooledObjects;
    [SerializeField] private int poolFactor;
    EnemySpawn enemySpawn;
    HexagonMovement hexagonMovement;

    private void Awake()
    {
        if (instancePM == null)
        {
            instancePM = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

        CreatePool();
    }

    public void CreatePool()
    {
        enemySpawn = FindObjectOfType<EnemySpawn>();
        hexagonMovement = FindObjectOfType<HexagonMovement>();

        pooledObjects = new List<GameObject>();
        poolFactor = (int)enemySpawn.spawnPeriod;

        for (int j = 0; j < poolFactor; j++)
        {
            for (int i = 0; i < obsticlePrefabs.Length; i++)
            {
                GameObject obj = Instantiate(obsticlePrefabs[i]);
                pooledObjects.Add(obj);
                obj.transform.SetParent(hexagonMovement.transform);
                obj.SetActive(false);
            }

            for (int i = 0; i < collectiblePrefabs.Length; i++)
            {
                GameObject obj = Instantiate(collectiblePrefabs[i]);
                pooledObjects.Add(obj);
                obj.transform.SetParent(hexagonMovement.transform);
                obj.SetActive(false);
            }
        }
    }

    public GameObject GetObjectFromPool()
    {
        int rObj = Random.Range(0, pooledObjects.Count);

        if (!pooledObjects[rObj].activeSelf)
        {
            pooledObjects[rObj].SetActive(true);

            for (int i = 0; i < pooledObjects[rObj].transform.childCount; i++)
            {
                pooledObjects[rObj].transform.GetChild(i).gameObject.SetActive(true);
            }
            return pooledObjects[rObj];
        }
        else
        {
            GameObject obj= enemySpawn.SpawnObsticle();
            return obj;
        }
    }

    public void ReturnObjectsToPool(GameObject obj)
    {
        obj.transform.position = enemySpawn.spawnPoint.position;
        obj.gameObject.SetActive(false);
    }

    public void ClearScene()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            pooledObjects[i].transform.position = enemySpawn.spawnPoint.position;
            pooledObjects[i].SetActive(false);
        }
    }

    //public void ClearAllPooledObjects()
    //{
    //    foreach (GameObject obj in pooledObjects)
    //    {
    //        Destroy(obj);
    //    }
    //    pooledObjects.Clear();
    //}
}
