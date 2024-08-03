using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;

    public bool isMoving = false;

    CharacterController player;
    HexagonMovement hexagonMovement;
    EnemySpawn enemySpawn;
    private void Awake()
    {
        player = GetComponent<CharacterController>();
        hexagonMovement = FindObjectOfType<HexagonMovement>();
        enemySpawn = FindObjectOfType<EnemySpawn>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        PlayerMove();
    }

    public void PlayerMove()
    {
        
        Vector3 moveVector = new Vector3(Input.GetAxis("Horizontal"), 0, 0);

        if (moveVector.x != 0)
        {
            isMoving = true;
            player.Move(moveVector * speed * Time.deltaTime);
            hexagonMovement.transform.Rotate(new Vector3(0, moveVector.x, 0));

            //float _balance= hexagonMovement.transform.eulerAngles.x;
            //if (_balance%60 > 29 && _balance <31)
            //{
            //    hexagonMovement.transform.eulerAngles=new Vector3(30, hexagonMovement.transform.eulerAngles.y, hexagonMovement.transform.eulerAngles.z);

            //}

        }
        else
        {
            isMoving = false;
        }
        
        
        //foreach (var item in enemySpawn.oPrefab)
        //{
        //    item.transform.Rotate(new Vector3(0, moveVector.x, 0));
        //}
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
