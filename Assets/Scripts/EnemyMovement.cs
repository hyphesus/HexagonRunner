using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //[SerializeField] private float speed;
    [SerializeField] private float poolOffset;
    [SerializeField] private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        EnemyMove();
    }

    void EnemyMove()
    {
        transform.Translate(Vector3.forward * -player.GetComponent<XXXPlayerMovement>().speed * Time.deltaTime);

        if (this.gameObject.activeSelf && transform.position.z <= poolOffset)
        {
            PoolManager.instancePM.ReturnObjectsToPool(this.gameObject);
        }
    }
}
