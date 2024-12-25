using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public enum PlayerMode { Exploration, TurnBased }
    public PlayerMode currentMode;

    // Referensi prefab
    public GameObject explorationPlayerPrefab;
    public GameObject turnBasedPlayerPrefab;
    public Vector3 respawnPosition;
    public CinemachineVirtualCamera Camera;
    public Vector2 explorationStartPosition;

    private GameObject activePlayer;

    private static PlayerManager instance;

    void Awake()
    {
        // Ensure only one instance of PlayerManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SwitchMode(PlayerMode.Exploration);
    }

    public void SwitchMode(PlayerMode mode)
    {
        if (activePlayer != null)
        {
            Destroy(activePlayer);
        }
        if (mode == PlayerMode.Exploration)
        {
            if (explorationPlayerPrefab != null && activePlayer == null)
            {
                activePlayer = GameObject.FindWithTag("Player");
            }
            if (activePlayer == null)
            {
                activePlayer = Instantiate(explorationPlayerPrefab, transform.position, Quaternion.identity);
            }
        }
        else if (mode == PlayerMode.TurnBased)
        {
            activePlayer = Instantiate(turnBasedPlayerPrefab, transform.position, Quaternion.identity);
        }

        currentMode = mode;

        if (Camera != null)
        {
            Camera.Follow = activePlayer.transform;
        }
    }

    public void SetRespawnPosition(Vector3 position)
    {
        respawnPosition = position;
    }

    public void RespawnPlayer()
    {
        if (activePlayer != null)
        {
            activePlayer.transform.position = respawnPosition;
        }
    }

    public void RestoreLastPosition()
    {
        float x = PlayerPrefs.GetFloat("PlayerLastPosX", transform.position.x);
        float y = PlayerPrefs.GetFloat("PlayerLastPosY", transform.position.y);
        float z = PlayerPrefs.GetFloat("PlayerLastPosZ", transform.position.z);
        activePlayer.transform.position = new Vector3(x, y, z);
    }
    public void SavePlayerPosition(Vector3 position)
    {
        PlayerPrefs.SetFloat("PlayerLastPosX", position.x);
        PlayerPrefs.SetFloat("PlayerLastPosY", position.y);
        PlayerPrefs.SetFloat("PlayerLastPosZ", position.z);
    }

    public void SaveExplorationStartPosition()
    {
        if (activePlayer != null)
        {
            explorationStartPosition = activePlayer.transform.position;
            Debug.Log($"Posisi awal eksplorasi disimpan: {explorationStartPosition}");
        }
    }

    public void RestoreExplorationStartPosition()
    {
        if (activePlayer != null)
        {
            activePlayer.transform.position = explorationStartPosition;
            Debug.Log($"Posisi awal eksplorasi dipulihkan: {explorationStartPosition}");
        }
    }

    public void CheckAndRemoveDefeatedTuyuls(GameObject tuyulObject, string tuyulName)
    {
        if (PlayerPrefs.HasKey($"{tuyulName}_Defeated") && PlayerPrefs.GetInt($"{tuyulName}_Defeated") == 1)
        {
            Destroy(tuyulObject);
            Debug.Log($"{tuyulName} sudah dikalahkan dan dihapus dari scene eksplorasi.");
        }
    }
}
