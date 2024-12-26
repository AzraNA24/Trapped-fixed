using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Canvas : MonoBehaviour
{
    public static Canvas Instance { get; private set; }
    public TextMeshProUGUI money;
    public TextMeshProUGUI health;
    public TextMeshProUGUI name;

    public Player player;
    public Slider hpSlider;
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
        health.text = player.currentHealth.ToString();
        name.text = player.Name;
        hpSlider.maxValue = player.Health;
        hpSlider.value = player.currentHealth;
        
    }
}
