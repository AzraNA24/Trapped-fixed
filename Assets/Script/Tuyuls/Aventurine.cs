using System.Collections;
using UnityEngine;

public class Aventurine : Tuyul
{
    public int DebuffRoundsLeft = 0;
    private bool canFUA = false;

    public Aventurine()
    {
        Name = "Aventurine";
        maxHealth = 50;
        AttackPower = 10;
        Money = 30;
        Type = TuyulType.Aventurine;
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

        if (random.NextDouble() < 0.4)
        {
            int stolenAmount = random.Next(1, 101);
            if (playerCharacter.DeductMoney(stolenAmount))
            {
                TuyulAnim.SetTrigger("TPBP");
                Debug.Log($"{Name} menggunakan jurus rahasia: 'Tangan Panjang, Badan Pendek'. Kamu kehilangan uang sebesar {stolenAmount}!");
                yield return new WaitForSeconds(1f);
            }
        }

        if (random.NextDouble() < 0.4)
        {
            yield return StartCoroutine(UseTheGreatGatsby(playerCharacter));
        }
        else
        {
            yield return StartCoroutine(ExecuteNormalAttack(playerCharacter));
        }
    }

    public IEnumerator UseTheGreatGatsby(Player playerCharacter)
    {
        TuyulAnim.SetTrigger("Ulti");
        int Ultimate = AttackPower + AttackPower / 2;
        yield return new WaitForSeconds(1f);

        playerCharacter.TakeDamage(Ultimate);
        Debug.Log($"{Name} memberikan {AttackPower * 1.5} damage tambahan dengan jurus 'The Great Gatsby'! Sisa HP: {playerCharacter.currentHealth}");
        canFUA = true;
    }

    public override void NormalAttack(Player playerCharacter)
    {
        StartCoroutine(ExecuteNormalAttack(playerCharacter));
    }

    private IEnumerator ExecuteNormalAttack(Player playerCharacter)
    {
        playerCharacter.TakeDamage(AttackPower);
        TuyulAnim.SetTrigger("Throws");
        Debug.Log($"{Name} mengeluarkan jurus 'Ketimpuk Batu' dan memberikan {AttackPower} damage! Sisa HP: {playerCharacter.currentHealth}");

        yield return new WaitForSeconds(1f); // Jeda untuk animasi
    }

    public void FUA(Player playerCharacter, bool playerCrit)
    {
        if (canFUA && playerCrit)
        {
            TuyulAnim.SetTrigger("FUA");
            int FUA = AttackPower / 2;
            playerCharacter.TakeDamage(FUA);
            Debug.Log($"{Name} melakukan Follow-Up Attack dan memberikan {FUA} damage tambahan! Sisa HP: {playerCharacter.currentHealth}");
            canFUA = false; // Reset setelah melakukan FUA
        }
    }
}
