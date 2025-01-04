using UnityEngine;

public class PoolTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.currentHealth = player.Health; 
                Debug.Log("Player health restored to max!");
            }
        }
    }
}