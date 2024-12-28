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
    if (mode == PlayerMode.TurnBased)
    {
        PlayerMovement movement = GetComponent<PlayerMovement>();
        PlayerAttack attack = GetComponent<PlayerAttack>();

        if (movement != null) movement.enabled = false;
        if (attack != null) attack.enabled = false;

        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("TurnBased", true);
        }
    }
    else if (mode == PlayerMode.Exploration)
    {
        PlayerMovement movement = GetComponent<PlayerMovement>();
        PlayerAttack attack = GetComponent<PlayerAttack>();

        if (movement != null) movement.enabled = true;
        if (attack != null) attack.enabled = true;

        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.ResetTrigger("TurnBased");
        }
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
