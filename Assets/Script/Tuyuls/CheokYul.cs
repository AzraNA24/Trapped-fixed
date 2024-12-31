using System.Collections;
using UnityEngine;

public class CheokYul : Tuyul
{
    public int DebuffRoundsLeft = 0;
    private bool isFlying = false; // Status untuk passive skill
    private int poisonDuration = 3; // Durasi poison effect (3 giliran)

    public CheokYul()
    {
        Name = "CheokYul; Impish Who Studies under The Ancient Beast [Kecoak Terbang]";
        maxHealth = 200;
        AttackPower = 15;
        Money = 100;
        Type = TuyulType.CheokYul;
    }

    void ShowMessage(string message)
    {
        DialogueBattle.Instance.UpdateDialog(message);
    }
    public override bool TakeDamage(int damage, Player playerCharacter)
    {
        currentHealth -= damage;

        FindObjectOfType<BattleHUD>().SetHP(currentHealth);
        
        ShowMessage($"{Name} menerima {damage} damage! Sisa HP: {currentHealth}");
        Debug.Log($"{Name} menerima {damage} damage! Sisa HP: {currentHealth}");
        
        // Offer to surrender if health is low
        if (currentHealth > 0 && currentHealth <= maxHealth * 0.3f && !isOfferingMoney)
        {
            ShowMessage($"{Name} menyerang pemain terlebih dahulu sebelum menawarkan deal.");
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
        ShowMessage($"{Name} menawarkan uang sebesar {Money} untuk ganti nyawanya. Terima? (1 = Iya, 2 = Tidak)");
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
            ShowMessage($"{Name} terus memengaruhi critical chance pemain! Ronde tersisa: {DebuffRoundsLeft}");
            Debug.Log($"{Name} terus memengaruhi critical chance pemain! Ronde tersisa: {DebuffRoundsLeft}");
        }

        if (random.NextDouble() < 0.4)
        {
            int stolenAmount = random.Next(1, 20);
            if (playerCharacter.DeductMoney(stolenAmount))
            {
                TuyulAnim.SetTrigger("TPBP");
                ShowMessage($"{Name} menggunakan jurus rahasia: 'Tangan Panjang, Badan Pendek'. Kamu kehilangan uang sebesar {stolenAmount}!");
                Debug.Log($"{Name} menggunakan jurus rahasia: 'Tangan Panjang, Badan Pendek'. Kamu kehilangan uang sebesar {stolenAmount}!");
                yield return new WaitForSeconds(1f);
            }
        }

        // Passive Talent: The Flying Horror
        if (currentHealth <= maxHealth / 2 && !isFlying)
        {
            isFlying = true;
            // tambahin kode buat animasi dia terbang

            TuyulAnim.SetTrigger("Passive");
            ShowMessage($"{Name} masuk ke mode 'The Flying Horror'!");
            Debug.Log($"{Name} masuk ke mode 'The Flying Horror'!");
        }

        if (Random.value < 0.2f) // Special Skill: The Democracy
        {
            yield return StartCoroutine(UseTheDemocracy(playerCharacter));
        }
        else if (Random.value < 0.2f) // Special Skill: Monster Lurks Beneath The Shadow of The Dawn
        {
            yield return StartCoroutine(UsePoison(playerCharacter));
        }
        else
        {
            // basic attack
            NormalAttack(playerCharacter);
        }
    }

    public IEnumerator UseTheDemocracy(Player playerCharacter)
    {
        yield return new WaitForSeconds(1f);

        int roachesCount = Random.Range(2, 9); // Memanggil 2-8 kecoak kecil
        TuyulAnim.SetTrigger("Democracy");
        ShowMessage($"{Name} memanggil {roachesCount} kecoak kecil untuk menyerang!");
        Debug.Log($"{Name} memanggil {roachesCount} kecoak kecil untuk menyerang!");

        for (int i = 0; i < roachesCount; i++)
        {
            int roachDamage = Random.Range(5, 10); // Damage tiap kecoak
            playerCharacter.TakeDamage(roachDamage);
            ShowMessage($"Seekor kecoak menyerang dan memberikan {roachDamage} damage! Sisa HP pemain: {playerCharacter.currentHealth}");
            Debug.Log($"Seekor kecoak menyerang dan memberikan {roachDamage} damage! Sisa HP pemain: {playerCharacter.currentHealth}");
        }
    }

    public IEnumerator UsePoison(Player playerCharacter)
    {
        yield return new WaitForSeconds(1f);
        TuyulAnim.SetTrigger("Monster");
        ShowMessage($"{Name} menggunakan jurus 'Monster Lurks Beneath The Shadow of The Dawn'! Pemain terkena efek poison selama {poisonDuration} giliran.");
        Debug.Log($"{Name} menggunakan jurus 'Monster Lurks Beneath The Shadow of The Dawn'! Pemain terkena efek poison selama {poisonDuration} giliran.");
        playerCharacter.StartCoroutine(ApplyPoison(playerCharacter));
    }

    private IEnumerator ApplyPoison(Player playerCharacter)
    {
        for (int i = 0; i < poisonDuration; i++)
        {
            yield return new WaitForSeconds(1f); // jeda per giliran
            int poisonDamage = 10; // damage per giliran
            playerCharacter.TakeDamage(poisonDamage);
            ShowMessage($"Poison effect: Pemain menerima {poisonDamage} damage. Sisa HP: {playerCharacter.currentHealth}");
            Debug.Log($"Poison effect: Pemain menerima {poisonDamage} damage. Sisa HP: {playerCharacter.currentHealth}");
        }
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
        ShowMessage($"{Name} mengeluarkan jurus 'Ketimpuk Batu' dan memberikan {AttackPower} damage! Sisa HP: {playerCharacter.currentHealth}");
        Debug.Log($"{Name} mengeluarkan jurus 'Ketimpuk Batu' dan memberikan {AttackPower} damage! Sisa HP: {playerCharacter.currentHealth}");
    }
}