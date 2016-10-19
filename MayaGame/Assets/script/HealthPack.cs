using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class HealthPack : NetworkBehaviour {
    public Renderer[] rend = new Renderer[2];
    Material[] mat = new Material[2];
    [ColorUsage(false, true, 0, 8, 0.125f, 3)]
    public Color fullColor;
    [ColorUsage(false, true, 0, 8, 0.125f, 3)]
    public Color emptyColor;
    [SyncVar]
    float percent = 100;
    // Use this for initialization
    void Start() {
        for (int i = 0; i < rend.Length; i++)
        {
            mat[i] = rend[i].material;
        }
    }

    // Update is called once per frame
    [ServerCallback]
    void Update() {
        Color newColor = Color.Lerp(emptyColor, fullColor, percent * 0.01f);
        for (int i = 0; i < mat.Length; i++)
        {
            mat[i].SetColor("_EmissionColor", newColor);
        }
        if (percent <= 0)
        {
            NetworkServer.Destroy(gameObject);
        }
    }

    public void Resuply(Transform player)
    {
        HitManagerPlayer con = player.GetComponent<HitManagerPlayer>();

        if (con.hitPoint < con.maxHP){
            con.CmdHeal();
            percent -= 30f;
            }
    }

    [Command]
    void CmdSetPercentage(float newPercent)
    {
        percent = newPercent;
    }
}
