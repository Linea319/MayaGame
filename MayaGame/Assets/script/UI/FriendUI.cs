using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FriendUI : MonoBehaviour {
    public FPSController player;
    public HitManagerPlayer playerHp;
    public Text pname;
    public Image armor;
    public Image hp;
    public Image prim;
    public Image sub;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (player == null || playerHp == null) Destroy(this);
        armor.fillAmount = playerHp.armor / playerHp.maxArmor;
        hp.fillAmount = playerHp.hitPoint / playerHp.maxHP;
        prim.fillAmount = player.primammo;
        sub.fillAmount = player.subammo;
	}
}
