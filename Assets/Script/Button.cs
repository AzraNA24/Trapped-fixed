using UnityEngine;

public class Button: MonoBehaviour
{
    public Animator animator;
    public BattleSystem battle;

    void Start()
    {
        if (battle == null)
        {
            battle = FindObjectOfType<BattleSystem>(); // Menemukan BattleSystem jika belum diassign di Inspector
        }
    }
    public void OnDrinkPotionButtonPressed()
    {
        battle.OnPotionButton();
        TriggerAnimation("Drink");
    }

    public void OnLongRangeButtonPressed()
    {
        battle.OnLongRangeAttackButton();
        TriggerAnimation("LongRange");
    }

    public void OnShortRangeButtonPressed()
    {
        battle.OnShortRangeAttackButton();
        TriggerAnimation("ShortRange");
    }

    private void TriggerAnimation(string animationName)
    {   
        if (animator == null)
        {
            Debug.LogError("Animator not assigned!");
            return;
        } else{
            Debug.Log("Animator assigned");
        }
        if (animator.runtimeAnimatorController == null)
        {
            Debug.LogError("AnimatorController is not assigned to the Animator!");
            return;
        }

        animator.SetBool(animationName, true);
        StartCoroutine(ReturnToStanceAfterAnimation(animationName));
    }


    private System.Collections.IEnumerator ReturnToStanceAfterAnimation(string animationName)
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.SetBool(animationName,false);
    }
}