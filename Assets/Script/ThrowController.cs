// using UnityEngine;

// public class ThrowController : MonoBehaviour
// {
//     public Animator StoneThrow;
//     public Renderer Stone;
//     private Animator CheongYul;


//     void Start()
//     {
        
//         CheongYul = GetComponent<Animator>();
        
//         if (CheongYul != null)
//         {
//             Debug.Log("CheongYul ditemukan.");
//         }
//         else
//         {
//             Debug.LogError("CheongYul TIDAK ditemukan!");
//         }

//         if (StoneThrow != null)
//         {
//             Debug.Log("StoneThrow ditemukan.");
//         }
//         else
//         {
//             Debug.LogError("StoneThrow TIDAK ditemukan!");
//         }
        
//     }

//     void Update()
//     {
//         if (Input.GetKeyDown(KeyCode.Space)) // Tombol untuk menendang
//         {
//             Stone.enabled = true;
//             Debug.Log("Batu muncul");
//             CheongYul.SetTrigger("OnThrow");
//             StoneThrow.SetTrigger("Bounce");
//         }
//         if (Stone.enabled && IsAnimationFinished(StoneThrow, "Bounce"))
//         {
//             Stone.enabled = false;
//             Debug.Log("Stone disembunyikan setelah animasi selesai.");
//         }
        
//     }
//     private bool IsAnimationFinished(Animator animator, string animationName)
// {
//     AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
//     return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 1f;
// }
//     public void OnThrow()
//     {
//         Debug.Log("Method dipanggil.");
//         if (StoneThrow != null)
//         {
//             Debug.Log("Animasi berhasil");
//         }
//         else
//         {
//             Debug.LogError("Ball Animator belum di-assign!");
//         }
//     }
// }