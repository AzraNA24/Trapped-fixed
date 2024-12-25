using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public struct LootTypeSprite
{
    public LootBox.LootType Type;
    public Sprite Icon;
}
public class LootBox : MonoBehaviour
{
    public LootType Type;

    public enum LootType
    {
        None, MoneyBag, HealthPotion, Gun, Bullet
    }

    private void Start()
    {
        if (Player.Instance == null)
        {
            Debug.LogError("Player reference is missing. Loot cannot be generated.");
        }
    }

    // Daftar LootType dan Sprite-nya
    public LootTypeSprite[] lootSprites;

    // Fungsi untuk mendapatkan sprite berdasarkan LootType
    public Sprite GetLootSprite(LootBox.LootType lootType)
    {
        foreach (var lootSprite in lootSprites)
        {
            if (lootSprite.Type == lootType)
            {
                return lootSprite.Icon;
            }
        }
        return null;
    }
    public void GenerateLoot()
    {
        Player currency = Player.Instance;
        if (currency == null || currency.Inventory == null)
        {
            Debug.LogError("Player or inventory reference is missing. Loot cannot be generated.");
            return;
        }

        int money = Random.Range(1, 4);
        int potion = Random.Range(0, 2);
        int bullet = Random.Range(0, 3);

        Debug.Log($"Loot generated: Money = {money}, Potion = {potion}, Bullet = {bullet}");

        currency.AddMoney(money);
        currency.Inventory.AddItem(LootType.HealthPotion, potion);
        currency.Inventory.AddItem(LootType.Bullet, bullet);

        Destroy(gameObject);
    }
}