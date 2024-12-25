using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class WinningController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winningMessageText; // Reference to the UI TM element
    private string winningMessage;
    public SceneTransition sceneTransition;

    private void Start()
    {
        // Ambil pesan dari PlayerPrefs
        winningMessage = PlayerPrefs.GetString("WinningMessage", "KELAZZZ.");
        winningMessageText.text = winningMessage;
    }

    public void ReturnToMainMenu(string sceneName)
    {
        sceneTransition.TransitionToScene(sceneName);
    }
}
