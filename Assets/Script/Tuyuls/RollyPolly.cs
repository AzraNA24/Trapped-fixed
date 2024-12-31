using System.Collections;
using UnityEngine;

public class RollyPolly : Tuyul    //jujur masih blm terlalu ngerti yang sepasang tuyul ini
{
    public int DebuffRoundsLeft = 0;
    public RollyPolly partner; // Referensi ke pasangan

    public bool HasPartnerAlive => partner != null && partner.currentHealth > 0;

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
            Debug.Log($"{Name} menyerang pemain terlebih dahulu sebelum menawarkan deal.");
            StartCoroutine(ExecuteNormalAttack(playerCharacter));

            // Setelah serangan selesai, mulai tawaran
            StartCoroutine(OfferDeal(playerCharacter));
            return false; 
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            return true;
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

        // Special Skill: Teamwork is Dreamwork (20%)
        if (random.NextDouble() < 0.2)
        {
            yield return StartCoroutine(UseTeamworkSkill(playerCharacter)); 
        }
        else
        {
            // basic attack
            NormalAttack(playerCharacter);
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
        Debug.Log($"{Name} mengeluarkan jurus 'Ketimpuk Batu' dan memberikan {AttackPower} damage! Sisa HP: {playerCharacter.currentHealth}");
    }

    public virtual IEnumerator UseTeamworkSkill(Player playerCharacter)
    {
        yield return new WaitForSeconds(1f);
        
        if (HasPartnerAlive)   
        {
            int teamworkDamage = Mathf.RoundToInt(AttackPower) + Mathf.RoundToInt(partner.AttackPower);
            playerCharacter.TakeDamage(teamworkDamage);
            Debug.Log($"{Name} dan {partner.Name} menggunakan 'Teamwork is Dreamwork'! Pemain menerima {teamworkDamage} damage!");

            // animasi rolly polly nyerang berdua (?)
        }
        else
        {
            Debug.Log($"{Name} tidak dapat menggunakan 'Teamwork is Dreamwork' karena partner telah mati!");
        }
    }
    
    public virtual int GetAttackPower()
    {
        return AttackPower; // Default attack power
    }
}