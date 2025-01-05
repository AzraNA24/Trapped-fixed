using System.Collections;
using UnityEngine;

public class Chef : Tuyul
{
    public AudioSource audioSource; 
    public AudioClip ketimpukSound;
    public int DebuffRoundsLeft = 0;

    public Chef()
    {
        Name = "Chef";
        maxHealth = 50;
        AttackPower = 20;
        Money = 30;
        Type = TuyulType.Chef;
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

        NormalAttack(playerCharacter);
    }

    public override void NormalAttack(Player playerCharacter)
    {
        StartCoroutine(ExecuteNormalAttack(playerCharacter));
    }

    private IEnumerator ExecuteNormalAttack(Player playerCharacter)
    {
        TuyulAnim.SetTrigger("Throws");
        yield return new WaitForSeconds(0.8f);
        audioSource.PlayOneShot(ketimpukSound);
        yield return new WaitForSeconds(1f);

        playerCharacter.TakeDamage(AttackPower);
        ShowMessage($"{Name} mengeluarkan jurus 'Ketimpuk Batu' dan memberikan {AttackPower} damage! Sisa HP: {playerCharacter.currentHealth}");
        Debug.Log($"{Name} mengeluarkan jurus 'Ketimpuk Batu' dan memberikan {AttackPower} damage! Sisa HP: {playerCharacter.currentHealth}");
    }
}