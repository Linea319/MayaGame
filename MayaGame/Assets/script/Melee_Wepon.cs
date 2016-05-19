using UnityEngine;
using System.Collections.Generic;
using Exploder;


[System.Serializable]
public class MeleeParameter : System.Object
{
    public float chargeTime = 1f;
    public float chargeDamageRaate = 1.5f;
    public float useStamina;
    public float mobility = 0;
}

public class Melee_Wepon : MonoBehaviour, WeponInterface {

    Camera myCamera;
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
        newAnime.runtimeAnimatorController = anim.runtimeAnimatorController;
        /* for (int b = 0; b < newAnime.animationClips.Length; b++)
         {
             Debug.Log(newAnime.animationClips[b].name);
         }*/
        // Debug.Log(reloadAnimRate);
        anim.SetFloat("ReloadSpeed", reloadAnimRate);
        newAnime[anim.runtimeAnimatorController.animationClips[2].name] = idleAnim;
        newAnime[anim.runtimeAnimatorController.animationClips[8].name] = chargeAnim;
        newAnime[anim.runtimeAnimatorController.animationClips[7].name] = attackAnim;
        newAnime[anim.runtimeAnimatorController.animationClips[6].name] = runAnim;
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


    }
    void LateUpdate()
    {

    }

    public void ReturnPrimaly()
    {
        if (sear)
        {
            anim.SetTrigger("MeleeAttack");
            weponAnim.SetBool("Charge", false);
            sear = false;
            attackNow = true;
            canHit = true;
            WeponCollieder.enabled = true;
        }

    }
    public void Primaly()
    {
        if (!sear)
        {
            anim.SetTrigger("Melee");
            weponAnim.SetBool("Charge", true);
            sear = true;
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
            if (hitM != null)
            {
                
                GameObject efect = Instantiate<GameObject>(effecter.ReturnEffect(hitM.effectType));
                efect.transform.position = col.ClosestPointOnBounds(WeponCollieder.bounds.center);
                efect.transform.LookAt(WeponCollieder.bounds.center);
                efect.transform.parent = col.transform;

                DamageParameter newDam = dam;
                Ray ray = new Ray(WeponCollieder.bounds.center, (col.bounds.center - WeponCollieder.bounds.center));
                RaycastHit hit;
                if (col.Raycast(ray, out hit, 10f))
                {
                    Vector3 penetratePoint = hitM.HitDamage(newDam, hit, ray);

                    FPSCon.CmdSendHP(hitM.transform.root.name, hitM.name, hitM.hitPoint);
                    if (penetratePoint != Vector3.zero)
                    {
                        //Debug.Log("penetration");
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
}
