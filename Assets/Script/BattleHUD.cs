using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{

    public TextMeshProUGUI nameText;
    public Slider hpSlider;
    public TextMeshProUGUI hpText;

    public void SetHUDTuyul(Tuyul enemyCharacter)
    {
        nameText.text = enemyCharacter.Name;
        hpSlider.maxValue = enemyCharacter.maxHealth;
        hpSlider.value = enemyCharacter.currentHealth;
        hpText.text = $"{enemyCharacter.currentHealth}";

    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }


}
