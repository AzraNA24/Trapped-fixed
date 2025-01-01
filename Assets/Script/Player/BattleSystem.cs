using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYER_TURN, TUYUL_TURN, WON, LOST }
public class BattleSystem : MonoBehaviour
{
    public BattleState state;
    public GameObject Player;
    public GameObject Tuyul;
    public GameObject Aventurine;
    public GameObject mrRizzler;
    public GameObject RollyPolly;
    public GameObject ChaengYul;
    public GameObject CheokYul;
    public GameObject JaekYul;
    public Transform playerStation;
    public Transform tuyulStation;
    public Button buttonAnimator;
    private Player playerCharacter;
    private Tuyul enemyCharacter;
    private System.Random random = new System.Random();
    public int maxPotionsPerBattle = 5;
    private int potionCounter;
    public BattleHUD tuyulHUD;
    public AudioClip PotionSound;
    public AudioClip ShortRangeSound;
    public AudioClip LongRangeClickSound;
    public AudioClip LongRangeShootSound;
    public AudioSource SFXSource;
    public AudioSource backgroundMusic;
 


    void Start()
    {
        potionCounter = 0;
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        potionCounter = 0;

        // Cari instance Player yang sudah ada
        playerCharacter = FindObjectOfType<Player>();
        if (playerCharacter == null)
        {
            Debug.LogError("Player tidak ditemukan! Pastikan Player sudah ada di scene.");
            yield break;
        }

        // Reset health player
        playerCharacter.currentHealth = playerCharacter.Health;
        Debug.Log($"Health player di-reset ke {playerCharacter.currentHealth}");

        GameObject selectedTuyulPrefab = null;

        if (PlayerAttack.currentTuyulName == "Aventurine")
        {
            selectedTuyulPrefab = Aventurine;
        }
        else if (PlayerAttack.currentTuyulName == "Mr.Rizzler")
        {
            selectedTuyulPrefab = mrRizzler;
        }
        else if (PlayerAttack.currentTuyulName == "RollyPolly")
        {
            selectedTuyulPrefab = RollyPolly;
        }
        else if (PlayerAttack.currentTuyulName == "ChaengYul")
        {
            selectedTuyulPrefab = ChaengYul;
        }
        else if (PlayerAttack.currentTuyulName == "CheokYul")
        {
            selectedTuyulPrefab = CheokYul;
        }
        else if (PlayerAttack.currentTuyulName == "JaekYul")
        {
            selectedTuyulPrefab = JaekYul;
        }

        // (harusnya) GameObject playerGO = Instantiate(Player, playerStation);
        // (harusnya) playerCharacter = playerGO.GetComponent<Player>();
        // (harusnya) buttonAnimator.animator = playerGO.GetComponent<Animator>();

        GameObject enemyGO = Instantiate(selectedTuyulPrefab, tuyulStation);
        enemyCharacter = enemyGO.GetComponent<Tuyul>();

        tuyulHUD.SetHUDTuyul(enemyCharacter);

        if (enemyCharacter.TuyulAnim != null)
        {
            if (enemyCharacter.TuyulAnim == null)
            {
                Debug.LogError("Animator for the enemy character is missing!");
            }

            if (!enemyCharacter.TuyulAnim.parameters.Any(p => p.name == "TurnBased"))
            {
                Debug.LogError("TurnBased parameter is missing in Animator Controller!");
            }

            enemyCharacter.TuyulAnim.SetBool("TurnBased", true);
            Debug.Log("Done. Animasi Tuyul");
        }
        else
        {
            Debug.Log("Animator pada enemyCharacter tidak ditemukan!");
        }

        ShowMessage($"Pertarungan dimulai! {enemyCharacter.Name} muncul!");
        Debug.Log($"Pertarungan dimulai! {enemyCharacter.Name} muncul!");
        yield return new WaitForSeconds(3f);

        state = BattleState.PLAYER_TURN;
        PlayerTurn();
    }


    //void SetupRollyPollyPair()
    //{
    //    Rolly rolly = Instantiate(Rolly, rollySpawnPoint).GetComponent<Rolly>();
    //    Polly polly = Instantiate(Polly, pollySpawnPoint).GetComponent<Polly>();

    //    rolly.partner = polly;
    //    polly.partner = rolly;
    //}

