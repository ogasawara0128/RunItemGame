using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int enemyLife = 3; //敵の体力
    public GameObject enemyBody; //見た目のオブジェクトの情報

    public float freezeTime = 1.0f;//無敵時間の秒数
    Coroutine damageStart;//ダメージ中のコルーチンを参照する変数

    public GameObject effectObject; //撃破時のエフェクト

    void Update()
    {
        //もしもダメージコルーチンがnullでなければ(ダメージ中)
        if (damageStart != null)
        {
            //valが一定周期で正か負か、値を決める
            float val = Mathf.Sin(Time.time * 50); //三角関数と時間
            if (val > 0)
            {
                //見た目のMeshRendererをON
                enemyBody.GetComponent<MeshRenderer>().enabled = true;
            }
            else
            {
                //見た目のMeshRendererをOFF
                enemyBody.GetComponent<MeshRenderer>().enabled = false;
            }

            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //ダメージコルーチンが発動してなければ(ダメージ中でなければ)
        if (damageStart == null)
        {
            //ぶつかった相手がBulletなら
            if (other.gameObject.tag == "Bullet")
            {
                enemyLife--; //体力1減る
                if (enemyLife <=0)
                {
                    CreateEffect();
                    Destroy(gameObject); //消滅
                }
                else //体力残っていれば
                {
                    damageStart = StartCoroutine(DamageStart()); //コルーチン発動
                }
            }
            //ぶつかった相手がSlashなら即撃破
            else if (other.gameObject.tag == "Slash")
            {
                CreateEffect();
                Destroy(gameObject);
            }
        }
    }

    //時間差でダメージ状態を解くコルーチン
    IEnumerator DamageStart()
    {
        //freezeTime待ってから、ダメージコルーチン変数を解放
        yield return new WaitForSeconds(freezeTime);
        damageStart = null;

        //見た目のMeshRendererをON
        enemyBody.GetComponent<MeshRenderer>().enabled = true;
    }

    void CreateEffect()
    {
        Instantiate(effectObject,transform.position,Quaternion.identity);
    }
}
