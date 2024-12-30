using System.Collections;
using UnityEngine;

public class MrRizzler : Tuyul
{
    public int DebuffRoundsLeft = 0;

    public MrRizzler()
    {
        Name = "Mr. Rizzler; The Charmer of Chaos";
        maxHealth = 50;
        AttackPower = 10;
        Money = 30;
        Type = TuyulType.MrRizzler;
    }

    public override bool TakeDamage(int damage, Player playerCharacter)
    {
        currentHealth -= damage;

        FindObjectOfType<BattleHUD>().SetHP(currentHealth);

        Debug.Log($"{Name} menerima {damage} damage! Sisa HP: {currentHealth}");

        // Offer to surrender if health is low
        if (currentHealth > 0 && currentHealth <= maxHealth * 0.3f && !isOfferingMoney)
        {
            Debug.Log($"{Name} menyerang pemain terlebih dahulu sebelum menawarkan deal.");
            StartCoroutine(ExecuteNormalAttack(playerCharacter));

            // Setelah serangan selesai, mulai tawaran
            StartCoroutine(OfferDeal(playerCharacter));
            return false; 
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            return true; // Tuyul is dead
        }

        return false;
    }

    public override IEnumerator OfferDeal(Player playerCharacter)
    {
        yield return new WaitForSeconds(1f); // Jeda untuk memastikan serangan selesai
        isOfferingMoney = true;
        Debug.Log($"{Name} menawarkan uang sebesar {Money} untuk ganti nyawanya. Terima? (1 = Iya, 2 = Tidak)");
        yield return StartCoroutine(WaitForPlayerChoice(playerCharacter)); // Tunggu input pemain
    }

    public override void EnemyAction(Player playerCharacter)
    {
        if (isOfferingMoney)
        {
            Debug.Log($"{Name} sedang menunggu keputusan pemain. Tidak melakukan aksi lain.");
            return;
        }

        StartCoroutine(ExecuteEnemyAction(playerCharacter));
    }
    
    private IEnumerator ExecuteEnemyAction(Player playerCharacter)
    {
        if (DebuffRoundsLeft > 0)
        {
            DebuffRoundsLeft--;
            Debug.Log($"{Name} terus memengaruhi critical chance pemain! Ronde tersisa: {DebuffRoundsLeft}");
        }

        if (random.NextDouble() < 0.3)
        {
            int stolenAmount = random.Next(1, 101);
            if (playerCharacter.DeductMoney(stolenAmount))
            {
                TuyulAnim.SetTrigger("TPBP");
                Debug.Log($"{Name} menggunakan jurus rahasia: 'Tangan Panjang, Badan Pendek'. Kamu kehilangan uang sebesar {stolenAmount}!");
                yield return new WaitForSeconds(1f);
            }
        }

        // Special Skill: Seduce You To Death (20% chance)
        if (random.NextDouble() < 0.2 && DebuffRoundsLeft == 0)
        {
            yield return StartCoroutine(UseSeduceYouToDeath(playerCharacter));
        }
        else
        {
            // basic attack
            NormalAttack(playerCharacter);
        }
    }
    
    public IEnumerator UseSeduceYouToDeath(Player playerCharacter)
    {
        TuyulAnim.SetTrigger("Seduce");
        yield return new WaitForSeconds(1f);

        DebuffRoundsLeft = 3;
        // Mengurangi critical chance pemain (10%)
        playerCharacter.criticalChance -= 0.1f;
        if (playerCharacter.criticalChance < 0) playerCharacter.criticalChance = 0;

        // Mengurangi efektivitas health potion (50%)
        playerCharacter.healthPotionEffectiveness -= 0.5f;
        if (playerCharacter.healthPotionEffectiveness < 0.1f) playerCharacter.healthPotionEffectiveness = 0.1f;

        Debug.Log($"{Name} menggunakan jurus spesial 'Seduce You To Death'! Efek health potion pemain berkurang dan critical chance turun menjadi {playerCharacter.criticalChance * 100}%.");
    }

    public void RemoveDebuff (Player playerCharacter){
        playerCharacter.criticalChance = 0.3f;
        playerCharacter.healthPotionEffectiveness = 1.0f;
        Debug.Log("Debuff diangkat!");
    }

    public override void NormalAttack(Player playerCharacter)
    {
        StartCoroutine(ExecuteNormalAttack(playerCharacter));
    }

    private IEnumerator ExecuteNormalAttack(Player playerCharacter)
    {
        TuyulAnim.SetTrigger("Throw");
        yield return new WaitForSeconds(1f);
        playerCharacter.TakeDamage(AttackPower);
        Debug.Log($"{Name} mengeluarkan jurus 'Ketimpuk Batu' dan memberikan {AttackPower} damage! Sisa HP: {playerCharacter.currentHealth}");
    }
}