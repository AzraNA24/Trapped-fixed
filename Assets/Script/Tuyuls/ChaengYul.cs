using System.Collections;
using UnityEngine;

public class ChaengYul : Tuyul
{
    public int DebuffRoundsLeft = 0;
    public Animator StoneThrow;
    public Renderer Stone;
    public Animator Heal;
    public Renderer healAttribute;

    void Start()
    {
        Stone.enabled = false;
        healAttribute.enabled = false;
    }

    public ChaengYul()
    {
        Name = "ChaengYul; Bestie of Pocong";
        maxHealth = 200;
        AttackPower = 15;
        Money = 100;
        Type = TuyulType.ChaengYul;
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

        // Passive Skill: Cursed Hop (30% chance)
        if (currentHealth <= 100 && random.NextDouble() < 0.3)
        {
            yield return StartCoroutine(UseCursedHop());
        }

        // Special Skill: Beyond the Grave (20% chance)
        if (random.NextDouble() < 0.2)
        {
            yield return StartCoroutine(UseBeyondTheGrave(playerCharacter));
        }
        else
        {
            NormalAttack(playerCharacter);
        }
    }

    private IEnumerator UseCursedHop()
    {
        int healAmount = Mathf.RoundToInt(maxHealth * 0.15f); // Heal 15% dari max HP
        currentHealth += healAmount;
        TuyulAnim.SetTrigger("CursedHop");
        healAttribute.enabled = true; // Munculkan efek heal

        ShowMessage($"{Name} menggunakan 'Cursed Hop' dan memulihkan {healAmount} HP! Sisa HP: {currentHealth}");
        Debug.Log($"{Name} menggunakan 'Cursed Hop' dan memulihkan {healAmount} HP! Sisa HP: {currentHealth}");

        yield return StartCoroutine(HideEffectAfterAnimation(Heal, healAttribute, "Heal"));
    }

    public IEnumerator UseBeyondTheGrave(Player playerCharacter)
    {
        TuyulAnim.SetTrigger("Behind");
        int Ultimate = AttackPower * 2;
        yield return new WaitForSeconds(2f);

        if (random.NextDouble() < 0.2)
        {
            playerCharacter.TakeDamage(100);
            ShowMessage("ChaengYul menyerang dengan jurus 'Scare You To Death' dan memberikan 100 damage!");
            Debug.Log("Scare You To Death");
        }
        else
        {
            playerCharacter.TakeDamage(Ultimate);
            ShowMessage($"{Name} menggunakan jurus spesial 'Beyond the Grave'! {playerCharacter.Name} menerima {Ultimate} damage! Sisa HP: {playerCharacter.currentHealth}");
            Debug.Log($"{Name} menggunakan jurus spesial 'Beyond the Grave'! {playerCharacter.Name} menerima {Ultimate} damage! Sisa HP: {playerCharacter.currentHealth}");
        }
    }

    public override void NormalAttack (Player playerCharacter)
    {
        playerCharacter.TakeDamage(AttackPower);

        StartCoroutine(ExecuteNormalAttack(playerCharacter));
    }
    public IEnumerator ExecuteNormalAttack(Player playerCharacter)
    {
        Stone.enabled = true;
        TuyulAnim.SetTrigger("OnThrow");

        ShowMessage($"{Name} mengeluarkan jurus 'Ketimpuk Batu' dan memberikan {AttackPower} damage! Sisa HP: {playerCharacter.currentHealth}");
        Debug.Log($"{Name} mengeluarkan jurus 'Ketimpuk Batu' dan memberikan {AttackPower} damage! Sisa HP: {playerCharacter.currentHealth}");

        yield return StartCoroutine(HideEffectAfterAnimation(StoneThrow, Stone, "Bounce"));
    }

    private IEnumerator HideEffectAfterAnimation(Animator animator, Renderer effect, string animationName)
    {
        effect.enabled = true;
        ShowMessage($"{Name} menggunakan efek {effect.name} untuk animasi {animationName}.");
        Debug.Log($"Efek {effect.name} diaktifkan untuk animasi {animationName}.");
        while (!IsAnimationFinished(animator, animationName))
        {
            yield return null;
        }

        // Sembunyikan efek setelah animasi selesai
        effect.enabled = false;
        ShowMessage($"{Name} menyembunyikan efek {effect.name} setelah animasi {animationName} selesai.");
        Debug.Log($"Efek {effect.name} disembunyikan setelah animasi {animationName} selesai.");
    }

    private bool IsAnimationFinished(Animator animator, string animationName)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 2f;
    }
}