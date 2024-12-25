using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private InventoryItem inventoryItem;

    [SerializeField]
    private CodexUI codexUI;

    public ItemUI inventoryUI;
    public List<ItemUI> slots = new List<ItemUI>();
    public Player player;
    public int inventorySize = 10;
    public AudioSource Open;
    public static InventoryController Instance;

    private void Awake()
    {
        // Validasi Null
        if (inventoryItem == null)
        {
            Debug.Log("inventoryItem is not assigned in the Inspector!");
        }

        if (codexUI == null)
        {
            Debug.Log("CodexUI is not assigned in the Inspector!");
        }

        if (Open == null)
        {
            Debug.Log("AudioSource (Open) is not assigned in the Inspector!");
        }
        Setup();
    }

    private void Update()
    {
        // Membuka/Tutup Inventory
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (inventoryItem != null)
            {
                if (!inventoryItem.isActiveAndEnabled)
                {
                    PlayOpenSound();
                    inventoryItem.Show();
                    Setup();
                    Debug.Log("Inventory terbuka");
                }
                else
                {
                    inventoryItem.Hide();
                    Debug.Log("Inventory tertutup");
                }
            }
        }

        // Membuka/Tutup Codex
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (codexUI != null)
            {
                if (!codexUI.isActiveAndEnabled)
                {
                    PlayOpenSound();
                    codexUI.Show();
                    Debug.Log("Codex terbuka");
                }
                else
                {
                    codexUI.Hide();
                    Debug.Log("Codex tertutup");
                }
            }
        }
    }

    private void PlayOpenSound()
    {
        if (Open != null && !Open.isPlaying)
        {
            Open.Play();
        }
    }

    void Setup()
    {
            if(slots.Count == player.Inventory.slot.Count)
            {
                for(int j = 0; j < slots.Count; j++)
                {
                    if(player.Inventory.slot[j].Type != LootBox.LootType.None)
                    {
                        // slots[j].SetItem(player.Inventory.slot[j]);
                        Debug.Log("Terisi");
                    }
                    else
                    {
                        // slots[j].SetEmpty();
                        Debug.Log("Kosong");
                    }
                }
            }
    }
}
