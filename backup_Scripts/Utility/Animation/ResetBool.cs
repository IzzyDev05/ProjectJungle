using UnityEngine;

public class ResetBool : StateMachineBehaviour
{
    [SerializeField] private string isInteractingBool;
    [SerializeField] private bool isInteractingStatus;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetBool(isInteractingBool, isInteractingStatus);
    }
}