using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Payload : NetworkBehaviour {
    public Transform[] Target;
    public bool canMove;
    NavMeshAgent agent;
    int targetNum = 0;
	// Use this for initialization
	void Start () {
        if (isServer)
        {
            agent = GetComponent<NavMeshAgent>();
            agent.enabled = true;
            agent.SetDestination(Target[targetNum].position);
        }
        //agent.CalculatePath();
        
	}
	
	// Update is called once per frame
	void Update () {
        if (!isServer) return;
        if (canMove )
        {
            /*
            for (int i = 0; i < agent.path.corners.Length - 1; i++)
            {
                Debug.DrawLine(agent.path.corners[i], agent.path.corners[i + 1], Color.red, 1f);
            }
            */
           // Debug.Log(agent.pathStatus);
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
            if (agent.pathStatus != NavMeshPathStatus.PathComplete || agent.speed <= 0)
            {
                
                    Vector3 dir = transform.position - agent.destination;
                    agent.Move(dir.normalized * 1.5f * Time.deltaTime);
                
            }
            //agent.Resume();
        }
        else
        {
            agent.Stop();
        }
	}

    [Command]
    public void CmdSetMove()
    {
        canMove = true;
        agent.Resume();
    }
}
