using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuManager : MonoBehaviour
{

    [SerializeField] private GameObject menuPanel;
    public GameObject tryPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject pausePanel;


    //  [SerializeField] private Button menuB;//anamenuy� a�ar
    [SerializeField] private Button restartB;
    [SerializeField] private Button optionsB;
    [SerializeField] private Button playB;
    /* [SerializeField] private Button quitB;
     [SerializeField] private Button pauseB;
     [SerializeField] private Button resumeB;
     [SerializeField] private Button volumeB;*/
    [SerializeField] private Button returnB;




    void Start()
    {

        menuPanel.SetActive(true);
        tryPanel.SetActive(false);
        optionsPanel.SetActive(false);
        gamePanel.SetActive(false);
        pausePanel.SetActive(false);

        Time.timeScale = 0f;

        playB.onClick.AddListener(playGame);
        optionsB.onClick.AddListener(option);
        returnB.onClick.AddListener(returnMenu);



    }

    void Update()
    {

    }

    public void restart()
    {
        //
        Time.timeScale = 1f;
        menuPanel.SetActive(false);
        gamePanel.SetActive(true);
    }
    public void playGame()
    {
        Time.timeScale = 1f;
        menuPanel.SetActive(false);
        gamePanel.SetActive(true);

    }
    public void option()
    {
        menuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
    public void returnMenu()
    {
        optionsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void resume()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    public void pause()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        gamePanel.SetActive(false);

    }

    public void mainMenu()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 0f;
        /*tryPanel.SetActive(false);
        optionsPanel.SetActive(false);
        gamePanel.SetActive(false);
        pausePanel.SetActive(false);
        menuPanel.SetActive(true);*/


    }
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
