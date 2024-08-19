using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorControllerHandler : MonoBehaviour
{

    private Animator animator;
    RuntimeAnimatorController mainController;
    public RuntimeAnimatorController[] minigameControllers;

    public enum ControllerType
    {
        main, minigame
    }

    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        mainController = animator.runtimeAnimatorController;
    }

    public void SetAnimatorController(ControllerType controllerType, int index)
    {
        if (controllerType == ControllerType.main)
        {
            animator.runtimeAnimatorController = mainController;
            return;
        }

        if (minigameControllers[index] != null)
        {
            animator.runtimeAnimatorController = minigameControllers[index];
            return;
        }

        Debug.LogError("Invalid Minigame Controller Index");
    }

    

    
}
