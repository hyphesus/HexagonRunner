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
        }
        else
        {
            isMoving = false;
        }
    }
}
