using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public Animator animator; 
    public float transitionDuration = 1f;
    
    public void TransitionToScene(string sceneName)
    {
        StartCoroutine(Transition(sceneName));
    }

    private IEnumerator Transition(string sceneName)
    {
        // Trigger the fade-out animation
        animator.SetTrigger("Fade out");

        // Wait for the transition duration
        yield return new WaitForSeconds(transitionDuration);

        // Load the next scene
        SceneManager.LoadScene(sceneName);
    }
}
