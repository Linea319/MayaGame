using UnityEngine;
using System.Collections;

public class Attack : StateMachineBehaviour
{
    public bool attackStart;
    public bool attackEnd;
    public int attackNum;
    public float attackDelay;
    float timer;
    int cAttacknum;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (attackStart)
        {
            animator.transform.root.GetComponent<EnemyAI>().AttackStart(0);
            return;
        }
        if (attackEnd)
        {
            animator.transform.root.GetComponent<EnemyAI>().AttackEnd(0);
            return;
        }
        animator.transform.root.GetComponent<EnemyAI>().Attack();
        timer = Time.time+attackDelay;
        cAttacknum = attackNum;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    if(cAttacknum > 1 && Time.time > timer)
        {
            cAttacknum--;
            
            animator.transform.root.GetComponent<EnemyAI>().Attack();
            timer = Time.time + attackDelay;
        }
    if(cAttacknum < 1)
        {
            animator.SetBool("blocker", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}

