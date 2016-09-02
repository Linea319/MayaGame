using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public interface BehaveInterface
{
   void Think();
    void Jump();
}


public class EnemyAI : NetworkBehaviour,BehaveInterface
{
    public float thinkRate = 3f;
    public float effectionRate = 50f;//100=limit
    public LayerMask mask;
    public Animator anim;
    public Animator AIAnim;
    public SyncAnim syncAnim;

    //parameter
    public float moveSpeed = 4f;
    public float defDistance = 25f;

    protected NavMeshAgent nav;
    float thinkTimer;
    public Transform target;
    Vector3 moveTarget;
    protected bool atack;
    public Collider[] atackCol;
    public float attackDamage = 25f;
    public float jumpDelay = 1f;

    //emotionRate
    public Vector2 attackEmotionRate;

    //emotion
    protected float thinkEmotion=50f;
    protected float distanceEmotion=50f;
    protected float attackEmotion = 0f;
    protected float retreatEmotion = 0f;

    //hate
    protected float hatepool;
    protected float currentHate;
    protected Transform hateTarget;

    //state 
    [SyncVar][HideInInspector] public bool dead;
    protected bool stopAI;
    [HideInInspector] public float shock;
    protected bool retreat;

    
    
    bool jumpNow;
    float jumpTimer;

    // Use this for initialization
    [ServerCallback]
    void Start () {

        nav = GetComponent<NavMeshAgent>();
        nav.enabled = true;
        nav.speed = moveSpeed;
        //nav.updatePosition = false;
        //nav.updateRotation = false;
	}

    // Update is called once per frame
    [ServerCallback]
    public virtual void Update () {
        if (dead || stopAI)
        {
            nav.Stop();
            return;
        }

        currentHate -= 25 * Time.deltaTime;
        nav.speed = Mathf.Lerp(moveSpeed, 0, shock / 100f);
        shock = Mathf.Lerp(shock, 0, Time.deltaTime*0.5f);

	    if(Time.time > thinkTimer)
        {
            //Think();
            AIAnim.SetTrigger("start");
            thinkTimer = Time.time + thinkRate;
            retreat = false;
        }

        if((moveTarget-transform.position).sqrMagnitude > 1f)
        {
            
        }
        else
        {

        }

        if (nav.isOnOffMeshLink)
        {
            Jump();
        }

	}

    public void Jump()
    {
        OffMeshLinkData data = nav.currentOffMeshLinkData;
        Vector3 endPos = data.endPos;
        Vector3 startPos = data.startPos;
        float height = Mathf.Abs(endPos.y - startPos.y);
        if (!jumpNow)
        {
            transform.LookAt(new Vector3(endPos.x, transform.position.y, endPos.z));
            jumpNow = true;
            jumpTimer = 0;
            syncAnim.SetTrigger("Jump");
        }
        jumpTimer += Time.deltaTime;
        if (jumpTimer > jumpDelay)
        {
            transform.position = Vector3.Lerp(startPos, endPos, jumpTimer - jumpDelay) + new Vector3(0, Mathf.Sin((jumpTimer - jumpDelay) * Mathf.PI) * height, 0);
        }
            
        //transform.position.y = Mathf.Sin((jumpTimer - 1) * Mathf.PI);
        //Debug.Log(Mathf.Sin((jumpTimer - 1) * Mathf.PI));
        if ((transform.position - endPos).sqrMagnitude < 0.5) 
        {
            nav.CompleteOffMeshLink();
            jumpNow = false;
            syncAnim.SetTrigger("JumpEnd");
        }
    }

    public virtual void Think()
    {
        if (target == null || target.GetComponent<HitManagerPlayer>().hitPoint<=0) SearchTarget();

        Move();
        //thinkTimer = Time.time + thinkRate;
    }

