using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float poolOffset;
    [SerializeField] private GameObject player;

    HexagonMovement hexagonMovement;

    private void Awake()
    {
        hexagonMovement = FindObjectOfType<HexagonMovement>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        EnemyMove();
    }

    void EnemyMove()
    {
        transform.Translate(Vector3.forward * -speed * Time.deltaTime);

        if (transform.position.z <= poolOffset)
        {
            PoolManager.instancePM.ReturnObjectsToPool(this.gameObject);
        }
    }
}
