using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Play tuþuna atandýðýnda sahneyi yükleyen fonksiyon
    public void OnPlayButtonPressed()
    {
        SceneManager.LoadScene("MainScene");
    }
}
