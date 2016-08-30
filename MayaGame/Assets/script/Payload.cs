using UnityEngine;
using System.Collections;

public class Payload : MonoBehaviour {
    public Transform[] Target;
    public bool canMove;
    NavMeshAgent agent;
    int targetNum = 0;
	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(Target[targetNum].position);
        //agent.CalculatePath();
        
	}
	
	// Update is called once per frame
	void Update () {
        if (canMove )
        {
            /*
            for (int i = 0; i < agent.path.corners.Length - 1; i++)
            {
                Debug.DrawLine(agent.path.corners[i], agent.path.corners[i + 1], Color.red, 1f);
            }
            */
            Debug.Log(agent.pathStatus);
            if (Vector3.Distance(transform.position, Target[targetNum].position) < 0.5)
            {
                    targetNum++;
                    if (targetNum >= Target.Length)
                    {
                        canMove = false;
                    return;
                    }
                    agent.SetDestination(Target[targetNum].position);
            }
            if (agent.pathStatus != NavMeshPathStatus.PathComplete)
            {
                
                    Vector3 dir = transform.position - agent.destination;
                    agent.Move(dir.normalized * agent.speed * Time.deltaTime);
                
            }
            //agent.Resume();
        }
        else
        {
            agent.Stop();
        }
	}
}
