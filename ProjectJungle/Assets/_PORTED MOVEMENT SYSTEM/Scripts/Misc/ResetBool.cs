using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBool : StateMachineBehaviour
{
    [SerializeField] private string isLockedInAnimBool;
    [SerializeField] private bool isLockedInAnimStatus;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(isLockedInAnimBool, isLockedInAnimStatus);
    }
}