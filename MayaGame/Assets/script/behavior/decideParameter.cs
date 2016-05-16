using UnityEngine;
using System.Collections;

public class decideParameter : StateMachineBehaviour {
    public string paramName;
    public float defParam;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat(paramName, defParam);
    }
}
