using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class Door : MonoBehaviour
{
    [Header("Rooms")]
    [SerializeField] private string startingRoom;
    [SerializeField] private List<string> normalRooms; 
    [SerializeField] private List<string> miniBossRooms;
    [SerializeField] private string bossRoom;
    [SerializeField] private string healRoom;

    [Header("Room Counters")]
    private static int roomCount = 1; 
    public TextMeshProUGUI roomCountText;
    private static HashSet<string> visitedScenes = new HashSet<string>();
    private static bool isBossRoomTriggered = false; 
    private static bool isGameInitialized = false;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip enterRoomSound;

    [Header("Animation Settings")]
    [SerializeField] private Animator doorAnimator;

    [Header("Transition Settings")]
    [SerializeField] private GameObject transitionDoor;

    private void Awake()
    {      
        if (!isGameInitialized)
        {
            ResetGameProgress();
            isGameInitialized = true;
        }
        LoadVisitedScenes();

        if (transitionDoor != null)
        {
            transitionDoor.SetActive(false);
        }
    }

    private void OnApplicationQuit()
    {
        SaveVisitedScenes();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            roomCount++;
            Debug.Log("Player entered trigger. Room count incremented to: " + roomCount);

            if (isBossRoomTriggered || roomCount >= 11)
            {
                isBossRoomTriggered = true;
                Debug.Log("Switching to Boss Room by default");
                SwitchToRoom(bossRoom);
            }
            else if (roomCount >= 6 && Random.value <= 0.4f)
            {
                isBossRoomTriggered = true;
                Debug.Log("Switching to Boss Room by probability.");
                SwitchToRoom(bossRoom);
            }
            else if (ShouldEnterMiniBossRoom())
            {
                Debug.Log("Switching to Mini-Boss Room.");
                string miniBoss = GetRandomRoom(miniBossRooms);
                SwitchToRoom(miniBoss);
            }
            else if (ShouldEnterHealRoom())
            {
                Debug.Log("Switching to Heal Room.");
                SwitchToRoom(healRoom);
            }
            else
            {
                Debug.Log("Switching to Normal Room.");
                string normalRoom = GetRandomRoom(normalRooms);
                SwitchToRoom(normalRoom);
            }
        }
    }

    private bool ShouldEnterMiniBossRoom()
    {
        // 30% chance after 3 rooms
        return roomCount >= 4 && Random.value <= 0.3f;
    }

    private bool ShouldEnterHealRoom()
    {
        // 30% chance after 4 rooms
        return roomCount >= 5 && Random.value <= 0.3f;
    }

    private void SwitchToRoom(string roomName)
    {
        if (transitionDoor != null)
        {
            transitionDoor.SetActive(true);
        }

        if (doorAnimator != null)
        {
            Debug.Log("Triggering Close animation");
            doorAnimator.SetTrigger("Close");
        }

        if (audioSource != null && enterRoomSound != null)
        {
            StartCoroutine(WaitForSoundAndSwitch(roomName, enterRoomSound.length));
        }
        else
        {
            StartCoroutine(WaitForAnimationAndSwitch(roomName));
        }

        // SceneManager.LoadScene(roomName);
        // visitedScenes.Add(roomName);
        // SaveVisitedScenes();
        // RoomText.Instance.UpdateRoomCount(roomCount);
        
    }

    private IEnumerator WaitForSoundAndSwitch(string roomName, float delay)
    {
        if (audioSource != null && enterRoomSound != null)
        {
            audioSource.PlayOneShot(enterRoomSound);
            yield return new WaitForSeconds(1f); 
        }

        if (doorAnimator != null)
        {
            AnimatorStateInfo stateInfo = doorAnimator.GetCurrentAnimatorStateInfo(0);
            
            while (!stateInfo.IsName("Transition_Close") || stateInfo.normalizedTime < 1f)
            {
                stateInfo = doorAnimator.GetCurrentAnimatorStateInfo(0);
                yield return null;
            }
        }

        // SceneManagerController.Instance.SwitchScene(roomName, SceneManagerController.GameMode.Exploration);
        
        Debug.Log($"[DEBUG] Loading scene: {roomName}");
        SceneManager.LoadScene(roomName);
        StartCoroutine(PlayOpenAnimation());
        transitionDoor?.SetActive(false);
    }

    private string GetRandomRoom(List<string> roomList)
    {
        List<string> unvisitedRooms = new List<string>();

        foreach (string room in roomList)
        {
            if (!visitedScenes.Contains(room))
            {
                unvisitedRooms.Add(room);
            }
        }

        Debug.Log("Unvisited Rooms: " + string.Join(", ", unvisitedRooms));

        if (unvisitedRooms.Count > 0)
        {
            int randomIndex = Random.Range(0, unvisitedRooms.Count);
            Debug.Log("Selected Room: " + unvisitedRooms[randomIndex]);
            return unvisitedRooms[randomIndex];
        }

        // Jika semua ruangan sudah dikunjungi, ambil ruangan secara acak dari daftar asli
        string fallbackRoom = roomList[Random.Range(0, roomList.Count)];
        Debug.Log("Fallback Room: " + fallbackRoom);
        return fallbackRoom;
    }

    private void SaveVisitedScenes()
    {
        PlayerPrefs.SetString("VisitedScenes", string.Join(",", visitedScenes));
        PlayerPrefs.SetInt("RoomCount", roomCount);
        PlayerPrefs.Save();
    }

    private void LoadVisitedScenes()
    {
        string savedData = PlayerPrefs.GetString("VisitedScenes", "");
        roomCount = PlayerPrefs.GetInt("RoomCount", 1);

        if (!string.IsNullOrEmpty(savedData))
        {
            visitedScenes = new HashSet<string>(savedData.Split(','));
        }
    }

    private void ResetGameProgress()
    {
        roomCount = 1;
        visitedScenes.Clear();
        PlayerPrefs.DeleteKey("RoomCount");
        PlayerPrefs.DeleteKey("VisitedScenes");
        PlayerPrefs.Save();
    }

    private IEnumerator PlayOpenAnimation()
    {
        yield return null;      
        //yield return new WaitForSeconds(0.5f);
        if (doorAnimator != null)
        {
            AnimatorStateInfo stateInfo = doorAnimator.GetCurrentAnimatorStateInfo(0);
            Debug.Log($"Triggering Open animation. Current State: {stateInfo.fullPathHash}");
            doorAnimator.SetTrigger("Open");
        }
        else
        {
            Debug.LogError("Door Animator is null!");
        }

        yield return new WaitForSeconds(1f); 

        if (transitionDoor != null)
        {
            transitionDoor.SetActive(false);
        }
    }

    private IEnumerator WaitForAnimationAndSwitch(string roomName)
    {
        // Tunggu animasi close selesai
        if (doorAnimator != null)
        {
            AnimatorStateInfo stateInfo = doorAnimator.GetCurrentAnimatorStateInfo(0);
            Debug.Log($"[DEBUG] Current Animator State: {stateInfo.fullPathHash}, Normalized Time: {stateInfo.normalizedTime}");
            
            while (!stateInfo.IsName("Transition_Close"))
            {
                stateInfo = doorAnimator.GetCurrentAnimatorStateInfo(0);
                yield return null;
            }
        }
 
        // Pindah ke scene baru
        SceneManager.LoadScene(roomName);

        // Tunggu scene selesai dimuat
        yield return null;

        // Trigger animasi open
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("Open");
        }
        else
        {
            Debug.LogError("Door Animator is null!");
        }
    }
    
}