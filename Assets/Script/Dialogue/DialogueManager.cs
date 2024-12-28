using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueTextBox;
    public Image spriteImage;
    public RectTransform dialogueBox;
    public Button continueButton;

    public Dialogue[] dialogues;
    private int currentDialogueIndex = 0;
    private Coroutine typingCoroutine;

    void Start()
    {
        ShowDialogue(0); // Tampilkan dialog pertama
    }

    public void ShowDialogue(int index)
    {
        if (index < 0 || index >= dialogues.Length)
        {
            Debug.Log("Dialog sudah selesai!");
            return;
        }

        Dialogue dialogue = dialogues[index];
        spriteImage.sprite = dialogue.dialogueSprite;
        dialogueBox.anchoredPosition = dialogue.dialoguePosition;

        // Mulai efek mengetik
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeText(dialogue.dialogueText));
    }

    private IEnumerator TypeText(string text)
    {
        dialogueTextBox.text = ""; // Kosongkan teks terlebih dahulu
        foreach (char letter in text)
        {
            dialogueTextBox.text += letter; // Tambahkan huruf satu per satu
            yield return new WaitForSeconds(0.05f); // Atur kecepatan mengetik (dalam detik)
        }
    }

    public void ShowNextDialogue()
    {
        currentDialogueIndex++;
        if (currentDialogueIndex < dialogues.Length)
        {
            ShowDialogue(currentDialogueIndex);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
