using UnityEngine;
using System.Collections;

public class JaekYul : Tuyul
{
    public int DebuffRoundsLeft = 0;
    private GameObject currentFormObject;
    private int poisonDuration = 3;

    public JaekYul()
    {
        Name = "Jaek Yul; Tuyul of All Trade, Master of All";
        maxHealth = 350;
        AttackPower = 20;
        Money = 200;
        Type = TuyulType.JaekYul;
    }

    void ShowMessage(string message)
    {
        DialogueBattle.Instance.UpdateDialog(message);
    }

    private void Awake()
    {
        currentFormObject = this.gameObject; // Default ke bentuk asli
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

        // Special skill: dapat berubah menjadi Tuyul lain
        if (Random.value < 0.3f) // 30% chance
        {
            TransformToRandomTuyul();
            yield return StartCoroutine(UseCurrentFormSpecialSkill(playerCharacter));
        }
        else
        {
            // basic attack
            NormalAttack(playerCharacter);
        }

        if (currentFormObject != null && currentFormObject != this.gameObject)
        {
            currentFormObject = this.gameObject; // Set ke bentuk asli
            ShowMessage($"{Name} kembali ke bentuk aslinya setelah menyerang!");
            Debug.Log($"{Name} kembali ke bentuk aslinya setelah menyerang!");
        }
    }

    private void TransformToRandomTuyul()
    {
        System.Type[] tuyulTypes = { typeof(Aventurine), typeof(CheokYul), typeof(ChaengYul), typeof(MrRizzler) };
        System.Type randomTuyulType = tuyulTypes[Random.Range(0, tuyulTypes.Length)];

        // Hapus GameObject bentuk lama jika bukan JaekYul asli
        if (currentFormObject != null && currentFormObject != this.gameObject)
        {
            Destroy(currentFormObject); 
        }

        // Buat GameObject baru untuk bentuk baru
        currentFormObject = new GameObject(randomTuyulType.Name); // Buat GameObject baru
        var newTuyul = currentFormObject.AddComponent(randomTuyulType) as Tuyul;

        // Menambahkan komponen Animator
        var animator = currentFormObject.GetComponent<Animator>();
        if (animator == null)
        {
            animator = currentFormObject.AddComponent<Animator>(); // Tambahkan Animator baru jika belum ada
            Debug.LogWarning($"Animator tidak ditemukan pada {currentFormObject.name}, Animator baru ditambahkan.");
        }

        newTuyul.TuyulAnim = animator; // Hubungkan Animator ke Tuyul baru

        ShowMessage($"{Name} berubah menjadi {randomTuyulType.Name}!");
        Debug.Log($"{Name} berubah menjadi {randomTuyulType.Name}!");
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

    public IEnumerator UseCurrentFormSpecialSkill(Player playerCharacter)
    {
        yield return new WaitForSeconds(1f);
        Tuyul currentForm = currentFormObject.GetComponent<Tuyul>(); // Ambil komponen Tuyul dari bentuk aktif

        if (currentForm is Aventurine aventurine)
        {
            yield return StartCoroutine(UseTheGreatGatsby(playerCharacter));
        }
        else if (currentForm is MrRizzler rizzler)
        {
            yield return StartCoroutine(UseSeduceYouToDeath(playerCharacter));
        }
        else if (currentForm is CheokYul cheokYul)
        {
            if (Random.value < 0.2f) // Special Skill: The Democracy
            {
                yield return StartCoroutine(UseTheDemocracy(playerCharacter));
            }
            else // Special Skill: Monster Lurks Beneath The Shadow of The Dawn
            {
                yield return StartCoroutine(UsePoison(playerCharacter));
            }
        }
        else
        {
            ShowMessage($"{Name} dalam bentuk {currentForm.Name} tidak memiliki special skill untuk digunakan!");
            Debug.Log($"{Name} dalam bentuk {currentForm.Name} tidak memiliki special skill untuk digunakan!");
        }
    }
    public IEnumerator UseTheGreatGatsby(Player playerCharacter)
    {
        TuyulAnim.SetTrigger("Aven");
        int Ultimate = AttackPower + AttackPower / 2;
        yield return new WaitForSeconds(1f);

        playerCharacter.TakeDamage(Ultimate);
        ShowMessage($"{Name} memberikan {Ultimate} damage tambahan dengan jurus 'The Great Gatsby'! Sisa HP: {playerCharacter.currentHealth}");
        Debug.Log($"{Name} memberikan {AttackPower * 1.5} damage tambahan dengan jurus 'The Great Gatsby'! Sisa HP: {playerCharacter.currentHealth}");
    }
    public IEnumerator UseSeduceYouToDeath(Player playerCharacter)
    {
        TuyulAnim.SetTrigger("Rizz");
        yield return new WaitForSeconds(1f);

        DebuffRoundsLeft = 3;
        // Mengurangi critical chance pemain (10%)
        playerCharacter.criticalChance -= 0.1f;
        if (playerCharacter.criticalChance < 0) playerCharacter.criticalChance = 0;

        // Mengurangi efektivitas health potion (50%)
        playerCharacter.healthPotionEffectiveness -= 0.5f;
        if (playerCharacter.healthPotionEffectiveness < 0.1f) playerCharacter.healthPotionEffectiveness = 0.1f;

        ShowMessage($"{Name} menggunakan jurus spesial 'Seduce You To Death'! Efek health potion pemain berkurang dan critical chance turun menjadi {playerCharacter.criticalChance * 100}%.");
        Debug.Log($"{Name} menggunakan jurus spesial 'Seduce You To Death'! Efek health potion pemain berkurang dan critical chance turun menjadi {playerCharacter.criticalChance * 100}%.");
    }
    public IEnumerator UseTheDemocracy(Player playerCharacter)
    {
        yield return new WaitForSeconds(1f);

        int roachesCount = Random.Range(2, 9); // Memanggil 2-8 kecoak kecil
        TuyulAnim.SetTrigger("CkyLD");
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
        TuyulAnim.SetTrigger("CkyLS");
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
}