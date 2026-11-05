using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    [Header("生成される弾")]
    public GameObject bullet;

    InputAction shootAction; //Attackアクションボタンの割り振り

    Coroutine shootStart; //シュートされているかどうかを知るための変数

    [Header("弾速・インターバル時間")]
    public float bulletSpeed = 10.0f;
    public float shootInterval = 0.5f;

    const int startRemaining = 10; //初期弾数
    private int bulletRemaining = startRemaining; //弾の残数

    //UIなどに残数を伝えるアクセサメソッド
    public int GetRemaining()
    {
        return bulletRemaining;
    }

    //弾数を回復するメソッド
    public void AddBullet(int num)
    {
        bulletRemaining += num;
    }

    void Start()
    {
        //PlayerInputコンポーネントに設定されている現Mapを取得
        InputActionMap actionMap = GetComponent<PlayerInput>().currentActionMap;
        //現マップ上にいる「Attack」というアクションを取得
        shootAction = actionMap.FindAction("Attack");
    }

    void Update()
    {
        //Attackアクションボタンが押された時Shootメソッド
        if (shootAction.WasPerformedThisFrame()&&bulletRemaining>0)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (shootStart == null)
        {
            bulletRemaining--; //弾を1減らす

            //UIControllerを探して、残弾更新メソッド
            UIController controller = GameObject.FindAnyObjectByType<UIController>();
            controller.UpdateBullet();

            //攻撃開始Bulletプレハブの生成とZ軸前方へシュート
            GameObject obj = Instantiate(bullet, transform.position, Quaternion.identity);

            Rigidbody rbody = obj.GetComponent<Rigidbody>();
            rbody.AddForce(new Vector3(0, 0, bulletSpeed), ForceMode.Impulse);
            //インターバルとなるコルーチンの開始
            shootStart = StartCoroutine(ShootStart());
        }
    }

    //インターバル用コルーチン
    IEnumerator ShootStart()
    {
        //0.5f秒待つ
        yield return new WaitForSeconds(shootInterval);
        //コルーチン変数の解放
        shootStart = null;
    }
}
