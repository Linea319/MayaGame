using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class FPS_UI : MonoBehaviour {

    public FPSController FPSCon;
    public Text primalyText;
    public Text secondalyText;
    public Text grenadeText;
    public Text specialText;
    public GameObject message;
    public Text MessageText;
    public Image MessageProgress;
    public Image crossHair;
    public Image throttle;
    public Image pitchBar;
    public Text pitchText;
    public Image yawBar;
    public Image hpBar;
    public Image armorBar;
    public Text taskText;
    public Image ItemBar;
    public Text[] taskInfo; 

    public RectTransform friendPanel;
    public GameObject friendPrefab;

    //Color change
    public Material UImat;
    public Color defColor;
    public Color hitColor;

    float crosshairScale;
    float throttleScale;
    Vector3 pitchDef;
    float pitchLimit;
    Vector3 yawDef;
    float yawRate;
    UIMessenger messager;

    // Use this for initialization
    void Start () {
        //UImat.SetColor("Color", defColor);
        UImat.color = defColor;
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

        if(messager != null)
        {
            MessageText.text = messager.messagaeText;
            MessageProgress.fillAmount = messager.progress/ messager.progressTime;
        }
        else
        {
            MessageText.text = null;
            MessageProgress.fillAmount = 0;
        }
        if(MessageProgress.fillAmount >= 1 && messager.usePlayer)
        {
            FPSCon.CmdTargetResulect(messager.name);
        }
        if (MessageProgress.fillAmount >= 1 && !messager.usePlayer && !messager.noAuth)
        {
            if(messager.methodObj != null)
            {
                FPSCon.CmdMessagerMethod(messager.methodObj.gameObject, messager.compMethhod,messager.useTarget);
            }
            else
            {
                FPSCon.CmdMessagerMethod(messager.gameObject, messager.compMethhod, messager.useTarget);
            }
            messager.SetProgress(false);
        }
        if (MessageProgress.fillAmount >= 1 && messager.useTarget && messager.noAuth)
        {
            if (messager.methodObj != null)
            {
                messager.methodObj.SendMessage( messager.compMethhod,FPSCon.transform);
            }
            else
            {
                messager.SendMessage(messager.compMethhod, FPSCon.transform);
            }
            messager.SetProgress(false);
        }
        if (MessageProgress.fillAmount >= 1 && !messager.useTarget && messager.noAuth)
        {
            if (messager.methodObj != null)
            {
                messager.methodObj.SendMessage(messager.compMethhod);
            }
            else
            {
                messager.SendMessage(messager.compMethhod);
            }
            messager.SetProgress(false);
        }


        yawBar.rectTransform.localPosition = yawDef + new Vector3(yawRate*yawAngle, 0, 0);
        //13.3f

        hpBar.fillAmount = FPSCon.hpMng.hitPoint / FPSCon.hpMng.maxHP;
        armorBar.fillAmount = FPSCon.hpMng.armor / FPSCon.hpMng.maxArmor;

        ItemBar.fillAmount = FPSCon.itemGage;

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
                grenadeText.text =  name + " " + mag+" ";
                break;
            case 3:
                specialText.text =  name + " " + mag+" ";
                break;
        }
    }

    public void SetMessageText(UIMessenger messageTarget)
    {
        messager = messageTarget;
        
    }

    public void SetTaskText(string text)
    {
        Debug.Log(text);
        taskText.text = text;
    }

    public IEnumerator HitColor()
    {
        UImat.color = hitColor;
        yield return new WaitForSeconds(0.5f);
        UImat.color = defColor;
    } 

    public void SetFriend()
    {
        
        GameObject[] playersObj = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log("Friend: "+ playersObj.Length);
        for (int j = 0; j < playersObj.Length; j++)
        {
            FPSController con = playersObj[j].GetComponent<FPSController>();
            if (!con.isLocalPlayer)
            {
                GameObject panel = Instantiate(friendPrefab) as GameObject;
                panel.GetComponent<RectTransform>().SetParent(friendPanel,false);
                FriendUI ui = panel.GetComponent<FriendUI>();
                ui.player = con;
                ui.playerHp = con.hpMng;
                ui.pname.text = con.playerName;
            }
        }
    }

    public void SetTaskInfo(string[] message)
    {
        //Debug.Log("taskWrite");
        for (int i = 0; i < message.Length; i++)
        {
            taskInfo[i].text = message[i];
        }
    }

    public void SetTaskInfoSingle(string message,int num)
    {
            taskInfo[num].text = message;
    }
}
