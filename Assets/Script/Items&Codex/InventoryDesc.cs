using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryDesc : MonoBehaviour
{
    [System.Serializable]
    public class Inventory
    {
        public string Title;
        public string Decs;
    }

    public Inventory[] Description;

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public Image targetImage;
    public Sprite[] InventorySprite;

    private void Awake()
    {
        InitializeDescriptions();
    }

    private void InitializeDescriptions()
    {
        Description = new Inventory[]
        {
            new Inventory {
                Title = "Walkie Talkie",
                Decs = "It's the agreed way to communicate with your partner. Hmm.. neither of you anticipate the lack of signal in this cursed place, though..",
            },
            new Inventory {
                Title = "Riffle",
                Decs = "When money can't get you out, A trusty gun can always be a reliable friend.",
            },
            new Inventory {
                Title = "Money bag",
                Decs = "Trusty money bag to bagged away your stolen money. Can be used as a weapon too. Inside, there's " +
                       (Player.Instance != null ? Player.Instance.Money.ToString() : "0") + " money.",
            },
            new Inventory {
                Title = "Health Potion",
                Decs = "Highly recommended for neck pain",
            },
            new Inventory {
                Title = "Bullets",
                Decs = "You Surely know what it is... right?",
            },
            new Inventory {
                Title = "Sigil of Pocong",
                Decs = "You took this token as a winning thropy againts Chaeng-Yul. It has ability to steal Tuyul's pocket money without actually fight them.",
            },
            new Inventory {
                Title = "Sigil of Kecoak",
                Decs = "You took this token as a winning thropy againts Chaeng-Yul. It has ability to poison other Tuyul.",
            },
        };
    }

    public void DisplayInventory(int index)
    {
        SetImageByIndex(index);
        Inventory Desc = Description[index];
        titleText.text = Desc.Title;
        descriptionText.text = Desc.Decs;
    }

    public void SetImageByIndex(int index)
    {
        if (index >= 0 && index < InventorySprite.Length)
        {
            targetImage.sprite = InventorySprite[index];
        }
        else
        {
            Debug.LogError("Indeks gambar tidak valid!");
        }
    }
}
