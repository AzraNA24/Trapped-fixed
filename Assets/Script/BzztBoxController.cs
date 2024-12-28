using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BzztBoxController : MonoBehaviour
{
    public Animator animator;
    public float entranceDelay = 2.0f;
    public float shakeDuration = 2.0f;
    public float shakeMagnitude = 0.1f;

    public Transform objectToShake;

    private Vector3 originalPosition;
    private bool isShaking = false;

    void Start()
    {
        if (objectToShake != null)
        {
            originalPosition = objectToShake.localPosition;
        }
        else
        {
            Debug.LogWarning("Object to shake is not assigned!");
        }

        StartCoroutine(HandleEntranceAndShake());
    }

    private IEnumerator HandleEntranceAndShake()
    {
        // Wait for the delay, then play the entrance animation
        yield return new WaitForSeconds(entranceDelay);
        animator.SetTrigger("StartEntrance");

        // Wait for the entrance animation to finish (adjust time as needed)
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Start shaking the specified child object
        StartShake();
    }

    private void StartShake()
    {
        if (!isShaking && objectToShake != null)
        {
            StartCoroutine(Shake());
        }
    }

    private IEnumerator Shake()
    {
        isShaking = true;
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            objectToShake.localPosition = originalPosition + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;

            yield return null;
        }

        objectToShake.localPosition = originalPosition;
        isShaking = false;
    }

    public void Disappear()
    {
        // Make the parent object disappear when pressed
        gameObject.SetActive(false);
    }
}
