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
        // Cari GameObject bernama "SpawnPoint" di scene baru
        GameObject spawnPoint = GameObject.Find("SpawnPoint"); //Nggak jalan, deng. Tapi masih oke hasilnya. Kalau ada waktu, atau kalau mau, benerin aja

        if (spawnPoint != null)
        {
            Debug.Log("SpawnPoint found at position: " + spawnPoint.transform.position);

            // Pindahkan pemain ke posisi SpawnPoint
            Transform playerTransform = GetPlayerTransform();
            if (playerTransform != null)
            {
                playerTransform.position = spawnPoint.transform.position;
            }
        }
        else
        {
            Debug.LogWarning("No SpawnPoint found in scene: " + scene.name);
        }
    }

    private Transform GetPlayerTransform()
    {
        // Cari pemain berdasarkan tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        return player != null ? player.transform : null;
    }
}
