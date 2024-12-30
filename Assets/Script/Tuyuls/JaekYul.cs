using UnityEngine;
using System.Collections;

public class JaekYul : Tuyul
{
    public int DebuffRoundsLeft = 0;
    private GameObject currentFormObject;

    public JaekYul()
    {
        Name = "Jaek Yul; Tuyul of All Trade, Master of All";
        maxHealth = 350;
        AttackPower = 20;
        Money = 200;
        Type = TuyulType.JaekYul;
    }

    private void Awake()
    {
        currentFormObject = this.gameObject; // Default ke bentuk asli
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
        Debug.Log($"{Name} mengeluarkan jurus 'Ketimpuk Batu' dan memberikan {AttackPower} damage! Sisa HP: {playerCharacter.currentHealth}");
    }

    public IEnumerator UseCurrentFormSpecialSkill(Player playerCharacter)
    {
        yield return new WaitForSeconds(1f);
        Tuyul currentForm = currentFormObject.GetComponent<Tuyul>(); // Ambil komponen Tuyul dari bentuk aktif

        if (currentForm is Aventurine aventurine)
        {
            aventurine.UseTheGreatGatsby(playerCharacter);
        }
        else if (currentForm is MrRizzler rizzler)
        {
            rizzler.UseSeduceYouToDeath(playerCharacter);
        }
        else if (currentForm is CheokYul cheokYul)
        {
            cheokYul.UsePoison(playerCharacter);
        }
        else if (currentForm is ChaengYul chaengYul)
        {
            chaengYul.UseBeyondTheGrave(playerCharacter);
        }
        else
        {
            Debug.Log($"{Name} dalam bentuk {currentForm.Name} tidak memiliki special skill untuk digunakan!");
        }
    }
}