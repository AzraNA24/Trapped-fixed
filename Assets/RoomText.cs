using UnityEngine;
using TMPro;

public class RoomText : MonoBehaviour
{
    public static RoomText Instance { get; private set; }
    public TextMeshProUGUI roomCountText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        roomCountText.text = "Room: Start Room";
    }

    public void UpdateRoomCount(int count)
    {
        if (roomCountText != null)
        {
            roomCountText.text = "Room: " + count;
        }
    }
}