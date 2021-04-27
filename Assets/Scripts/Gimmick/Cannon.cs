using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{

    public GameObject Obj_Cannon;
    public GameObject Player;

    //bulletプレハブ
    public GameObject bullet;
    //弾が生成されるポジションを保有するゲームオブジェクト
    public GameObject bulletPos;
    //弾丸のスピード
    public float speed = 1500f;
    //敵生成時間間隔
    public float interval = 0.5f;
    //経過時間
    private float time = 0f;
    //Rayの長さ
    private float maxDistance = 100;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var aim = this.Player.transform.position - this.transform.position;
        var look = Quaternion.LookRotation(aim);
        this.transform.localRotation = look;

        //Cannonからプレイヤーに向かってRayを飛ばす
        Ray ray = new Ray(Obj_Cannon.transform.position, Obj_Cannon.transform.forward);
        RaycastHit hit;


        // 文字列からレイヤーマスクを作る
        int mask = LayerMask.GetMask(new string[] { "Player", "Mebiusu" });


        ////Physics.Raycast（発射位置、Rayの方向、衝突したオブジェクト情報、Rayの長さ）
        //Physics.Raycast(ray, out hit, maxDistance);

        if (Physics.Raycast(ray, out hit, maxDistance, mask)) 
        {

            if (time > interval)
            {
                

                //ballをインスタンス化して発射
                GameObject createdBullet = Instantiate(bullet) as GameObject;
                createdBullet.transform.position = bulletPos.transform.position;
                //発射ベクトル
                Vector3 force;
                //発射の向きと速度を決定
                force = bulletPos.transform.forward * speed;
                // Rigidbodyに力を加えて発射
                createdBullet.GetComponent<Rigidbody>().AddForce(force);
                //時間リセット
                time = 0;

            }
        }
            

        time += Time.deltaTime;


        //Rayを画面に表示
        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.green, 0, true);


    }


}
