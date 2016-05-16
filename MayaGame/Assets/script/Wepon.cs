using UnityEngine;
using System.Collections.Generic;
using Exploder;

public interface WeponInterface
{
    void Primaly();
    void Secondary();
    void Reload();
    void ReturnPrimaly();
    float ReturnChangeSpeed(float changeAnimLength);
    void Setup(int slot);
    void CantAttack();
    void CanAttack();
    void ShotEffect();
    void SendUI();
}

[System.Serializable]
public class GunParameter : System.Object
{

    public float accuracy = 0;
    public float recoil = 0;
    public float rate = 0;
    public float range = 0;
    public float mobility = 0;
    public float reload = 0;
    public int magazine = 0;
    public int totalAmmo = 0;

}

[System.Serializable]
public class DamageParameter : System.Object
{
    public float damage = 0;
    public float shock = 0;
    public float penetration = 0;
    public float heat = 0;

    public DamageParameter(float dam,float sho,float pen,float he)
    {
        damage = dam;
        shock = sho;
        penetration = pen;
        heat = he;
    }

    public DamageParameter multiple(float rate)
    {
        return new DamageParameter(damage * rate, shock * rate, penetration * rate, heat * rate);
    }
}

    public enum Selector
{
    full,
    semi,
    burst
}

public class Wepon : MonoBehaviour, WeponInterface
{


    Camera myCamera;
    public Animator anim;
    public Animator weponAnim;
    public Transform camPosition;
    public Transform ADSPosition;
    public Transform magazineTr;
    public Transform muzuleTr;
    public float ADSTime = 0.3F;
    public FPSController FPSCon;
    public GameObject hitEffect;
    public AudioSource shotSound;
    public ParticleSystem muzuleFlash;
    public ParticleSystem yakkyou;
    [SerializeField]
    public GunParameter pa;
    public DamageParameter dam;
    public float ADSRate = 1f;
    public AnimationClip idleAnim;
    public AnimationClip reloadAnim;
    public AnimationClip cockAnim;
    public AnimationClip runAnim;
    public bool noCock;
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
    public Dictionary<string, float> parameters = new Dictionary<string, float>(){
        {"accuracy",0 },
        {"recoil",0},
        {"rate" ,0},
        {"range",0},
        {"mobility",0},
        {"reload",0},
        {"magazine",0 },
        {"totalAmmo",0 }
    };
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
        camPosition = FPSCon.camPosition;
        FPSCon.ADSPosition = ADSPosition;
        //FPSCon.useWepon = this;
        anim = FPSCon.anim;
        parameters["accuracy"] = pa.accuracy;
        parameters["recoil"] = pa.recoil;
        parameters["rate"] = pa.rate;
        parameters["range"] = pa.range;
        parameters["reload"] = pa.reload;
        parameters["mobility"] = pa.mobility;
        parameters["magazine"] = pa.magazine;
        parameters["totalAmmo"] = pa.totalAmmo;
        if (first)
        {
            magPos = magazineTr.localPosition;
            magRot = magazineTr.localRotation;
            magazine = (int)parameters["magazine"];
            first = false;
        }
        else
        {
            MagReturn();
        }
        Exploder = FindObjectOfType<ExploderObject>();
        AnimatorOverrideController newAnime = new AnimatorOverrideController();
        newAnime.runtimeAnimatorController = anim.runtimeAnimatorController;
       /* for (int b = 0; b < newAnime.animationClips.Length; b++)
        {
            Debug.Log(newAnime.animationClips[b].name);
        }*/
        reloadAnimRate = reloadAnim.length / (parameters["reload"] * 0.01f);
        // Debug.Log(reloadAnimRate);
        anim.SetFloat("ReloadSpeed", reloadAnimRate);
        newAnime[anim.runtimeAnimatorController.animationClips[2].name] = idleAnim;
        newAnime[anim.runtimeAnimatorController.animationClips[3].name] = reloadAnim;
        newAnime[anim.runtimeAnimatorController.animationClips[4].name] = cockAnim;
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


        if (selector == Selector.burst && sear && burst >= 0)
        {
            Primaly();
        }

        if (weponAnim != null)
        {
            weponAnim.SetInteger("ammo", magazine);
        }


