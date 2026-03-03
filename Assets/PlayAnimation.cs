using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    public Animator animator;

    public void TriggerAnimation()
    {
        animator.SetTrigger("PlayOnce");
    }
}