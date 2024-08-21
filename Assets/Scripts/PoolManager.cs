using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instancePM; 

    [SerializeField] private GameObject[] obsticlePrefabs;
    [SerializeField] private GameObject[] collectiblePrefabs;
    [SerializeField] public List<GameObject> pooledObjects;
    [SerializeField] private EnemySpawn enemySpawn;

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

        pooledObjects = new List<GameObject>();

        for (int i = 0; i < obsticlePrefabs.Length; i++)
        {
            GameObject obj=Instantiate(obsticlePrefabs[i]);
            pooledObjects.Add(obj);
            obj.SetActive(false);
        }

        for (int i = 0;i < collectiblePrefabs.Length; i++)
        {
            GameObject obj = Instantiate(collectiblePrefabs[i]);
            pooledObjects.Add(obj);
            obj.SetActive(false);
        }
    }

    public GameObject GetObjectFromPool()
    {
        int rObj = Random.Range(0, pooledObjects.Count);

        if (!pooledObjects[rObj].activeSelf)
        {
            pooledObjects[rObj].SetActive(true);
            return pooledObjects[rObj];
        }
        else
        {
            GameObject obj = GetObjectFromPool();
            return obj;
        }
    }

    public void ReturnObjectsToPool(GameObject obj)
    {
        obj.transform.position = enemySpawn.spawnPoint.position;
        obj.gameObject.SetActive(false);
    }
}
