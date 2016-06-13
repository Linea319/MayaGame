using UnityEngine;
using Exploder;


[System.Serializable]
public class ArmorParameter : System.Object
{

    public float shockResist = 1f;
    public float armorResist = 1f;
    public float heatResist = 1f;

}

public class HitManeger : HitManagerDef {
    ExploderObject exploder;


    // Use this for initialization
    void Start () {

        Initialize();
    }

    public override void Initialize()
    {
        exploder = FindObjectOfType<ExploderObject>();
        if (mesh == null)
        {
            meshBounds = gameObject.GetComponent<Renderer>().bounds;
        }
        colSize = meshBounds.size.magnitude;
        sizeMagnitude = meshBounds.extents.magnitude;
        
        net = transform.root.GetComponent<NetAdapter>();
        
        if (net != null)
        {
            //Debug.Log(this.name);
            net.crackObjs.Add(gameObject.name, this);
        }

        
    }

    // Update is called once per frame
    void Update () {
	    if(hitPoint <= 0)
        {
            Destruct();
            Destroy(gameObject);
        }
       // Debug.Log(meshBounds.center);
	}

    public override void Destruct()
    {
        if (yoroke)
        {
            EnemyAI ai = transform.root.GetComponent<EnemyAI>();
            ai.moveSpeed *= speedDebufRate;
            ai.attackDamage *= damageDebufRate;
            ai.GetComponent<SyncAnim>().SetTrigger("hit");
            StartCoroutine(ai.StopOnTime(0.6f));
        }

        if(mesh != null)
        {
            exploder.ExplodeObject(mesh.gameObject);
            
            return;
        }
        exploder.ExplodeObject(gameObject);
    }

    public override Vector3 HitDamage(DamageParameter damages,RaycastHit hitInfo,Ray ray)
    {
        
        float pointRate = (1f - Vector3.Distance(mesh.bounds.center, hitInfo.point) / sizeMagnitude)*1.5f;
        Debug.DrawLine(mesh.bounds.center, ray.origin,Color.red,1f);
        pointRate = Mathf.Clamp01(pointRate);
        //Debug.Log(pointRate);
        float damage = damages.shock / armor.shockResist*pointRate;
        Ray returnRay = new Ray(ray.GetPoint(hitInfo.distance*2f+colSize), -ray.direction);
        RaycastHit returnHit;
        hitInfo.collider.Raycast(returnRay, out returnHit, hitInfo.distance+colSize);
        float penetrateLength = Vector3.Distance(hitInfo.point,returnHit.point);
        float penetrateNum = damages.penetration*0.001f - penetrateLength * armor.armorResist;
        //Debug.Log("armorLength:"+penetrateLength+",penetrate:"+penetrateNum);
        Vector3 rePoint = Vector3.zero;
        if(penetrateNum >0)
        {
            //pointRate = (1f - rayDistance(ray, hitInfo.point) / sizeMagnitude);
            damage += penetrateNum*1000f * pointRate * pointRate;
            
            rePoint = returnHit.point;
        }
        hitPoint -= damage;
        Debug.Log("col:" + name + " damage:" + damage + " penetrate:"+ pointRate);

        EnemyAI ai = transform.root.GetComponent<EnemyAI>();
        ai.shock += damages.shock / armor.shockResist * 0.06f;

        return rePoint;
    }
}