    void PlayerTurn()
    {
        // Terapkan efek poison hanya untuk CheokYul
        if (enemyCharacter is CheokYul cheokyul && enemyCharacter != null) // Cek apakah Tuyul adalah JaekYul
        {
            // cheokyul.ApplyPoison(playerCharacter); // Jalankan efek poison
        }
        if (enemyCharacter is MrRizzler mrRizzler && mrRizzler.DebuffRoundsLeft == 0 && playerCharacter.criticalChance < 0.3f)
        {
            mrRizzler.RemoveDebuff(playerCharacter);
        }
        if (!enemyCharacter.isOfferingMoney)
        {
        ShowMessage("Pilih tindakan: Serang Jarak Pendek, Jarak Jauh, atau Potion");
        }
    }

    public void OnShortRangeAttackButton()
    {
        if (state != BattleState.PLAYER_TURN)
            return;
        StartCoroutine(ShortRangeAttack());

        ShowMessage("Penyerangan selesai dilakukan");
        Debug.Log("Penyerangan selesai dilakukan");

    }

    IEnumerator ShortRangeAttack()
    {
        if (ShortRangeSound != null && SFXSource != null)
        {
            SFXSource.PlayOneShot(ShortRangeSound);
        }

        yield return new WaitUntil(() => !buttonAnimator.animator.GetCurrentAnimatorStateInfo(0).IsName("ShortRange"));
        int AttackPower = Mathf.CeilToInt(playerCharacter.Money * 0.09f);
        bool isCritical = random.NextDouble() < playerCharacter.criticalChance;
        int finalAttackPower = isCritical ? AttackPower * 2 : AttackPower;

        if (isCritical)
        {
            ShowMessage("Serangan Critical! Kamu menyerang musuh dengan serangan jarak dekat sebesar " + finalAttackPower);
            Debug.Log($"Serangan Critical! Kamu menyerang musuh dengan serangan jarak dekat sebesar {finalAttackPower}!");
        }
        else
        {
            ShowMessage("Kamu menyerang musuh dengan serangan jarak dekat sebesar " + finalAttackPower);
            Debug.Log($"Kamu menyerang musuh dengan serangan jarak dekat sebesar {finalAttackPower}!");
            yield return new WaitForSeconds(1f);
        }

        bool isDead = enemyCharacter.TakeDamage(finalAttackPower, playerCharacter);

        if (enemyCharacter is Aventurine aventurine)
        {
            aventurine.FUA(playerCharacter, isCritical);
        }

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.TUYUL_TURN;
            StartCoroutine(EnemyTurn());
        }
    }

    public void OnLongRangeAttackButton()
    {
        if (state != BattleState.PLAYER_TURN)
            return;

        if (!playerCharacter.HasBullets())
        {
            ShowMessage("Peluru tidak cukup untuk serangan jarak jauh!");
            Debug.Log("Peluru tidak cukup. Tetap di giliran PLAYER_TURN.");
            return; // Tetap di PLAYER_TURN
        }

        StartCoroutine(LongRangeAttack());
    }

    IEnumerator LongRangeAttack()
    {
        if (!playerCharacter.HasBullets())
        {
            ShowMessage("Peluru tidak cukup untuk serangan jarak jauh!");
            Debug.Log("Peluru tidak cukup untuk serangan jarak jauh!");
            yield break;
        }

        playerCharacter.Inventory.UseItem(new LootBox { Type = LootBox.LootType.Bullet }, 1); // Kurangi peluru
        Debug.Log($"Peluru digunakan. Sisa peluru: {playerCharacter.Inventory.GetItemCount(LootBox.LootType.Bullet)}");

        int AttackPower = Mathf.CeilToInt(playerCharacter.Money * 0.15f);
        bool isCritical = random.NextDouble() < playerCharacter.criticalChance + 0.1f;
        int finalAttackPower = isCritical ? AttackPower * 2 : AttackPower;

        if (random.NextDouble() < 0.3)
        {
            ShowMessage("Kamu meleset!");
            Debug.Log($"You Missed!");
            state = BattleState.TUYUL_TURN;
            StartCoroutine(EnemyTurn());
        }
        else
        {

            if (isCritical)
            {
                ShowMessage("Serangan Critical! Kamu menyerang musuh dengan serangan jarak dekat sebesar " + finalAttackPower);
                Debug.Log($"Serangan Critical! Kamu menyerang musuh dengan serangan jarak dekat sebesar {finalAttackPower}!");
            }
            else
            {
                ShowMessage("Kamu menyerang musuh dengan serangan jarak dekat sebesar " + finalAttackPower);
                Debug.Log($"Kamu menyerang musuh dengan serangan jarak dekat sebesar {finalAttackPower}!");
            }
            
            bool isDead = enemyCharacter.TakeDamage(finalAttackPower, playerCharacter);

            if (enemyCharacter is Aventurine aventurine)
            {
                aventurine.FUA(playerCharacter, isCritical);
            }

            yield return new WaitForSeconds(2f);

            if (isDead)
            {
                state = BattleState.WON;
                EndBattle();
            }
            else
            {
                state = BattleState.TUYUL_TURN;
                StartCoroutine(EnemyTurn());
            }
        }
    }

    IEnumerator EnemyTurn()
    {
        // ShowMessage($"{enemyCharacter.Name} sedang menyerang!");
        Debug.Log($"{enemyCharacter.Name} sedang menyerang!");
        enemyCharacter.TuyulAnim.SetBool("TurnBased", true);

        yield return new WaitForSeconds(1f);

        enemyCharacter.EnemyAction(playerCharacter);

        yield return new WaitForSeconds(2f);

        if (playerCharacter.currentHealth <= 0)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYER_TURN;
            PlayerTurn();
        }
    }


    public void EndBattle()
    {
        if (state == BattleState.WON)
        {
            
            playerCharacter.AddMoney(enemyCharacter.Money);

            PlayerPrefs.SetInt($"{enemyCharacter.Name}_Defeated", 1);
            PlayerPrefs.Save();

            if (enemyCharacter != null)
            {
                Destroy(enemyCharacter.gameObject);
                ShowMessage($"{enemyCharacter.Name} telah dihancurkan.");
                Debug.Log($"{enemyCharacter.Name} telah dihancurkan.");
            }
            
            // Cek apakah musuh adalah JaekYul
            if (enemyCharacter.Name == "JaekYul") 
            {
                string randomWinningMessage = WinningMessage.GetRandomWinningMessage();
                PlayerPrefs.SetString("WinningMessage", randomWinningMessage);
                PlayerPrefs.Save();
        
                Debug.Log(randomWinningMessage);
                SceneManager.LoadScene("WinningScene");
            }
            else
            {
                Debug.Log("Kembali ke mode eksplorasi setelah memenangkan pertarungan.");
                SceneManagerController.Instance.ReturnToLastScene();
                FindObjectOfType<PlayerManager>()?.RestoreExplorationStartPosition();
            }
        
        }
        else if (state == BattleState.LOST)
        {
            string gameOverMessage = GameOverMessage.GetRandomGameOverMessage(enemyCharacter.Name);
            PlayerPrefs.SetString("GameOverMessage", gameOverMessage);
            PlayerPrefs.Save();

            SceneManager.LoadScene("GameOver");
        }
    }

    public void OnPotionButton()
    {
        if (state != BattleState.PLAYER_TURN)
            return;

        if (potionCounter >= maxPotionsPerBattle)
        {
            ShowMessage("Kamu telah menggunakan semua potion yang tersedia untuk pertempuran ini, player kembung!");
            Debug.Log("Kamu telah menggunakan semua potion yang tersedia untuk pertempuran ini, player kembung!");
            //matiin animasinya
            return;
        }

        if (playerCharacter.Inventory.GetItemCount(LootBox.LootType.HealthPotion) > 0)
        {
            potionCounter++;
            playerCharacter.UsePotion(); // Menggunakan potion dan mengurangi dari inventory
            ShowMessage($"Potion digunakan. Health sekarang: {playerCharacter.currentHealth}");
            Debug.Log($"Potion digunakan. Health sekarang: {playerCharacter.currentHealth}");
        
            state = BattleState.TUYUL_TURN;
            StartCoroutine(EnemyTurn());    
        }
        else
        {
            //matiin animasinya
            ShowMessage("Tidak ada potion yang tersedia!");
            Debug.Log("Tidak ada potion yang tersedia!");
        }  
    }

    IEnumerator UsePotion()
    {
        potionCounter++;
        playerCharacter.UsePotion();

        ShowMessage($"Potion digunakan {potionCounter}/{maxPotionsPerBattle} kali dalam pertempuran ini.");
        Debug.Log($"Potion digunakan {potionCounter}/{maxPotionsPerBattle} kali dalam pertempuran ini.");
        yield return new WaitForSeconds(1f);

        state = BattleState.TUYUL_TURN;
        StartCoroutine(EnemyTurn());
    }

    void ShowMessage(string message)
    {
        DialogueBattle.Instance.UpdateDialog(message);
    }
}