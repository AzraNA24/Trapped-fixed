using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemUI : MonoBehaviour
{
    public TMP_Text QuantityBullet;
    public TMP_Text QuantityHealth;
    public Player player;

public void Update()
    {
        player = Player.Instance;
        QuantityBullet.text = "" + player.Inventory.GetItemCount(LootBox.LootType.Bullet).ToString();
        QuantityHealth.text = "" + player.Inventory.GetItemCount(LootBox.LootType.HealthPotion).ToString();
    }

}
