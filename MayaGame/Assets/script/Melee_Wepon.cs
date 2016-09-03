using UnityEngine;
using System.Collections.Generic;
using Exploder;


[System.Serializable]
public class MeleeParameter : System.Object
{
    public float chargeTime = 1f;
    public float chargeDamageRaate = 3f;
    public float useStamina;
    public float mobility = 0;
    public float range = 3.5f;
    
}

public class Melee_Wepon : MonoBehaviour, WeponInterface {

    Camera myCamera;
    public bool RayHit;
    [HideInInspector]
    public Animator anim;
    public Animator weponAnim;
    [HideInInspector]
    public FPSController FPSCon;
    public GameObject hitEffect;
    public AudioSource shotSound;
    public ParticleSystem muzuleFlash;
    public ParticleSystem yakkyou;
    [SerializeField]
    public MeleeParameter pa;
    public DamageParameter dam;
    public AnimationClip idleAnim;
    public AnimationClip chargeAnim;
    public AnimationClip attackAnim;
    public AnimationClip runAnim;
    public Collider WeponCollieder;
    public LayerMask wepMask;
    public ParticleSystem chargeEffect;
    public ParticleSystem chargeCompEffect;

    bool ADS;
    bool reload;
    bool sear;
    bool attackNow;
    bool canHit;
    bool first = true;
    int burst = 3;
    float burstDelay = 0.4f;
    float burstTimer;
    float ADSTimer = 0;
    float rpmTimer = 0;
    int mySlot;
    HitEffectManeger effecter;

    Vector3 recoilDmp;
    float recoilOffset;
    bool prePause;
    Vector3 magPos;
    Quaternion magRot;
    float reloadAnimRate = 1f;
    int magazine = 0;
    public Selector selector;
    public ExploderObject Exploder;
    Transform parent;
    int hitNum;
    float chargeTimer;


    // Use this for initialization
    void Start()
    {
        //initialize();

    }

    public void initialize()
    {
        WeponCollieder.enabled = false;
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
        myCamera = Camera.main;
        FPSCon = transform.root.GetComponent<FPSController>();
        //FPSCon.useWepon = this;
        anim = FPSCon.anim;
        if (first)
        {
            first = false;
        }
        else
        {
        }
        Exploder = FindObjectOfType<ExploderObject>();
        AnimatorOverrideController newAnime = new AnimatorOverrideController();
        newAnime.runtimeAnimatorController = FPSCon.defAnim;
        /* for (int b = 0; b < newAnime.animationClips.Length; b++)
         {
             Debug.Log(newAnime.animationClips[b].name);
         }*/
        // Debug.Log(reloadAnimRate);
        anim.SetFloat("ReloadSpeed", reloadAnimRate);
        newAnime[FPSCon.defAnim.animationClips[2].name] = idleAnim;
        newAnime[FPSCon.defAnim.animationClips[8].name] = chargeAnim;
        newAnime[FPSCon.defAnim.animationClips[7].name] = attackAnim;
        newAnime[FPSCon.defAnim.animationClips[6].name] = runAnim;
        // Debug.Log(newAnime.animationClips[1].name);
        anim.runtimeAnimatorController = newAnime;
        // Debug.Log(anim.runtimeAnimatorController.animationClips[1].name);
        reload = false;
        SendUI();
        effecter = FindObjectOfType<HitEffectManeger>();

    }

    public void Setup(int slot)
    {
        mySlot = slot;
        initialize();
    }


    // Update is called once per frame
    void Update()
    {
        if (Pause.pause) { return; }
        if (sear)
        {
            anim.SetBool("Melee", true);           
            chargeTimer += Time.deltaTime;
            weponAnim.SetFloat("ChargeRate", chargeTimer / pa.chargeTime);
            if(chargeEffect != null && !chargeEffect.isPlaying)
            {
                chargeEffect.Play();
            }
            if (chargeCompEffect != null && !chargeCompEffect.isPlaying && chargeTimer >= pa.chargeTime)
            {
                chargeCompEffect.Play();
            }
        }
        else
        {
            if (chargeEffect != null && chargeEffect.isPlaying)
            {
                chargeEffect.Stop();
            }
            if (chargeCompEffect != null && chargeCompEffect.isPlaying )
            {
                chargeCompEffect.Stop();
            }
        }

    }
    void LateUpdate()
    {

    }

    public void ReturnPrimaly()
    {
        if (sear) {
        anim.SetTrigger("MeleeAttack");
        weponAnim.SetBool("Charge", false);
        sear = false;
        attackNow = true;
        canHit = true;
            if (RayHit)
            {
                Ray wepRay = new Ray(myCamera.transform.position, myCamera.transform.forward);
                MeleeRay(wepRay,pa.range,0);
            }
            else
            {
                WeponCollieder.enabled = true;
            }
        
        hitNum = 0;
            FPSCon.stamina -= pa.useStamina;
        }

    }

    public void Primaly()
    {
        if (!sear)
        {
            anim.SetBool("Melee", false);
            anim.SetTrigger("MeleeTrigger");
            weponAnim.SetBool("Charge", true);
            sear = true;
            chargeTimer = 0;
        }
    }

