using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TextMeshProUGUI coinTxt;

    [HideInInspector] public int coinScore = 0;
    [SerializeField] GameObject player;
    PlayerHealth playerHealth;
    PlayerScript playerScript;

    [SerializeField] AudioSource reviveSound;

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        playerHealth = FindObjectOfType<PlayerHealth>();
        playerScript = FindObjectOfType<PlayerScript>();
        Cursor.visible = false; 
        coinTxt = GetComponentInChildren<TextMeshProUGUI>();
        ResumeGame();
    }

    private void Update()
    {
        coinTxt.text = $"{coinScore}";  
        coinScore = Mathf.Clamp(coinScore, 0, 100);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        UnityEngine.Application.Quit();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void Revive()
    {
        if (coinScore > 4)
        {
            reviveSound.Play();
            coinScore -= 5;
            gameOverPanel.SetActive(false);
            playerHealth.numOflives = 3;
            player.SetActive(true);
            playerScript.enabled = true;
            playerScript.controller.enabled = true;
        }
        else return;
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

