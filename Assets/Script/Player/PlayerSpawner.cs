using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
    private static PlayerSpawner instance;

    private void Awake()
    {
        // Singleton untuk mempertahankan objek antar scene
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Berlangganan event saat scene dimuat
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Dapatkan mode saat ini dari SceneManagerController
        SceneManagerController.GameMode currentMode = SceneManagerController.Instance != null 
            ? SceneManagerController.Instance.GetCurrentGameMode() 
            : SceneManagerController.GameMode.Exploration; // Default ke Exploration jika null

        // Pilih SpawnPoint berdasarkan mode
        string spawnPointName = currentMode == SceneManagerController.GameMode.Exploration ? "SpawnPoint" : "PlayerSpawnPoint";

        // Cari GameObject SpawnPoint di scene baru
        GameObject spawnPoint = GameObject.Find(spawnPointName);

        if (spawnPoint != null)
        {
            Debug.Log($"{spawnPointName} found at position: " + spawnPoint.transform.position);

            // Pindahkan pemain ke posisi SpawnPoint
            Transform playerTransform = GetPlayerTransform();
            if (playerTransform != null)
            {
                playerTransform.position = spawnPoint.transform.position;
            }
        }
        else
        {
            Debug.LogWarning($"No {spawnPointName} found in scene: " + scene.name);
        }
    }

    private Transform GetPlayerTransform()
    {
        // Cari pemain berdasarkan tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        return player != null ? player.transform : null;
    }
}
