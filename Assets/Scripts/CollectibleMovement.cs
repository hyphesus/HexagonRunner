using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleMovement : MonoBehaviour
{
    //[SerializeField] private float speed;
    [SerializeField] private float poolOffset;
    [SerializeField] private float turnSpeed;
    [SerializeField] private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        CollMovement();
    }

    void CollMovement()
    {
        transform.Translate(Vector3.forward * -player.GetComponent<PlayerMovement>().speed * Time.deltaTime);
        
        int childCount = transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            transform.GetChild(i).Rotate(Vector3.up * turnSpeed * Time.deltaTime);
            //transform.GetChild(i).Rotate(new Vector3(turnSpeed * Time.deltaTime, 0, turnSpeed * Time.deltaTime));
        }

        if (transform.position.z <= poolOffset)
        {
            PoolManager.instancePM.ReturnObjectsToPool(this.gameObject);
        }
    }
}
