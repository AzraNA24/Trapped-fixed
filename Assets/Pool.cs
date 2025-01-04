using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Triggered by: {other.name}");
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log("Player detected.");
            player.currentHealth = player.Health;
            Debug.Log($"Player health restored to full. Current health: {player.currentHealth}");
        }
    }
}
