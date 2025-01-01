using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public string Name;
    public int Health = 100;
    public int currentHealth { get; set; } //{ get; private set; }
    public int Money;
    public InventoryItem Inventory;
    public float criticalChance = 0.3f; // Default 30%
    public float healthPotionEffectiveness = 1.0f; // Default 100%

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializeInventory();
        currentHealth = Health;
    }

    private void InitializeInventory()
    {
        if (Inventory == null)
        {
            Inventory = gameObject.AddComponent<InventoryItem>();
            Inventory.slot = new List<InventoryItem.Slots>();
            Inventory.slot.Add(new InventoryItem.Slots(LootBox.LootType.None, 99));
            Inventory.slot.Add(new InventoryItem.Slots(LootBox.LootType.Bullet, 5));
            Inventory.slot.Add(new InventoryItem.Slots(LootBox.LootType.HealthPotion, 1));
            Debug.Log("Inventory successfully initialized.");
        }
    }

    public bool TakeDamage(int damage)
    {
        Debug.Log($"Health sebelum: {currentHealth}, damage diterima: {damage}");
        currentHealth -= damage;
        Debug.Log($"Health sesudah: {currentHealth}");
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            return true; // Player is dead
        }
        return false;
    }

    public void Heal(int amount)
    {
        currentHealth += Mathf.RoundToInt(amount * healthPotionEffectiveness);
        currentHealth = Mathf.Clamp(currentHealth, 0, Health);
    }

    public void AddMoney(int amount)
    {
        if (amount > 0)
        {
            Money += amount;
            Debug.Log($"Money added: {amount}. Total money: {Money}");
        }
    }

    public bool DeductMoney(int amount)
    {
        if (Money >= amount)
        {
            Money -= amount;
            Debug.Log($"Money deducted: {amount}. Remaining money: {Money}");
            return true;
        }
        Debug.Log("Insufficient money.");
        return false;
    }

    public bool HasBullets()
    {
        return Inventory.GetItemCount(LootBox.LootType.Bullet) > 0;
    }
    public bool UsePotion()
    {
        LootBox healthPotionBox = new LootBox { Type = LootBox.LootType.HealthPotion };
        if (Inventory.UseItem(healthPotionBox, 1))
        {
            Heal(20);
            Debug.Log($"Potion used. Current health: {currentHealth}");
            return true;
        }
        Debug.Log("No potion available!");
        return false;
    }

    public bool UseBullet()
    {
        LootBox bulletBox = new LootBox { Type = LootBox.LootType.Bullet };
        if (Inventory.UseItem(bulletBox, 1))
        {
            Debug.Log("Bullet used.");
            return true;
        }
        Debug.Log("No bullet available!");
        return false;
    }

    public void ResetInventory()
    {
        Inventory.slot = new List<InventoryItem.Slots>
        {
            new InventoryItem.Slots(LootBox.LootType.None, 99),
            new InventoryItem.Slots(LootBox.LootType.Bullet, 5), // Peluru awal
            new InventoryItem.Slots(LootBox.LootType.HealthPotion, 1) // Potion awal
        };

        Debug.Log("Inventory di-reset ke default.");
    }
}