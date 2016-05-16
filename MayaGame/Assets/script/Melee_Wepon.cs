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
    public LayerMask wepMask;

    bool ADS;
    bool reload;
    bool sear;
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
        Debug.Log(magPos);
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

    void ShootHit(Ray ray, float rayRange, int penetrateNum)
    {
        RaycastHit hit;
        LayerMask mask = ~(1 << 2 | 1 << 8);
        if (Physics.Raycast(ray, out hit, rayRange, wepMask) && penetrateNum < 3)
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

                DamageParameter newDam = dam.multiple(1 - penetrateNum * 0.4f);
                Vector3 penetratePoint = hitM.HitDamage(newDam, hit, ray);

                FPSCon.CmdSendHP(hitM.transform.root.name, hitM.name, hitM.hitPoint);
                if (penetratePoint != Vector3.zero)
                {

                    Ray newRay = new Ray(penetratePoint, ray.direction);
                    ShootHit(newRay, rayRange - hit.distance, ++penetrateNum);
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
}
