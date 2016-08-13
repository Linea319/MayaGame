using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FPS_UI : MonoBehaviour {

    public FPSController FPSCon;
    public Text primalyText;
    public Text secondalyText;
    public Text grenadeText;
    public Text specialText;
    public Image crossHair;
    public Image throttle;
    public Image pitchBar;
    public Text pitchText;
    public Image yawBar;
    public Image hpBar;
    public Image armorBar;

    float crosshairScale;
    float throttleScale;
    Vector3 pitchDef;
    float pitchLimit;
    Vector3 yawDef;
    float yawRate;

    // Use this for initialization
    void Start () {
        pitchDef = pitchBar.rectTransform.localPosition;
        pitchLimit = pitchBar.rectTransform.sizeDelta.y*2f;
        Debug.Log(pitchLimit);
        yawDef = yawBar.rectTransform.localPosition;
        yawRate = yawBar.rectTransform.rect.width/(540f+30f);
        Debug.Log(yawRate);
    }
	
	// Update is called once per frame
	void Update () {
        crosshairScale =  Mathf.Lerp(crosshairScale, Mathf.Clamp(FPSCon.speed/FPSCon.moveSpeed,0.25f,1f),Time.deltaTime*10f);
        crossHair.transform.localScale = Vector3.one * crosshairScale;

        crossHair.enabled = !FPSCon.ADS;

       // throttleScale = Mathf.Lerp(throttleScale, (FPSCon.speed / FPSCon.runSpeed)*0.8f+Random.value*0.1f, Time.deltaTime * 10f);
        throttle.fillAmount = (FPSCon.stamina / 100f);
        float pitchAngle = FPSCon.chara.transform.localEulerAngles.x;
        if (pitchAngle > 180)
        {
            pitchAngle -= 360;
        }
        pitchBar.rectTransform.localPosition = pitchDef + new Vector3(0, (pitchLimit / 94f)*pitchAngle, 0);
        pitchText.text = (-pitchAngle).ToString("F0");
        float yawAngle = FPSCon.transform.eulerAngles.y;
        if (yawAngle > 180)
        {
            yawAngle -= 360;
        }
        yawBar.rectTransform.localPosition = yawDef + new Vector3(yawRate*yawAngle, 0, 0);
        //13.3f

        hpBar.fillAmount = FPSCon.hpMng.hitPoint / FPSCon.hpMng.maxHP;
        armorBar.fillAmount = FPSCon.hpMng.armor / FPSCon.hpMng.maxArmor;

    }

    public void SetWeponText(int slot,string name,string mag,string total="")
    {
        switch (slot)
        {
            case 0:
                primalyText.text = "P:" + name + " " + mag + " / " + total;
                break;
            case 1:
                secondalyText.text = "S:" + name + " " + mag + " / " + total;
                break;
            case 2:
                grenadeText.text =  name + " " + mag;
                break;
            case 3:
                specialText.text =  name + " " + mag + " / " + total;
                break;
        }
    }
}
