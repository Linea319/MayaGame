using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class HitManagerDef : MonoBehaviour {

    public EffectType effectType;
    public float hitPoint;
    public ArmorParameter armor;
    public GameObject DestroyObj;
    public bool yoroke;
    public float speedDebufRate=1f;
    public float damageDebufRate = 1f;

    [HideInInspector]
    public Collider col;
    [HideInInspector]
    public Bounds meshBounds;
    [HideInInspector]
    public float sizeMagnitude;
    [HideInInspector]
    public NetAdapter net;
    [HideInInspector]
    public Renderer mesh;
    [HideInInspector]
    public float colSize;
    bool death;
    [HideInInspector]public float maxHitPoint;

    // Use this for initialization
    void Start()
    {

        Initialize();
    }

    public virtual void Initialize()
    {
        col = GetComponent<Collider>();
        colSize = col.bounds.size.magnitude;
        meshBounds =col.bounds;
        sizeMagnitude = meshBounds.extents.magnitude;
        maxHitPoint = hitPoint;
        net = transform.root.GetComponent<NetAdapter>();

        if (net != null )
        {
            //Debug.Log(this.name);
           // net.crackObjs.Add(gameObject.name, this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hitPoint <= 0 && !death && DestroyObj!=null)
        {
            if (DestroyObj == net.gameObject)
            {
                if (net.isServer)
                {
                    StartCoroutine( net.Death());
                }
            }
            else
            {
                Destroy(DestroyObj);
            }
            death = true;
        }
        // Debug.Log(meshBounds.center);
    }

   

    public virtual void Destruct()
    {

    }

    public virtual Vector3 HitDamage(DamageParameter damages, RaycastHit hitInfo, Ray ray)
    {

        float pointRate = (1f - Vector3.Distance(col.bounds.center, hitInfo.point) / sizeMagnitude) * 1.5f;
        Debug.DrawLine(col.bounds.center, ray.origin, Color.red, 1f);
        pointRate = Mathf.Clamp01(pointRate);
        //Debug.Log(pointRate);
        float damage = damages.shock / armor.shockResist * pointRate;
        Ray returnRay = new Ray(ray.GetPoint(hitInfo.distance * 2f+colSize), -ray.direction);
        RaycastHit returnHit;
        hitInfo.collider.Raycast(returnRay, out returnHit, hitInfo.distance+colSize);
        float penetrateLength = Vector3.Distance(hitInfo.point, returnHit.point);
        float penetrateNum = damages.penetration * 0.001f - penetrateLength * armor.armorResist;

        Vector3 rePoint = Vector3.zero;
        if (penetrateNum > 0)
        {
            //pointRate = (1f - rayDistance(ray, hitInfo.point) / sizeMagnitude);
            damage += penetrateNum * 1000f * pointRate;
            rePoint = returnHit.point;
        }
        hitPoint -= damage;
        Debug.Log("col:" + name + " damage:" + damage + " penetrate:" + penetrateNum * 1000f * pointRate);

        EnemyAI ai = transform.root.GetComponent<EnemyAI>();
        ai.shock += damages.shock / armor.shockResist * 0.16f;

        return rePoint;
    }

    public virtual void SetHP(float hp)
    {
        hitPoint = hp;
    }

    public float rayDistance(Ray ray,Vector3 point)
    {
        float distance = Vector3.Cross(point+ray.direction, meshBounds.center-point).magnitude/(point + ray.direction).magnitude;
        return distance;                                                                                                                                                                                                                                 
    }
}
