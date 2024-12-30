using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tuyul : MonoBehaviour
{
    public Animator TuyulAnim;
    public string Name = "Tuyul; Scurry Impish Little Trickster";
    public int maxHealth;
    public int currentHealth;
    public int AttackPower;
    public int Money;
    public bool isOfferingMoney = false;
    public System.Random random = new System.Random();
    public TuyulType Type { get; set; }

    void Start()
    {
        currentHealth = maxHealth;

        if (TuyulAnim == null)
        {
            TuyulAnim = GetComponent<Animator>(); // Ambil komponen Animator
            if (TuyulAnim == null)
            {
                Debug.LogError($"{name} tidak memiliki komponen Animator!");
            }
            TuyulAnim.SetBool("TurnBased", false);
        }
    }

    // Method to handle taking damage with additional Tuyul abilities
    public virtual bool TakeDamage(int damage, Player playerCharacter)
    {
        currentHealth -= damage;

        FindObjectOfType<BattleHUD>().SetHP(currentHealth);
        
        Debug.Log($"{Name} menerima {damage} damage! Sisa HP: {currentHealth}");

        // Offer to surrender if health is low
        if (currentHealth > 0 && currentHealth <= maxHealth * 0.3f && !isOfferingMoney)
        {
            Debug.Log($"{Name} menyerang pemain terlebih dahulu sebelum menawarkan deal.");
    
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

    public virtual IEnumerator OfferDeal(Player playerCharacter)
    {
        yield return new WaitForSeconds(1f); // Jeda untuk memastikan serangan selesai
        isOfferingMoney = true;
        Debug.Log($"{Name} menawarkan uang sebesar {Money} untuk ganti nyawanya. Terima? (1 = Iya, 2 = Tidak)");
        yield return StartCoroutine(WaitForPlayerChoice(playerCharacter)); // Tunggu input pemain
    }

    public IEnumerator WaitForPlayerChoice(Player playerCharacter)
    {
        Debug.Log("Menunggu input pemain...");

        bool decisionMade = false;
        while (!decisionMade)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) // Pemain memilih "Terima"
            {
                Debug.Log($"{Name} melarikan diri setelah memberikan uang sebesar {Money}!");
                playerCharacter.AddMoney(Money);
                currentHealth = 0; // Tuyul mati
                decisionMade = true;

                // Akhiri pertarungan
                EndBattleEarly();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2)) // Pemain memilih "Tolak"
            {
                Debug.Log($"{Name} terus melawan!");
                decisionMade = true;
            }

            yield return null; // Tunggu frame berikutnya
        }

        isOfferingMoney = false; // Reset flag
    }

    private void EndBattleEarly()
    {
        Debug.Log("Pertarungan diakhiri karena pemain menerima tawaran Tuyul.");
        SceneManagerController.Instance.ReturnToLastScene();
    }

    public virtual void EnemyAction(Player playerCharacter)
    {
        NormalAttack(playerCharacter);
    }

    public virtual void NormalAttack(Player playerCharacter)
    {
        playerCharacter.TakeDamage(AttackPower);
        TuyulAnim.SetTrigger("Throws");
        Debug.Log($"{Name} mengeluarkan jurus 'Ketimpuk Batu' dan memberikan {AttackPower} damage! Sisa HP: {playerCharacter.currentHealth}");
    }

    public enum TuyulType
    {
        Aventurine,
        MrRizzler,
        RollyPolly,
        ChaengYul,
        CheokYul,
        JaekYul
    }
}