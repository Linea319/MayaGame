using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class HitManagerPlayer :NetworkBehaviour{
    [SyncVar] public float hitPoint = 100f;
    [SyncVar] public float armor = 100f;
    public float rechargeDelay = 3f;
    public float rechargeRate = 50f;
    float rechargeTimer=0f;
    [HideInInspector]public  float maxArmor;
    [HideInInspector]public float maxHP;
    UIMessenger messenger;
    FPSController controller;

	// Use this for initialization
	void Start () {
        Initialize();
	}

    void Initialize()
    {
        maxArmor = armor;
        maxHP = hitPoint;
        if (controller == null)
        {
            controller = GetComponent<FPSController>();
        }
        if (messenger == null)
        {
            messenger = GetComponent<UIMessenger>();
        }
    }
	
	// Update is called once per frame
    [ServerCallback]
	void Update () {
        if (!controller.isLocalPlayer) return;
	if(Time.time > rechargeTimer)
        {
            armor += rechargeRate * Time.deltaTime;
            armor = Mathf.Clamp(armor, 0, maxArmor);
        }

    if(hitPoint <= 0 && !controller.dead)
        {
            Death();
            controller.CmdPlayerDeath();
        }
	}

    public void HitDamage(float damage)
    {
        if (!controller.isLocalPlayer) return;
        //Debug.Log("hit:"+damage);
        CmdSetDamage(damage);
        //rechargeTimer = Time.time + rechargeDelay;
        StartCoroutine_Auto( controller.UICon.HitColor());

    }

    [Command]
    void CmdSetDamage(float damage)
    {
        if (armor <= damage)
        {
            hitPoint -= (damage - armor);
            armor = 0;

        }
        else
        {
            armor -= damage;
        }
        rechargeTimer = Time.time + rechargeDelay;
    }


    public void Death()
    {
        controller.dead = true;
        messenger.enabled = true;
        Camera.main.fieldOfView = 70;
    }

    [Client]
    public void Resulect()
    {
        armor = maxArmor;
        hitPoint = maxHP;
        controller.dead = false;
        messenger.SetProgress(false);
        messenger.enabled = false;
    }

    [Command]
    public void CmdResulect()
    {
        armor = maxArmor;
        hitPoint = maxHP;
        controller.dead = false;
        messenger.SetProgress(false);
        messenger.enabled = false;
        RpcResulect();
    }

    [ClientRpc]
    public void RpcResulect()
    {
        armor = maxArmor;
        hitPoint = maxHP;
        controller.dead = false;
        messenger.SetProgress(false);
        messenger.enabled = false;
    }

    [Command]
    public void CmdHeal()
    {
            hitPoint = maxHP;
    }
}
