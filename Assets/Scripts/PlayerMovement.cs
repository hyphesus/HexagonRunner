using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;

    CharacterController player;
    HexagonMovement hexagonMovement;

    private void Awake()
    {
        player = GetComponent<CharacterController>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        PlayerMove();
    }

    void PlayerMove()
    {
        {
            Vector3 moveVector = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
            player.Move(moveVector * speed * Time.deltaTime);
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Coll1")
    //    {
    //        Debug.Log("Coll1");
    //        PlayerMove();
    //    }
    //}
}
