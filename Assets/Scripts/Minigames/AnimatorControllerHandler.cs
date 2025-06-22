using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorControllerHandler : MonoBehaviour
{

    private Animator animator;
    public RuntimeAnimatorController[] animationControllers;
        
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetAnimatorController(int index)
    {
        if (animationControllers[index] != null)
        {
            animator.runtimeAnimatorController = animationControllers[index];
            return;
        }

        Debug.LogError("Invalid Animator Controller Index");
    }
}
