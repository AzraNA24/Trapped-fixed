using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RoomManager : MonoBehaviour
{
    public Transform[] respawnPoints; // Posisi respawn untuk setiap Tuyul
    public List<GameObject> tuyulsInRoom;

    void Start()
    {
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null && audio.isPlaying)
        {
            Debug.Log($"RoomManager AudioSource aktif dengan clip: {audio.clip.name}");
        }

        for (int i = 0; i < tuyulsInRoom.Count; i++)
        {
            GameObject tuyul = tuyulsInRoom[i];
            if (PlayerPrefs.GetInt($"{tuyul.name}_Defeated", 0) == 1)
            {
                Destroy(tuyul);

                // Jika Tuyul terakhir dikalahkan, setel posisi respawn
                if (i == tuyulsInRoom.Count - 1)
                {
                    FindObjectOfType<PlayerManager>()?.SetRespawnPosition(respawnPoints[i].position);
                }
            }
        }
    }
}