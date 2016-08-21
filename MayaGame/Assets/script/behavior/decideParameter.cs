using UnityEngine;
using System.Collections;

public enum SetNumType
{
    set,
    add,
    subtractive
}

public class decideParameter : StateMachineBehaviour {
    public string paramName;
    public SetNumType type;
    public float defParam;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float param = animator.GetFloat(paramName);
        
        if(type == SetNumType.set)
        {
            animator.SetFloat(paramName, defParam);
        }
        if (type == SetNumType.add)
        {
            param = param + defParam;
            animator.SetFloat(paramName, param);
        }
        if (type == SetNumType.subtractive)
        {
            param = param - defParam;
            animator.SetFloat(paramName, param);
        }

    }
}
