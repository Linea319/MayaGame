using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


public class Payload : NetworkBehaviour {
    public Transform[] Target;
    public bool canMove;
    public float stopRate;
    public float timeRate;
    NavMeshAgent agent;
    UIMessenger messager;
    int targetNum = 0;
    float timer;
    bool fragMove = false;
    bool move;
    Vector3 prePosition;



    // Use this for initialization
    void Start () {
        messager = GetComponent<UIMessenger>();
        if (isServer)
        {
            agent = GetComponent<NavMeshAgent>();
            agent.enabled = true;
            agent.SetDestination(Target[targetNum].position);
            Stop();
            
        }
        
	}
	
	// Update is called once per frame
    [ServerCallback]
	void Update () {
        //Debug.Log(agent.pathStatus + ":"+agent.desiredVelocity.sqrMagnitude);
        if (!isServer)
        {
            if (fragMove) {
                //CmdSetMove();
                fragMove = false;
            }
            return;
        }
           
        if (canMove)
        {
            /*
            for (int i = 0; i < agent.path.corners.Length - 1; i++)
            {
                Debug.DrawLine(agent.path.corners[i], agent.path.corners[i + 1], Color.red, 1f);
            }
            */
            if (move)
            {
                
                Vector3 dir = Target[targetNum].position - transform.position;
                agent.Stop();
                transform.Translate(dir.normalized * 1.5f*Time.deltaTime);               
                
            }

            if ((transform.position - prePosition).magnitude < 0.01f*Time.deltaTime)
            {
                move = true;
                agent.updatePosition = false;
            }
            prePosition = transform.position;
            if (Vector3.Distance(transform.position, Target[targetNum].position) < 0.5f)
            {
                    targetNum++;
                    if (targetNum >= Target.Length)
                    {
                        canMove = false;
                    return;
                    }
                agent.SetDestination(Target[targetNum].position);
                move = false;
                agent.nextPosition = transform.position;
                agent.updatePosition = true;
                agent.Resume();
            }
            

            if (Time.time > timer)
            {
                float num = Random.Range(0, 100f);
                if (num < stopRate)
                {
                    Stop();
                }
                timer = Time.time + timeRate;
            }

            //agent.Resume();
        }
        else
        {
            Stop();
        }

        

	}

    [Server]
    public void Stop()
    {
        canMove = false;
        move = false;
        agent.Stop();
        RpcSetMove(true);
        
    }

    [Client]
    public void SetMove()
    {
        //fragMove = true;
        CmdSetMove();
    }

    [Command]
    void CmdSetMove()
    {
        canMove = true;
        agent.Resume();
        timer = Time.time + timeRate;
        RpcSetMove(false);
        
    }

    [ClientRpc]
    public void RpcSetMove(bool hyouzi)
    {
        if(messager != null)
        messager.enabled = hyouzi;
    }

}
