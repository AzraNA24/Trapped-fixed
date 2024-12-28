using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Story : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    public GameObject ContinueButton;
    public SceneTransition sceneTransition;

    private int index;
    private bool isTyping;

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        ContinueButton.SetActive(false);
        StartStory();
    }

    public void OnContinueButtonClicked()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            textComponent.text = lines[index];
            isTyping = false;
            ContinueButton.SetActive(true);
        }
        else
        {
            NextLine();
        }
    }

    void StartStory()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        textComponent.text = string.Empty;
        ContinueButton.SetActive(false);

        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
        ContinueButton.SetActive(true);
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            StartCoroutine(TypeLine());
        }
        else
        {
            TransitionToStartRoom();
        }
    }

    void TransitionToStartRoom()
    {
        sceneTransition.TransitionToScene("StartRoom");
    }
}
