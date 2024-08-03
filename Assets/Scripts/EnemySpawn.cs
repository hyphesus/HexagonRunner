using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject[] oPrefab;
    [SerializeField] private Vector3[] oRotation;
    [SerializeField] private float oSpeed;
    [SerializeField] private int oRandom;
    [SerializeField] private int rRandom;
    [SerializeField] private float spawnStartTime;
    [SerializeField] private float spawnPeriod;

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
        //balance = hexagonMovement.transform.eulerAngles.x;
    }

    void SpawnObsticle()
    {
        oRandom = Random.Range(0, oPrefab.Length);
        rRandom = Random.Range(0, oRotation.Length);
       
        GameObject spawnedObsticle = Instantiate(oPrefab[oRandom], oPrefab[oRandom].transform.position, Quaternion.Euler(oRotation[oRandom]));
        spawnedObsticle.transform.SetParent(hexagonMovement.gameObject.transform);

        //balance = Quaternion.Angle(hexagonMovement.transform.rotation, spawnedObsticle.transform.rotation);

        //spawnedObsticle.transform.localRotation = Quaternion.Euler(spawnedObsticle.transform.eulerAngles.x, spawnedObsticle.transform.eulerAngles.y,spawnedObsticle.transform.eulerAngles.z - balance);
    }
}
