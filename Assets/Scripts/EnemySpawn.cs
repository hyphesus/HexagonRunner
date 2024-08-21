using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawn : MonoBehaviour
{
    //[SerializeField] private GameObject[] oPrefab;
    [SerializeField] private Vector3[] oRotation;
    [SerializeField] private float oSpeed;
    [SerializeField] private int oRandom;
    [SerializeField] private int rRandom;
    [SerializeField] private float spawnStartTime;
    [SerializeField] private float spawnPeriod;
    [SerializeField] public Transform spawnPoint;
    [SerializeField] private PoolManager poolManager;

    float balance;

    HexagonMovement hexagonMovement;
    PlayerMovement playerMovement;
    private void Awake()
    {
        hexagonMovement = FindObjectOfType<HexagonMovement>();
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    void Start()
    {
        InvokeRepeating("SpawnObsticle", spawnStartTime, spawnPeriod);
    }

    void Update()
    {

    }

    void SpawnObsticle()
    {
        //oRandom = Random.Range(0, oPrefab.Length);
        rRandom = Random.Range(0, oRotation.Length);
        spawnPoint.localRotation = Quaternion.Euler(oRotation[rRandom]);
        //GameObject spawnedObsticle = Instantiate(oPrefab[oRandom], spawnPoint.position, spawnPoint.rotation);
        GameObject spawnedObsticle = poolManager.GetObjectFromPool();
        spawnedObsticle.transform.rotation = spawnPoint.rotation;
        Debug.Log($"PoolednObjects Count: {poolManager.pooledObjects.Count}");
        spawnedObsticle.transform.SetParent(hexagonMovement.gameObject.transform);
    }
}
