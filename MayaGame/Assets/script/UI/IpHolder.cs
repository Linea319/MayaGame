using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IpHolder : MonoBehaviour {

    string IP;
    public InputField IPField;

    public void SaveIP()
    {
        IP = IPField.text;
        PlayerPrefs.SetString("recentIP", IP);
    }

    public void LoadIP()
    {
        IP = PlayerPrefs.GetString("recentIP");
        IPField.text = IP;
    }
}
