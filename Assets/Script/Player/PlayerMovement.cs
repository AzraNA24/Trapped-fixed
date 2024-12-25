using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    public AudioSource footstepAudio;

    private Vector2 movement;
    private float idleTimer = 0f; // Tracks time the player is stationary
    private float idleThreshold = 5f; // Time in seconds before switching to idle animation
    // private bool isAttacking = false;
    private Vector2 lastMovement;
    private bool isPlayingFootstep = false;
    
    void Update()
    {
    movement.x = Input.GetAxisRaw("Horizontal");
    movement.y = Input.GetAxisRaw("Vertical");

    if (movement.sqrMagnitude > 0)
    {
        lastMovement = movement; // Dia harus nyimpen arah terakhir, kalau enggak, default ke kiri
    }

    animator.SetFloat("Horizontal", lastMovement.x);
    animator.SetFloat("Vertical", lastMovement.y);      

    bool isMoving = movement.sqrMagnitude > 0;                                                                                                           
    animator.SetBool("isMoving", isMoving);

    if (isMoving)
    {
        idleTimer = 0f;
        PlayFootstepSound();
    }
    else
    {
        StopFootstepSound();
        idleTimer += Time.deltaTime;
    }

    if (idleTimer > idleThreshold)
    {
        animator.SetBool("isIdle", true);
    }
    else
    {
        animator.SetBool("isIdle", false);
    }

    // if (Input.GetMouseButtonDown(0) && !isAttacking)
    // {
    //     animator.SetFloat("Horizontal", movement.x);
    //     animator.SetFloat("Vertical", movement.y);
    //     // StartCoroutine(PerformAttack());
    // }

    }

    void FixedUpdate()
    {
        // Move the player using Rigidbody2D
        // if (!isAttacking) // Prevent movement during attack
        // {
            Vector2 newPosition = rb.position + movement * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);
        // }
    }

//     private IEnumerator PerformAttack()
// {
//     isAttacking = true;
//     float attackDirectionX = movement.x;
//     float attackDirectionY = movement.y;

//     // Pastikan nilai Horizontal/Vertical sesuai arah terakhir, atau default ke kanan/kiri
//     if (movement.sqrMagnitude == 0) // Jika pemain diam, gunakan nilai default
//     {
//         attackDirectionX = animator.GetFloat("Horizontal");
//         attackDirectionY = animator.GetFloat("Vertical");
//     }

//     animator.SetFloat("Horizontal", attackDirectionX);
//     animator.SetFloat("Vertical", attackDirectionY);

//     animator.SetBool("isClick", true);

//     yield return new WaitForSeconds(0.5f);

//     // Kembalikan isClick ke false
//     animator.SetBool("isClick", false);
//     isAttacking = false;
// }

    void PlayFootstepSound()
    {
        if (!isPlayingFootstep)
        {
            footstepAudio.loop = true; // Agar terus bermain selama berjalan
            footstepAudio.Play();
            // Debug.Log("Playing Footstep Sound");
            isPlayingFootstep = true;
        }
    }

    void StopFootstepSound()
    {
        if (isPlayingFootstep)
        {
            // Debug.Log("Stopping Footstep Sound");
            footstepAudio.Stop();
            isPlayingFootstep = false;
        }
    }
}
