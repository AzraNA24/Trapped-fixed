using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuyulLewat : MonoBehaviour
{
    public float speed = 5f;

    void Start()
    {
        // Tentukan spawn dari kiri atau kanan layar
        float spawnPositionX = Random.Range(0, 2) == 0 ? -10f : 10f; // Spawn dari kiri (-10) atau kanan (10)
        transform.position = new Vector2(spawnPositionX, Random.Range(-4f, 4f)); // Posisi random di layar
    }

    void Update()
    {
        // Gerakan horizontal
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        // Cek jika sudah keluar layar, balik arah
        if (transform.position.x < -11f || transform.position.x > 11f)
        {
            speed = -speed; // Balik arah jika keluar batas
        }
    }
}
