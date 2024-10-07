using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class XXXPlayerMovement : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] private float accelerationModifier;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private XXXMenuManager menuManager;
    [SerializeField] public bool isGameOver;
    [SerializeField] private MusicController musicController;
    [SerializeField] private SfxController sfxController;
    public bool isMoving = false;

    public GameObject tryPanel;
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
        isGameOver = false;
        Application.targetFrameRate = 120;
    }

    void Update()
    {
        PlayerMove();
    }

    public void PlayerMove()
    {
        if (!isGameOver)
        {
            AccelerateObjects();
            Vector3 moveVector = new Vector3(Input.GetAxis("Horizontal"), 0, 0);

            if (moveVector.x != 0)
            {
                isMoving = true;
                //player.Move(moveVector * speed * Time.deltaTime);
                hexagonMovement.transform.Rotate(new Vector3(0, moveVector.x * speed * AccelerateObjects() * Time.deltaTime, 0));
            }
            else
            {
                isMoving = false;
            }
        }
    }

    public float AccelerateObjects()
    {
        if (menuManager.isGameStarted)
        {
            speed += accelerationModifier;
            Debug.Log($"Speed: {speed}");
        }

        return speed;
    }

    public void ResetPlayerScore()
    {
        if (scoreManager != null)
        {
            scoreManager.ResetScore();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isGameOver)
        {
            if (other.gameObject.CompareTag("Collectible"))
            {
                scoreManager.UpdateScore();
                sfxController.PlayCoinSfx();
                other.gameObject.SetActive(false);
            }

            if (other.gameObject.CompareTag("Obsticle"))
            {
                isGameOver = true;
                PoolManager.instancePM.ClearScene();
                Time.timeScale = 0;
                Debug.Log("Game Over");
                //try again panelini açma
                tryPanel.gameObject.SetActive(true);
                musicController.PlayMenuMusic();
            }
        }
    }
}
