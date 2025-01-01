using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public SceneTransition sceneTransition;
   public void GoToScene(string sceneName){
        sceneTransition.TransitionToScene(sceneName);
        SceneManagerController.Instance.StartNewGame();
   }

}
