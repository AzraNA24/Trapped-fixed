using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }
    private CinemachineVirtualCamera cinemachineCam;

    // private void Awake()
    // {
    //     if (Instance == null)
    //     {
    //         Instance = this;
    //         DontDestroyOnLoad(gameObject);
    //     }
    //     else
    //     {
    //         Destroy(gameObject);
    //         return;
    //     }
    // } 
    private void Start()
    {
        // Temukan Cinemachine Virtual Camera di scene
        cinemachineCam = FindObjectOfType<CinemachineVirtualCamera>();

        // Pastikan Player.Instance sudah diatur sebelum mengaksesnya
        if (cinemachineCam != null && Player.Instance != null)
        {
            // Set target Follow ke Player Instance
            cinemachineCam.Follow = Player.Instance.transform;
        }
        else
        {
            Debug.LogError("Cinemachine Camera or Player instance not found.");
        }
    }
}
