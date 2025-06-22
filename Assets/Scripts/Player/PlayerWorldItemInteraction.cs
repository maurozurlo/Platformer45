using UnityEngine;
using System;
using System.Collections.Generic;

public enum ANIM_TRIGGER { drown, shake }
public enum WORLD_ITEM { shakeable_obj, killing_fluid }

public class PlayerWorldItemInteraction : MonoBehaviour
{
    Animator anim;
    public Camera killCamera;
    AnimatorControllerHandler handler;

    static readonly int TriggerIdle = Animator.StringToHash("idle");
    static readonly int TriggerDrown = Animator.StringToHash("drown");
    static readonly int TriggerShake = Animator.StringToHash("shake");
    static readonly int BoolIsGrounded = Animator.StringToHash("IsGrounded");

    Dictionary<WORLD_ITEM, Action> interactionMap;

    private void Start()
    {
        anim = GetComponent<Animator>();
        handler = GetComponent<AnimatorControllerHandler>();
        if (anim == null)
        {
            Debug.LogError("Animator not found on Player!");
            return;
        }

        interactionMap = new Dictionary<WORLD_ITEM, Action> {
            { WORLD_ITEM.killing_fluid, () => {
                anim.SetTrigger(TriggerDrown);
            }},
            { WORLD_ITEM.shakeable_obj, () => anim.SetTrigger(TriggerShake) }
        };
    }

    private void StopAnimation(bool grounded)
    {
        anim.StopPlayback();
        anim.SetBool(BoolIsGrounded, grounded);
    }

    public void TriggerInteraction(WORLD_ITEM item)
    {
        StopAnimation(true);
        anim.SetTrigger(TriggerIdle);
        handler.SetAnimatorController(1);
        if (interactionMap.TryGetValue(item, out var action))
        {
            action.Invoke();
        }
        else
        {
            Debug.LogWarning($"No interaction defined for {item}");
        }
    }
}
