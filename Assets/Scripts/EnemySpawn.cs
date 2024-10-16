using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private Vector3[] oRotation;
    [SerializeField] private float oSpeed;
    [SerializeField] private int oRandom;
    [SerializeField] private int rRandom;
    [SerializeField] private float spawnStartTime;
    [SerializeField] public float spawnPeriod;
    [SerializeField] public Transform spawnPoint;
    [SerializeField] private PoolManager poolManager;


    HexagonMovement hexagonMovement;

    private void Awake()
    {
        hexagonMovement = FindObjectOfType<HexagonMovement>();
    }

    void Start()
    {
        InvokeRepeating("SpawnObsticle", spawnStartTime, spawnPeriod);
    }

    public GameObject SpawnObsticle()
    {
        rRandom = Random.Range(0, oRotation.Length);
        spawnPoint.localRotation = Quaternion.Euler(oRotation[rRandom]);
        GameObject spawnedObsticle = poolManager.GetObjectFromPool();
        spawnedObsticle.transform.rotation = spawnPoint.rotation;
        Debug.Log($"PoolednObjects Count: {poolManager.pooledObjects.Count}");
        spawnedObsticle.transform.SetParent(hexagonMovement.gameObject.transform);
        return spawnedObsticle;
    }

    //public void ClearAllSpawnedEnemies()
    //{
    //    foreach (GameObject enemy in poolManager.pooledObjects)
    //    {
    //        Destroy(enemy);
    //    }
    //    poolManager.pooledObjects.Clear();
    //}
}
