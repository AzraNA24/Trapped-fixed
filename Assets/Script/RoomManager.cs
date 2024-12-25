using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RoomManager : MonoBehaviour
{
    public Transform[] respawnPoints; // Posisi respawn untuk setiap Tuyul
    public List<GameObject> tuyulsInRoom;

    void Start()
    {
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