using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using TMPro;
using UnityEngine;

public class Canvas : MonoBehaviour
{
    public static Canvas Instance { get; private set; }
    public TextMeshProUGUI money;
    public TextMeshProUGUI Bullet;
    public TextMeshProUGUI Potion;
    public Player player;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Update()
    {
        player = Player.Instance;
        money.text = "Money: " + player.Money.ToString();
        Bullet.text = "Bullet: " + player.Inventory.GetItemCount(LootBox.LootType.Bullet).ToString();
        Potion.text = "Potion: " + player.Inventory.GetItemCount(LootBox.LootType.HealthPotion).ToString();
    }
}
