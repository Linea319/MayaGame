using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class SyncPosition : NetworkBehaviour {

    [SyncVar]   //ホストから全クライアントへ送られる位置
    private Vector3 syncPos;
    [SyncVar]   //ホストから全クライアントへ送られる回転
    private Quaternion syncRot;
    //Playerの現在位置
    [SerializeField]
    Transform myTransform;
    //Lerp: ２ベクトル間を補間する
    [SerializeField]
    float lerpRate = 15;

    void FixedUpdate()
    {
        TransmitPosition();
        LerpPosition(); //2点間を補間する
    }

    //ポジション補間用メソッド
    void LerpPosition()
    {
        //補間対象は相手プレイヤーのみ
        if (isClient)
        {
            //Lerp(from, to, 割合) from〜toのベクトル間を補間する
            myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
            myTransform.rotation = Quaternion.Lerp(myTransform.rotation, syncRot, Time.deltaTime * lerpRate);
        }
    }
    //クライアントからホストへ、Position情報を送る
   

    //クライアントのみ実行される
    [ServerCallback]
    //位置情報を送るメソッド
    void TransmitPosition()
    {
        syncPos = myTransform.position;
        syncRot = myTransform.rotation;
    }
}
