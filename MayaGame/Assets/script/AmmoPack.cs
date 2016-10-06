using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class AmmoPack : NetworkBehaviour {

    [SyncVar]
    public float Percent = 100;
    [ColorUsage(false, true, 0, 8, 0.125f, 3)]
    public Color fullColor;
    [ColorUsage(false, true, 0, 8, 0.125f, 3)]
    public Color emptyColor;
    public Renderer[] rend = new Renderer[3];
    Material[] mat = new Material[3];

    // Use this for initialization
    void Start () {
        for(int i = 0; i < rend.Length; i++)
        {
            mat[i] = rend[i].material;
        }
        
	}
	
	// Update is called once per frame
    [ServerCallback]
	void Update () {

        Color newColor = Color.Lerp(emptyColor,fullColor,Percent*0.01f);
        for (int i = 0; i < mat.Length; i++)
        {
            mat[i].SetColor("_EmissionColor", newColor);
        }

        if (Percent <= 0)
        {
            NetworkServer.Destroy(gameObject);
        }
	}

    public void Resuply(Transform player)
    {
        FPSController con = player.GetComponent<FPSController>();
        Wepon wep = con.wepons[0].GetComponent<Wepon>();
        ResuplyWepon(wep);
        wep.SendUI();
        Wepon wep2 = con.wepons[1].GetComponent<Wepon>();
        ResuplyWepon(wep2);
        wep2.SendUI();
    }

    void ResuplyWepon(Wepon wep)
    {
        if (wep == null) return;
        float ammoRate = 1f - (float)wep.pa.totalAmmo / (float)wep.maxAmmo;
        if (Percent > 12.5f * ammoRate)
        {
            wep.pa.totalAmmo = wep.maxAmmo;
            Percent -= 12.5f * ammoRate;
        }
        else
        {
            wep.pa.totalAmmo += (int)(wep.maxAmmo * (Percent / 12.5f)* ammoRate);
            Percent =0;
        }

        CmdSetPercentage(Percent);
    }

    [Command]
    void CmdSetPercentage(float newPercent)
    {
        Percent = newPercent;
    }
}
