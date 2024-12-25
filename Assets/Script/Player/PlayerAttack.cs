using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;
    public Transform AttackPoint;
    public float AttackRange = 0.5f;
    public LayerMask Layer;
    public AudioSource hitSound;
    public static string currentTuyulName;
    // Buat Codex
    // public CodexUI codexUI;
    // public int codexIndex;
    public AudioSource newIntro;

    private bool isTriggered = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
            hitSound.Play();
            Debug.Log("Player memukul");
        }
    }

    void Attack()
{
    float direction = transform.localScale.x;
    animator.SetFloat("Horizontal", direction);
    animator.SetTrigger("Attack");

    Collider2D[] hitThing = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, Layer);

    foreach (Collider2D Thing in hitThing)
    {
        string objectName = Thing.gameObject.name;
        string objectLayer = LayerMask.LayerToName(Thing.gameObject.layer);

        Debug.Log($"Detected: {Thing.gameObject.name}");

        if (Thing.isTrigger)
        {
            Debug.Log($"Trigger detected: {Thing.gameObject.name}");
        }

        if (objectLayer == "Tuyul")
        {
            currentTuyulName = Thing.gameObject.name;
            isTriggered = true;

            // if (codexUI == null)
            // {
            //     return;
            // }
            Debug.Log($"Tuyul detected: {currentTuyulName}! Switching to TurnBased scene after audio...");
            StartCoroutine(PlayAudioAndSwitchScene());
            return;
        }

        LootBox lootBox = Thing.GetComponent<LootBox>();
        if (lootBox != null)
        {
            lootBox.GenerateLoot();
        }
    }
}


    IEnumerator PlayAudioAndSwitchScene()
    {
        // if (newIntro != null && !codexUI.tuyulCodex[codexIndex].isUnlocked)
        // {
        //     int index = codexUI.GetTuyulIndexByName(currentTuyulName);
        //     if (index != -1)
        //     {
        //         codexIndex = index;
                newIntro.Play();
        //         codexUI.tuyulCodex[codexIndex].isUnlocked = true;
        //         Debug.Log(codexUI.tuyulCodex[codexIndex].Name + " has been unlocked!");            
        //     }
        //     else
        //     {
        //         Debug.Log("Tuyul tidak ditemukan dalam codex!");
        //     }
            
            yield return new WaitForSeconds(newIntro.clip.length);
        // }

        // Pindah ke scene setelah audio selesai
        SceneManagerController.Instance.SwitchScene("TurnBased", SceneManagerController.GameMode.TurnBased);

        yield break; // ini sementara biar ga error
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(AttackPoint.position, AttackRange);
    }
}
