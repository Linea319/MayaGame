using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ParamTextSetter : MonoBehaviour {
    public Text[] texts;
    GunParameter gunP;
    
	
    public void SetGunParam(string name,GunParameter param,DamageParameter dam)
    {
        texts[0].text = "NAME:"+name.Replace("(Clone)","");
        texts[1].text = "SHOCK:" + ((int)dam.shock).ToString("d3");
        texts[2].text = "PENETRATE:" + ((int)dam.penetration).ToString("d3");
        texts[3].text = "HEAT:" + ((int)dam.heat).ToString("d3");
        texts[4].text = "ACU:" + ((int)param.accuracy).ToString("d3");
        texts[5].text = "STA:" + ((int)param.recoil).ToString("d3");
        texts[6].text = "RAN:" + ((int)param.range).ToString("d3");
        texts[7].text = "MOB:" + ((int)param.mobility).ToString("d3");
        texts[8].text = "REL:" + ((int)param.reload).ToString("d3");
        texts[9].text = "ROF:" + ((int)param.rate).ToString("d3");
        texts[10].text = "MAG:" + param.magazine.ToString("d3");
        texts[11].text = "AMM:" + ((int)param.totalAmmo).ToString("d3");
        gunP = param;
    }

    public void SetMeleeParam(string name, MeleeParameter param, DamageParameter dam)
    {
        texts[0].text = "NAME:" + name.Replace("(Clone)", "");
        texts[1].text = "SHOCK:" + ((int)dam.shock).ToString("d3");
        texts[2].text = "PENETRATE:" + ((int)dam.penetration).ToString("d3");
        texts[3].text = "HEAT:" + ((int)dam.heat).ToString("d3");
        texts[4].text = "TIME:" + ((int)param.chargeTime).ToString("d3");
        texts[5].text = "CHARGE:" + ((int)param.chargeDamageRaate).ToString("d3");
        texts[6].text = "STAMINA:" + ((int)param.useStamina).ToString("d3");
        texts[7].text = "MOB:" + ((int)param.mobility).ToString("d3");
        texts[8].text = "RAN:" + ((int)param.range).ToString("d3");
    }

    public void SetHosei(GunParameter hosei)
    {
        GunParameter nParam = new GunParameter();
        nParam.Add(hosei);
        nParam.Add(gunP);
        texts[4].text = "ACU:" + ((int)nParam.accuracy).ToString("d3")+"("+ ((int)hosei.accuracy).ToString()+")";
        texts[5].text = "STA:" + ((int)nParam.recoil).ToString("d3") + "(" + ((int)hosei.recoil).ToString() + ")";
        texts[6].text = "RAN:" + ((int)nParam.range).ToString("d3") + "(" + ((int)hosei.range).ToString() + ")";
        texts[7].text = "MOB:" + ((int)nParam.mobility).ToString("d3") + "(" + ((int)hosei.mobility).ToString() + ")";
        texts[8].text = "REL:" + ((int)nParam.reload).ToString("d3") + "(" + ((int)hosei.reload).ToString() + ")";
        texts[9].text = "ROF:" + ((int)nParam.rate).ToString("d3") + "(" + ((int)hosei.rate).ToString() + ")";
        texts[10].text = "MAG:" + nParam.magazine.ToString("d3") + "(" + (hosei.magazine).ToString() + ")";
        texts[11].text = "AMM:" + (nParam.totalAmmo).ToString("d3") + "(" + (hosei.totalAmmo).ToString() + ")";

    }

    public void ResetHosei()
    {
        texts[4].text = "ACU:" + ((int)gunP.accuracy).ToString("d3");
        texts[5].text = "STA:" + ((int)gunP.recoil).ToString("d3");
        texts[6].text = "RAN:" + ((int)gunP.range).ToString("d3");
        texts[7].text = "MOB:" + ((int)gunP.mobility).ToString("d3");
        texts[8].text = "REL:" + ((int)gunP.reload).ToString("d3");
        texts[9].text = "ROF:" + ((int)gunP.rate).ToString("d3");
        texts[10].text = "MAG:" + gunP.magazine.ToString("d3");
        texts[11].text = "AMM:" + (gunP.totalAmmo).ToString("d3");
    }

}
