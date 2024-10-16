using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//public class MenuManager : MonoBehaviour
//{
//    [SerializeField] private GameObject menuPanel;
//    public GameObject tryPanel;
//    [SerializeField] private GameObject optionsPanel;
//    [SerializeField] private GameObject gamePanel;
//    [SerializeField] private GameObject pausePanel;

//    [SerializeField] private Button restartB;
//    [SerializeField] private Button optionsB;
//    [SerializeField] private Button playB;
//    [SerializeField] private Button returnB;

//    [SerializeField] private LineToPlayerMovement lineToPlayerMovement;
//    [SerializeField] private MusicController musicController;

//    void Start()
//    {
//        menuPanel.SetActive(true);
//        tryPanel.SetActive(false);
//        optionsPanel.SetActive(false);
//        gamePanel.SetActive(false);
//        pausePanel.SetActive(false);
//        musicController.PlayMenuMusic();

//        Time.timeScale = 0f;

//        playB.onClick.AddListener(playGame);
//        optionsB.onClick.AddListener(option);
//        returnB.onClick.AddListener(returnMenu);
//    }

//    public void restart()
//    {
//        menuPanel.SetActive(false);
//        pausePanel.SetActive(false);
//        tryPanel.SetActive(false);
//        gamePanel.SetActive(true);
//        musicController.PlayInGameMusic();

//        ResetGame();

//        // Resume the game
//        Time.timeScale = 1f;
//    }

//    public void playGame()
//    {
//        Time.timeScale = 1f;
//        menuPanel.SetActive(false);
//        gamePanel.SetActive(true);
//        musicController.PlayInGameMusic();
//    }

//    public void option()
//    {
//        menuPanel.SetActive(false);
//        optionsPanel.SetActive(true);
//    }

//    public void returnMenu()
//    {
//        optionsPanel.SetActive(false);
//        menuPanel.SetActive(true);
//    }

//    public void resume()
//    {
//        Time.timeScale = 1f;
//        pausePanel.SetActive(false);
//        gamePanel.SetActive(true);
//        musicController.PlayInGameMusic();
//    }

//    public void pause()
//    {
//        Time.timeScale = 0f;
//        pausePanel.SetActive(true);
//        gamePanel.SetActive(false);
//        musicController.PlayMenuMusic();
//    }

//    public void mainMenu()
//    {
//        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//        Time.timeScale = 0f;
//    }

//    public void Quit()
//    {
//#if UNITY_EDITOR
//        UnityEditor.EditorApplication.isPlaying = false;
//#else
//        Application.Quit();
//#endif
//    }

//    private void ResetGame()
//    {
//        // Find the PlayerMovement script to reset necessary components
//        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();

//        if (playerMovement != null)
//        {
//            // Clear all spawned enemies and other objects
//            EnemySpawn enemySpawn = FindObjectOfType<EnemySpawn>();
//            //if (enemySpawn != null)
//            //{
//            //    enemySpawn.ClearAllSpawnedEnemies();
//            //}

//            playerMovement.speed = 5f; // Reset to your desired initial speed
//            playerMovement.isGameOver = false;
//            playerMovement.isMoving = false;

//            Transform parentObject = playerMovement.transform.parent;
//            if (parentObject != null)
//            {
//                parentObject.position = Vector3.zero;
//                parentObject.rotation = Quaternion.identity;
//            }

//            // Reset the player's score through the public method
//            playerMovement.ResetPlayerScore();

//            if (lineToPlayerMovement != null)
//            {
//                lineToPlayerMovement.ResetCurrentSide();
//            }
//        }

//        // Reset other game-specific components if needed
//        //PoolManager.instancePM.ClearAllPooledObjects();
//    }
//}
