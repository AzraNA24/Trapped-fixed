using UnityEngine;
using UnityEngine.UI;

public class Codex: MonoBehaviour
{
    public Text titleText;
    public Text descText;
    public Image imageDesc;

    // Data codex entries
    [System.Serializable]
    public class CodexEntry
    {
        public string itemName;
        [TextArea]
        public string itemDescription;
        public Sprite itemImage;
    }

    public CodexEntry[] codexEntries;
    public void ShowEntry(int index)
    {
        if (index >= 0 && index < codexEntries.Length)
        {
            titleText.text = codexEntries[index].itemName;
            descText.text = codexEntries[index].itemDescription;
            imageDesc.sprite = codexEntries[index].itemImage;
        }
    }
}