    public virtual void Move()
    {
        Vector3 vec = (transform.position - target.position).normalized;
        vec.y = 0;
        vec = vec.normalized;
        float distance = defDistance * (1f - distanceEmotion / 100f);
        Collider[] points = Physics.OverlapSphere(target.position + vec * distance, 10f, mask);
        
        int posNum = 0;
        if (points.Length > 0)
        {
            if (moveTarget == points[posNum].transform.position)
            {
                posNum = Random.Range(0, points.Length);
            }
            moveTarget = points[posNum].transform.position;

        }
        else
        {
            moveTarget = (target.position + vec * distance) + Random.insideUnitSphere * 5f;

        }
        Debug.DrawLine(moveTarget, transform.position, Color.blue, 1f);
        nav.SetDestination(moveTarget);
    } 

    public virtual void Retreat()
    {
        Vector3 vec = (transform.position - target.position).normalized;
        vec.y = 0;
        vec = vec.normalized;
        float distance = defDistance * (retreatEmotion / 100f) * 0.5f;
        //Collider[] points = Physics.OverlapSphere(transform.position + vec * distance, 10f, mask); 
        moveTarget = (transform.position + vec * distance)+ Random.insideUnitSphere * 5f;
        nav.SetDestination(moveTarget);
        if (retreatEmotion < 90f)
        {
            nav.updateRotation = false;
        }
        Debug.DrawLine(moveTarget, transform.position, Color.green, 1.5f);
        //retreatEmotion *= 0.5f;
        retreat = true;
        nav.Resume();
    }

    public virtual void Dodge()
    {

    }

    public virtual void Dodge(int dir)
    {

    }

    public virtual void Attack()
    {
        thinkTimer  = Time.time + 1f;
    }

    public void SearchTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if(players.Length > 0)
        {
            float distance=float.MaxValue;
            for(int i = 0; i < players.Length; i++)
            {
                float cDis = (transform.position - players[i].transform.position).sqrMagnitude;
                if (distance > cDis && players[i].GetComponent<HitManagerPlayer>().hitPoint >0)
                {
                    target = players[i].transform;
                    distance = cDis;
                }
            }
        }
    }

    public void searchTargetFar()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 0)
        {
            float distance = 0;
            for (int i = 0; i < players.Length; i++)
            {
                float cDis = (transform.position - players[i].transform.position).sqrMagnitude;
                if (distance < cDis && players[i].GetComponent<HitManagerPlayer>().hitPoint > 0)
                {
                    target = players[i].transform;
                    distance = cDis;
                }
            }
        }
    }

    public void searchTargetHate()
    {
        if (hateTarget == null)
        {
            SearchTarget();
            return;
        }
        if(hateTarget.GetComponent<HitManagerPlayer>().hitPoint > 0)
        target = hateTarget;
    }

    public virtual void AttackStart(int num)
    {
        atackCol[num].enabled = true;         
        atack = true;
    }

    public virtual void AttackEnd(int num)
    {
        atackCol[num].enabled = false;
        atack = false;
    }

    public void AttackHit(Transform target)
    {
        HitManagerPlayer playerHP = target.GetComponent<HitManagerPlayer>();
        playerHP.HitDamage(attackDamage);
        Debug.Log("attackhit");
    }

    public void EnterNear(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            AIAnim.SetTrigger("near");
        }
    }

    public virtual void Stop()
    {
        dead = true;
        nav.Stop();
    }

    [Server]
    public void StopSend(float time)
    {
        StartCoroutine(StopOnTime(time));
    }

    [Server]
     IEnumerator StopOnTime(float time)
    {

        Debug.Log("Yoroke");
        stopAI = true;
        nav.Stop();
        attackEmotion = 0f;
        retreatEmotion += 10f;
        yield return new WaitForSeconds(time);
        Debug.Log("ReturnYoroke");
        stopAI = false;
        nav.Resume();
        //AIAnim.SetTrigger("start");
        //thinkTimer = Time.time + thinkRate;
        
    } 

    public virtual void SetHate(Transform target,float hate)
    {
        if (target != hateTarget)
        {
            hatepool += hate;
            if (hatepool > currentHate)
            {
                currentHate = hatepool;
                hatepool = 0;
                hateTarget = target;
            }
        }
        else
        {
            currentHate += hate;
         }
        //Debug.Log("hatetarget:"+hateTarget);
    }
}