    public void Secondary()
    {

    }

    public void Reload()
    {

    }

    void StopShot()
    {
        if (muzuleFlash.isPlaying) muzuleFlash.Stop();
    }

    public float ReturnChangeSpeed(float changeAnimLength)
    {
        float speed;
        speed = changeAnimLength / (pa.mobility * 0.01f);
        Debug.Log(pa.mobility);


        return speed;

    }

    public void AttackEnd()
    {
        attackNow = false;
        WeponCollieder.enabled = false;
        anim.SetBool("Melee", false);
    }

    public void CantAttack()
    {
        FPSCon.ADS = false;
        reload = true;
    }

    public void CanAttack()
    {
        reload = false;
    }

    public void ShotEffect()
    {
        muzuleFlash.Play();
        shotSound.Play();
    }

    public void SendUI()
    {
        if (FPSCon.isClient && FPSCon.UICon != null)
        {
            string name = gameObject.name;
            name = name.Replace("(Clone)", "");
            FPSCon.UICon.SetWeponText(mySlot, name,"---", "---");
            //Debug.Log("UISet");
        }
        //Debug.Log("UI");
    }

    

    public void MeleeHit(Collider col)
    {
        if (attackNow && canHit)
        {
            canHit = false;
            
            HitManagerDef hitM = col.transform.GetComponent<HitManagerDef>();
            if (hitM != null && hitNum < 4)
            {
                
                GameObject efect = Instantiate<GameObject>(effecter.ReturnEffect(hitM.effectType));
                efect.transform.position = col.ClosestPointOnBounds(WeponCollieder.bounds.center);
                efect.transform.LookAt(WeponCollieder.bounds.center);
                efect.transform.parent = col.transform;
                float chargeRate = chargeTimer / pa.chargeTime;
                chargeRate = Mathf.Clamp01(chargeRate);
                DamageParameter newDam = dam.multiple((1f-0.2f*hitNum)*(pa.chargeDamageRaate * chargeRate));
                Ray ray = new Ray(WeponCollieder.bounds.center, (col.bounds.center - WeponCollieder.bounds.center));
                RaycastHit hit;
                if (col.Raycast(ray, out hit, 10f))
                {
                    Vector3 penetratePoint = hitM.HitDamage(newDam, hit, ray);

                    FPSCon.CmdSendHP(hitM.transform.root.name, hitM.name, hitM.hitPoint,hitM.lastDamage);
                    Debug.Log(hitNum+":"+hit.collider.name+":"+ hitM.hitPoint);
                    hitNum++;
                    if (penetratePoint != Vector3.zero)
                    {
                        
                        canHit = true;
                    }
                }
            }
            else
            {
                GameObject efect = Instantiate<GameObject>(effecter.ReturnEffect(EffectType.misc));
                efect.transform.position = col.ClosestPointOnBounds(WeponCollieder.bounds.center);
                efect.transform.LookAt(WeponCollieder.bounds.center);
                efect.transform.parent = col.transform;

            }


        }
    }

    public void MeleeRay(Ray ray, float rayRange, int penetrateNum)
    {
        RaycastHit hit;
        LayerMask mask = ~(1 << 2 | 1 << 8);
        if (Physics.SphereCast(ray.origin,0.1f,ray.direction, out hit, rayRange, wepMask) && penetrateNum < 4)
        {
            //Debug.Log(hit.transform.name);

            Vector3 fracVec = hit.point;
            HitManagerDef hitM = hit.transform.GetComponent<HitManagerDef>();
            if (hitM != null)
            {
                GameObject efect = Instantiate<GameObject>(effecter.ReturnEffect(hitM.effectType));
                efect.transform.position = hit.point;
                efect.transform.LookAt(efect.transform.position + hit.normal);
                efect.transform.parent = hit.transform;

                float chargeRate = chargeTimer / pa.chargeTime;
                chargeRate = Mathf.Clamp01(chargeRate);
                DamageParameter newDam = dam.multiple((1f - 0.2f * hitNum) * (pa.chargeDamageRaate * chargeRate));
                Ray hitRay = new Ray(ray.origin, hit.point - ray.origin);
                Vector3 penetratePoint = hitM.HitDamage(newDam, hit, hitRay);

                FPSCon.CmdSendHP(hitM.transform.root.name, hitM.name, hitM.hitPoint, hitM.lastDamage);
                if (penetratePoint != Vector3.zero)
                {

                    Ray newRay = new Ray(hit.point - ray.direction*0.1f, ray.direction);
                    MeleeRay(newRay, rayRange, ++penetrateNum);
                }
            }
            else
            {
                GameObject efect = Instantiate<GameObject>(effecter.ReturnEffect(EffectType.misc));
                efect.transform.position = hit.point;
                efect.transform.LookAt(efect.transform.position + hit.normal);
                efect.transform.parent = hit.transform;

            }
        }
    }

    public bool CanReload()
    {
        return false;
    }
}
