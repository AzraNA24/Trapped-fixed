using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerController : MonoBehaviour
{
    public static SceneManagerController Instance;

    public enum GameMode { Exploration, TurnBased }
    public string lastSceneName;
    private GameMode currentMode;

    public GameObject playerController;
    private GameObject playerControllerInstance;

    private Vector2 playerPosition; // Menyimpan posisi Player terakhir

    public AudioClip explorationMusic;
    public AudioClip turnBasedMusic;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Instantiate Player Controller di awal permainan
        playerControllerInstance = Instantiate(playerController);
        currentMode = GameMode.Exploration;

        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayMusic(explorationMusic);
        }
    }

    public void SwitchScene(string sceneName, GameMode mode)
    {
        if (mode == GameMode.TurnBased)
        {
            // Simpan posisi eksplorasi sebelum masuk Turn-Based
            FindObjectOfType<PlayerManager>()?.SaveExplorationStartPosition();

            // Set posisi spesifik pemain di mode Turn-Based
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                player.transform.position = new Vector2(-2, 2);
                PlayerMovement movement = player.GetComponent<PlayerMovement>();
                PlayerAttack attack = player.GetComponent<PlayerAttack>();

                if (movement != null) movement.enabled = false; // Matikan PlayerMovement
                if (attack != null) attack.enabled = false;     // Matikan PlayerAttack

                Animator animator = player.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.SetBool("TurnBased", true); // Aktifkan animasi Turn-Based
                }
            }

            // Play Turn-Based music
            if (AudioManager.instance != null)
            {
                AudioManager.instance.StopMusic();
                AudioManager.instance.DebugActiveAudioSources(); 
                AudioManager.instance.PlayMusic(turnBasedMusic);
            }

        }
        else if (mode == GameMode.Exploration)
        {
            // Ganti musik kembali ke eksplorasi
            GameObject player = GameObject.FindWithTag("Player");
            Animator animator = player.GetComponent<Animator>();
            animator.SetBool("TurnBased", false);
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayMusic(explorationMusic);
            }
        }

        currentMode = mode;

        if (mode == GameMode.TurnBased)
        {
            lastSceneName = SceneManager.GetActiveScene().name;
        }
        SceneManager.LoadScene(sceneName);
    }
    public GameMode GetCurrentGameMode()
    {
        return currentMode;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        // Reposition player setelah scene baru dimuat
        RepositionPlayer();
        PlayerManager prefabController = playerControllerInstance.GetComponent<PlayerManager>();
        if (prefabController != null)
        {
            if (currentMode == GameMode.Exploration)
                prefabController.SwitchMode(PlayerManager.PlayerMode.Exploration);
            else
                prefabController.SwitchMode(PlayerManager.PlayerMode.TurnBased);
        }
    }

    private void SavePlayerPosition()
    {
        PlayerManager playerManager = FindObjectOfType<PlayerManager>();
        GameObject activePlayer = GameObject.FindWithTag("Player");

        if (playerManager != null && activePlayer != null)
        {
            playerPosition = activePlayer.transform.position; 
            playerManager.SavePlayerPosition(playerPosition); 
            Debug.Log($"Posisi pemain disimpan: {playerPosition}");
        }
    }

    private void RepositionPlayer()
    {
        // Reposisi Player di scene baru
        GameObject activePlayer = GameObject.FindWithTag("Player");
        if (activePlayer != null)
        {
            activePlayer.transform.position = playerPosition;
            Debug.Log($"Posisi pemain dipulihkan: {playerPosition}");
        }
    }
    
    public void ReturnToLastScene()
    {
        if (!string.IsNullOrEmpty(lastSceneName))
        {
            SceneManager.LoadScene(lastSceneName);
            SceneManager.sceneLoaded += (scene, mode) =>
            {
                // Pulihkan posisi Player
                FindObjectOfType<PlayerManager>()?.RestoreExplorationStartPosition();

                if (AudioManager.instance != null)
                {
                    AudioManager.instance.StopMusic();
                    AudioManager.instance.PlayMusic(explorationMusic);
                }

                // Hapus Tuyul yang sudah dikalahkan
                GameObject[] tuyuls = GameObject.FindGameObjectsWithTag("Tuyul");
                foreach (GameObject tuyul in tuyuls)
                {
                    string tuyulName = tuyul.name;
                    FindObjectOfType<PlayerManager>()?.CheckAndRemoveDefeatedTuyuls(tuyul, tuyulName);
                }
            };
        }
        else
        {
            Debug.LogWarning("Last scene name is empty!");
        }
    }

    public void StartNewGame()
    {
        Player.Instance.ResetInventory();
        Player.Instance.currentHealth = Player.Instance.Health; 
        Player.Instance.Money = 100; 
        SceneManager.LoadScene("MainMenu"); 
    }
}