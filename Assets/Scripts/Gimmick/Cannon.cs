using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{

    GameObject player;

    //bulletプレハブ
    public GameObject bullet;

    //弾丸のスピード
    [SerializeField]
    float speed = 150f;
    //敵生成時間間隔
    [SerializeField]
    float interval = 0.5f;
    //経過時間
    private float time = 0f;
    //Rayの長さ
    [SerializeField]
    float maxDistance = 100;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {


            var aim = player.transform.position - transform.position;

            //Cannonからプレイヤーに向かってRayを飛ばす
            Ray ray = new Ray(transform.position, aim.normalized);
            RaycastHit hit;


            // 文字列からレイヤーマスクを作る
            int mask = LayerMask.GetMask(new string[] { "Player", "Mebiusu" });


            ////Physics.Raycast（発射位置、Rayの方向、衝突したオブジェクト情報、Rayの長さ）

            if (Physics.Raycast(ray, out hit, maxDistance, mask) && hit.transform.CompareTag("Player")) {

                transform.LookAt(player.transform);

                if (time > interval) {


                    //ballをインスタンス化して発射
                    GameObject createdBullet = Instantiate(bullet, transform.parent);
                    createdBullet.transform.position = transform.position+ aim.normalized;
                    //発射ベクトル
                    Vector3 force;
                    //発射の向きと速度を決定
                    force = aim.normalized * speed;
                    // Rigidbodyに力を加えて発射
                    createdBullet.GetComponent<Rigidbody>().AddForce(force);
                    //時間リセット
                    time = 0;

                }
                time += Time.deltaTime;
            }




            //Rayを画面に表示
            Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.green, 0, true);

    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}
