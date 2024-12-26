using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public Transform leftDoor; 
    public Transform rightDoor; 
    public Animator animator;  
    public float doorWidth = 1f; 

    private void Start()
    {
        AdjustDoorSizeAndPosition(); 
    }

    private void AdjustDoorSizeAndPosition()
    {
        // Hitung ukuran layar berdasarkan kamera
        float screenWidth = Camera.main.orthographicSize * Camera.main.aspect;
        float screenHeight = Camera.main.orthographicSize;

        leftDoor.localScale = new Vector3(doorWidth, screenHeight * 2, 1);
        rightDoor.localScale = new Vector3(doorWidth, screenHeight * 2, 1);

        leftDoor.position = new Vector3(-screenWidth, 0, 0);
        rightDoor.position = new Vector3(screenWidth, 0, 0);
    }

    public void StartOpenAnimation()
    {
        AdjustDoorSizeAndPosition(); 
        animator.SetTrigger("Open");
    }

    public void StartCloseAnimation()
    {
        AdjustDoorSizeAndPosition(); 
        animator.SetTrigger("Close");
    }
}
