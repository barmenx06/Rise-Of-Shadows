using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public MapProfile[] availableMaps;

    public void OnPlayButtonClicked()
    {
        int index = Random.Range(0, availableMaps.Length);
        MapManager.Instance.currentMap = availableMaps[index];

        // Oyun sahnesinin adýný yaz: örn. "GameScene"
        SceneManager.LoadScene("MainScene");
    }
}