        FPSCon.mouseVec += recoilDmp;
        FPSCon.recoilVec = recoilDmp;
        recoilDmp = Vector3.Slerp(recoilDmp, Vector3.zero, 5 * Time.deltaTime);
        recoilOffset = Mathf.Lerp(recoilOffset, 0, 5 * Time.deltaTime);

        
    }
    void LateUpdate()
    {
        myCamera.transform.position = myCamera.transform.position + myCamera.transform.rotation * new Vector3(0, 0, recoilOffset);
        if (reload || sear)
        {
            StopShot();
        }
        if (!FPSCon.isLocalPlayer)
        {
            StopShot();
        }
    }

    public void ReturnPrimaly()
    {
        if (selector == Selector.full)
        {
            StopShot();
        }
        if (selector == Selector.semi)
        {
            sear = false;
        }
        if (selector == Selector.burst && burst <= 0)
        {
            sear = false;
            burst = 3;
        }

    }
    public void Primaly()
    {
        if (rpmTimer > Time.time || magazine <= 0 || FPSCon.run || reload || burstTimer > Time.time)
        {
            if (magazine <= 0) StopShot();
            return;
        }
        if (sear)
        {
            if (selector != Selector.burst || burst <= 0)
            {
                return;
            }
        }


        ShotEffect();
        FPSCon.CmdShot();
        float accuracy = (parameters["accuracy"] * 0.01f);
        Vector3 randomCone = ( myCamera.transform.forward);
        Vector3 move = FPSCon.moveVec;
        move.y = 0;
        accuracy *= 1f + (move.magnitude / FPSCon.moveSpeed);

        
        if (FPSCon.ADS) { accuracy *= 0.14f; }
        accuracy = Mathf.Clamp(accuracy, 0.001f, 10f);
        //Debug.Log(accuracy);
        randomCone = Quaternion.Euler(new Vector3(Random.Range(-accuracy, accuracy), Random.Range(-accuracy, accuracy), Random.Range(-accuracy, accuracy))) * randomCone;
        
        Ray ray = new Ray(myCamera.transform.position, randomCone);
        ShootHit(ray, parameters["range"],0);

        //muzuleFlash.emissionRate = parameters["rate"] / 60f;       		
        float recoil = (parameters["recoil"] / 100f);
        if (FPSCon.crouch) { recoil *= 0.75f; }
        recoilDmp = new Vector3(-recoil, recoil * Mathf.Sign(Random.Range(-1, 1)), 0) * 0.01f;//recoil
        recoilOffset = recoil * 0.025f;
        FPSCon.shakeVec = new Vector3(0, 0, recoilOffset);

        rpmTimer = Time.time + (60f / parameters["rate"]);
        magazine--;
        pa.totalAmmo--;
        if (selector == Selector.semi)
        {
            sear = true;
        }
        if (selector == Selector.burst)
        {
            burst--;
            if (burst <= 0)
            {
                burstTimer = Time.time + burstDelay;
            }
            sear = true;
        }

        if (weponAnim != null)
        {

            weponAnim.SetTrigger("shot");
        }
        SendUI();

    }

    public void Secondary()
    {
        if (!reload)
        {
            FPSCon.ADS = !FPSCon.ADS;
            FPSCon.ADSTimer = FPSCon.ADSTime;
            if (FPSCon.ADS) { myCamera.fieldOfView /= ADSRate; }
            else { myCamera.fieldOfView *= ADSRate; }
        }
    }

    public void Reload()
    {
        if (FPSCon.ADS)
        {
            FPSCon.ADSTimer = FPSCon.ADSTime;
            myCamera.fov *= ADSRate;
        }
        FPSCon.ADS = false;


        if (magazine <= 0)
        {
            anim.SetBool("Cock", true);
        }


        reload = true;
        burst = 3;
        sear = false;

    }

    void MagDetach()
    {
        magazineTr.SetParent(FPSCon.handPosL);
    }

    public void MagReturn()
    {
        magazineTr.SetParent(transform);
        magazineTr.localRotation = magRot;
        magazineTr.localPosition = magPos;
    }

    void ReloadEnd()
    {
        MagReturn();
        if (magazine > 0 || noCock)
        {
            if (pa.magazine <= pa.totalAmmo)
            {
                magazine = (int)parameters["magazine"];
            }
            else
            {
                magazine = pa.totalAmmo;
            }
            anim.SetBool("Cock", false);
            reload = false;
            FPSCon.reload = false;
            SendUI();
        }
        else
        {
            anim.SetBool("Cock", true);
        }
    }

    void CockEnd()
    {
        if (pa.magazine <= pa.totalAmmo)
        {
            magazine = (int)parameters["magazine"];
        }
        else
        {
            magazine = pa.totalAmmo;
        }
        anim.SetBool("Cock", false);
        reload = false;
        FPSCon.reload = false;
        SendUI();

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
        MagReturn();
        if (FPSCon.ADS)
        {
            FPSCon.ADSTimer = FPSCon.ADSTime;
            myCamera.fov *= ADSRate;
        }
        magazineTr.SetParent(transform);
        magazineTr.localRotation = magRot;
        magazineTr.localPosition = magPos;
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
        if (!yakkyou.IsAlive())
        {
            //Debug.Log("dead");
            yakkyou.enableEmission = true;
        }
        yakkyou.Emit(1);
        shotSound.Play();
    }

    public void SendUI()
    {
        if (FPSCon.isClient && FPSCon.UICon!=null)
        {
            string name = gameObject.name;
            name = name.Replace("(Clone)", "");
            FPSCon.UICon.SetWeponText(mySlot, name, magazine.ToString(), pa.totalAmmo.ToString());
            //Debug.Log("UISet");
        }
        //Debug.Log("UI");
    }

    void ShootHit(Ray ray,float rayRange,int penetrateNum)
    {
        RaycastHit hit;
        LayerMask mask = ~(1<<2 | 1 << 8);
        if (Physics.Raycast(ray, out hit, rayRange,wepMask) && penetrateNum<3)
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

                DamageParameter newDam = dam.multiple(1-penetrateNum*0.4f);
                Vector3 penetratePoint = hitM.HitDamage(newDam, hit, ray);
                
                FPSCon.CmdSendHP(hitM.transform.root.name,hitM.name,hitM.hitPoint);
                if (penetratePoint != Vector3.zero)
                {
                    
                    Ray newRay = new Ray(penetratePoint, ray.direction);
                    ShootHit(newRay,rayRange-hit.distance, ++penetrateNum);
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
