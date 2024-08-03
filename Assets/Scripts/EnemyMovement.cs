using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed;

    HexagonMovement hexagonMovement;

    private void Awake()
    {
        hexagonMovement = FindObjectOfType<HexagonMovement>();
    }
    void Update()
    {
        EnemyMove();
    }

    void EnemyMove()
    {
        transform.Translate(Vector3.forward * -speed * Time.deltaTime);
    }
}
