using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DeathPopupUI : MonoBehaviour
{
    public GameObject deathPanel;
    public TMP_Text scoreText;

    void Start()
    {
        deathPanel.SetActive(false);
    }

    public void ShowDeathPopup()
    {
        deathPanel.SetActive(true);
        scoreText.text = "Score : " + Mathf.FloorToInt(ScoreManager.score);
        Time.timeScale = 0f; 
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
