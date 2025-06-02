using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public GameObject panelRoot;

    public void ShowGameOver()
    {
        Debug.Log("Panel Acýlýyor");
        Time.timeScale = 0f;
        if (panelRoot != null)
            panelRoot.SetActive(true);
    }

    public void OnRetryButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnMainMenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // sahne adýný doðru yaz
    }
}
