using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Rendering;

[System.Serializable]
public class InventoryItem : MonoBehaviour
{
    [System.Serializable]
    public class Slots
    {
        public LootBox.LootType Type;
        public int Count;
        public int Max;
        public Sprite icon;

        public Slots(LootBox.LootType type, int maxCount)
        {
            Type = type;
            Count = 0;
            Max = maxCount;
        }
    }

    public List<Slots> slot = new List<Slots>();

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public bool AddItem(LootBox.LootType itemType, int amount)
    {
        foreach (Slots slots in slot)
        {
            if (slots.Type == itemType || slots.Type == LootBox.LootType.None)
            {
                if (slots.Count + amount <= slots.Max)
                {
                    slots.Type = itemType;
                    slots.Count += amount;
                    Debug.Log("Ditambahkan sprite");
                    Debug.Log($"Item {itemType} added: {amount}. Total: {slots.Count}");
                    return true;
                }
                Debug.Log($"Slot full for item {itemType}. Cannot add.");
                return false;
            }
        }
        Debug.Log("No available slot for this item!");
        return false;
    }

    public bool UseItem(LootBox lootBox, int amount)
    {
        foreach (Slots slots in slot)
        {
            if (slots.Type == lootBox.Type && slots.Count >= amount)
            {
                slots.Count -= amount;

                if (slots.Count == 0)
                {
                    Debug.Log($"Item {lootBox.Type} habis, slot kosong kembali.");
                    slots.Type = LootBox.LootType.None; // Reset slot jika kosong
                }

                Debug.Log($"Item {lootBox.Type} used: {amount}. Remaining: {slots.Count}");
                return true;
            }
        }
        Debug.Log($"Not enough {lootBox.Type} in inventory!");
        return false;
    }

    public int GetItemCount(LootBox.LootType itemType)
    {
        foreach (Slots slots in slot)
        {
            if (slots.Type == itemType)
            {
                return slots.Count;
            }
        }
        Debug.Log($"Item {itemType} tidak ditemukan di inventory.");
        return 0;
    }
}
