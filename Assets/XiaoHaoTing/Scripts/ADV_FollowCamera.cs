using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADV_FollowCamera : MonoBehaviour
{
    //public Transform playerTransform;
    //public Vector3 deviation;

    //void Start()
    //{
    //    deviation = transform.position - playerTransform.position;
    //}

    //private void Update()
    //{
    //    transform.position = playerTransform.position + deviation;
    //}



    public GameObject player;   //プレイヤー情報格納用
    Vector3 offset;      //相対距離取得用
    public float smoothing = 5f;
    // Use this for initialization
    void Start()
    {

        //playerの情報を取得
        this.player = GameObject.Find("LisaPrefab");

        // MainCamera(自分自身)とplayerとの相対距離を求める
        offset = transform.position - player.transform.position;

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //新しいトランスフォームの値を代入する
        transform.position = player.transform.position + offset;

        //playerの向きと同じようにカメラの向きを変更する
        //transform.rotation = player.transform.rotation;
        transform.position = Vector3.Lerp(transform.position, player.transform.position,smoothing * Time.deltaTime);
    }
}
