using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 diff; //対象とカメラの維持するべき距離

    //public GameObject target; //カメラの対象(Player)
    GameObject player;
    public float followSpeed = 5.0f; //追従スピード

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //diff = target.transform.position - transform.position; //最初に維持するべき距離を定義
        diff = player.transform.position - transform.position; //最初に維持するべき距離を定義
    }

    // Playerが動く(Update･FixedUpdate)後に実行される
    void LateUpdate()
    {
        //現地点A→目的地Bに向けて収束していく
        //現地点Aの位置はどんどん詰めていくのでやがて限りなくイコールに近い距離になってくる
        //最終的には限りなく近づき続けるだけでイコールにはならないが、描画上は完全に止まっているかのように表現される
        transform.position = Vector3.Lerp(transform.position, //現時点A
            player.transform.position - diff, //目的地B
            Time.deltaTime * followSpeed //A→Bへの進行に対して、どれだけ進んだ位置にいるべきかの進捗率
            ); 
    }
}