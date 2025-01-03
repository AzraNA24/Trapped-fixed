using System.Collections;
using UnityEngine;

public class RollyPolly : Tuyul
{
    public int DebuffRoundsLeft = 0;
    public GameObject Rolly;
    private Animator rollyAnimator;

    void Awake()
    {
        if (Rolly != null)
        {
            Rolly.SetActive(false); // Sembunyikan Rolly di awal
            rollyAnimator = Rolly.GetComponent<Animator>(); // Ambil Animator Rolly
        }
    }

    public RollyPolly()
    {
        Name = "RollyPolly";
        maxHealth = 50;
        AttackPower = 10;
        Money = 30;
        Type = TuyulType.RollyPolly;
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

        if (random.NextDouble() < 0.4)
        {
            yield return StartCoroutine(UseTeamworkisDreamwork(playerCharacter));
        }
        else
        {
            yield return StartCoroutine(ExecuteNormalAttack(playerCharacter));
        }
    }

    public IEnumerator UseTeamworkisDreamwork(Player playerCharacter)
    {
        if (Rolly == null || rollyAnimator == null)
        {
            Debug.LogWarning("Rolly atau Animator tidak ditemukan!");
            yield break;
        }

        // Tampilkan Rolly dan jalankan animasi "Go RollyPolly"
        Rolly.SetActive(true);
        TuyulAnim.SetTrigger("Yippie");
        ShowMessage($"{Name} memanggil Rolly untuk menyerang!");

        yield return new WaitForSeconds(rollyAnimator.GetCurrentAnimatorStateInfo(0).length); // Tunggu animasi selesai

        // Jalankan animasi "Ulti"
        rollyAnimator.SetTrigger("Ulti");
        ShowMessage("Rolly mengeluarkan jurus : The Power of Imagination!");
        yield return new WaitForSeconds(rollyAnimator.GetCurrentAnimatorStateInfo(0).length); // Tunggu animasi selesai
        playerCharacter.TakeDamage(2*AttackPower);
        Rolly.SetActive(false);
    }
    

    public override void NormalAttack(Player playerCharacter)
    {
        StartCoroutine(ExecuteNormalAttack(playerCharacter));
    }

    private IEnumerator ExecuteNormalAttack(Player playerCharacter)
    {
        playerCharacter.TakeDamage(AttackPower);
        TuyulAnim.SetTrigger("Throws");
        ShowMessage($"{Name} mengeluarkan jurus 'Ketimpuk Batu' dan memberikan {AttackPower} damage! Sisa HP: {playerCharacter.currentHealth}");
        Debug.Log($"{Name} mengeluarkan jurus 'Ketimpuk Batu' dan memberikan {AttackPower} damage! Sisa HP: {playerCharacter.currentHealth}");

        yield return new WaitForSeconds(1f); // Jeda untuk animasi
    }
}
