using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class HexagonMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] float repeatTrigger;
    float repeatZ;
    Vector3 startPos;

    //ProBuilderShape proBuilderShape;

    // void Start()
    // {
    //     startPos = transform.position;
    //     repeatZ = GetComponent<ProBuilderShape>().size.y / repeatTrigger;
    // }

    // Update is called once per frame
    void Update()
    {
        RepeatMove();
    }

    void RepeatMove()
    {
        transform.Translate(Vector3.up * -speed * Time.deltaTime);

        if (transform.position.z < startPos.z - repeatZ)
        {
            transform.position = startPos;
        }
    }
}
