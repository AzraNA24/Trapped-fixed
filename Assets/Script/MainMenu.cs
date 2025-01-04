using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
     public SceneTransition sceneTransition;

     public void GoToScene(string sceneName){
          //sceneTransition.TransitionToScene(sceneName);  
          //SceneManagerController.Instance.StartNewGame();

          sceneTransition.TransitionToScene(sceneName, () =>
          {
               if (Player.Instance != null)
               {
                    Player.Instance.ResetInventory();
                    Player.Instance.currentHealth = Player.Instance.Health;
                    Player.Instance.Money = 100;
               }
               else
               {
                    Debug.LogWarning("Player instance not found. Player-related resets skipped.");
               }

               PlayerPrefs.DeleteAll(); 
               Debug.Log("Loot status direset.");
          });
     }
}