using UnityEngine;


public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;
    public int TotalMoney = 100;
    // public ItemHUD itemHUD;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AddMoney(int amount)
    {
        if (amount > 0)
        {
            TotalMoney += amount;
            Debug.Log($"Uang ditambahkan sebesar {amount}. Total uang: {TotalMoney}");
            // itemHUD.UpdateMoneyUI();
        }
    }

    public bool DeductMoney(int amount)
    {
        if (TotalMoney >= amount)
        {
            TotalMoney -= amount;
            Debug.Log($"Uang dikurangi sebesar {amount}. Sisa uang: {TotalMoney}");
            // itemHUD.UpdateMoneyUI();
            return true;
        }
        else
        {
            Debug.Log("Uang tidak cukup.");
            return false;
        }
    }

    public void ShowBalance()
    {
        Debug.Log($"Saldo saat ini: {TotalMoney}");
    }

}