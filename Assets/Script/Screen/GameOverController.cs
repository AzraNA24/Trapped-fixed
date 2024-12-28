using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameOverMessageText; // Reference to the UI text element
    private string gameOverMessage;
    public SceneTransition sceneTransition;

    private void Start()
    {
        // Ambil pesan dari PlayerPrefs
        gameOverMessage = PlayerPrefs.GetString("GameOverMessage", "HAHAHA NT.");
        gameOverMessageText.text = gameOverMessage;
    }

    public void ReturnToMainMenu(string sceneName)
    {
        sceneTransition.TransitionToScene(sceneName);
    }
}
