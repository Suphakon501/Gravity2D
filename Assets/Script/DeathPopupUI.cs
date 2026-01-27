using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathPopupUI : MonoBehaviour
{
    public GameObject deathPanel;

    void Start()
    {
        deathPanel.SetActive(false);
    }

    public void ShowDeathPopup()
    {
        deathPanel.SetActive(true);
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
