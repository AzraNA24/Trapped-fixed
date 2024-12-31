using UnityEngine;
using TMPro;

public class DialogueBattle : MonoBehaviour
{
    public static DialogueBattle Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI dialogText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateDialog(string message)
    {
        if (dialogText != null)
        {
            dialogText.text = message;
            Debug.Log($"Dialog Updated: {message}");
        }
        else
        {
            Debug.LogError("Dialog Text not assigned in DialogueBattle!");
        }
    }
}