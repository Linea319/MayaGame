using UnityEngine;
using System.Collections;

public class Chase : StateMachineBehaviour
{
    public bool zigzag;
    public float zigzagTime;
    [Range(0,1)]
    public float zigzagRate;

    NavMeshAgent nav;
    Transform target;
    float zigzagTimer=0;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        EnemyAI ai = animator.transform.root.GetComponent<EnemyAI>();
        if(ai.target == null)
        {
            ai.SearchTarget();
        }
        nav = animator.transform.root.GetComponent<NavMeshAgent>();
        target = ai.target;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (zigzag && zigzagTimer<Time.time)
        {
            float dis = Vector3.Distance(animator.transform.position, target.position);
            nav.destination = target.position+Random.insideUnitSphere*dis;
            zigzagTimer = Time.time + zigzagTime;
        }
        else if(!zigzag){
            nav.destination = target.position;
        }
        Debug.DrawLine(animator.transform.position, nav.destination, Color.blue);
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
