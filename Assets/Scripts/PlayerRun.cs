using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRun : MonoBehaviour
{
    const int MinLane = -2; //最小レーン番号
    const int MaxLane = 2; //最大レーン番号
    const float LaneWidth = 2.0f; //レーン幅

    CharacterController controller; //CharacterControllerを参照できる変数
    [SerializeField] private GameObject animatorBody; //Animatorコンポーネントを持っているオブジェクトの取得
    Animator animator; //Animatorを参照できる変数

    //Vector3 moveDirection =new Vector3(0,0,0);
    Vector3 moveDirection = Vector3.zero; //Moveメソッドに与える方向
    int targetLane; //目指すべきレーン

    [Header("重力・前進・横移動・ジャンプ")]
    public float gravity; //重力地
    public float speedZ; //前に進む力
    public float speedX; //横移動スピード
    public float speedJump; //ジャンプ力

    [Header("前進加速力")]
    public float accelerationZ = 10.0f; //加速力

    Coroutine resetIntervalCol; //インプットのインターバル置くための変数

    private InputAction moveAction; //"Move"アクションを参照するための変数
    private InputAction jumpAction; //"Jump"アクションを参照するための変数

    void Start()
    {
        //必要なコンポーネントを自動取得
        controller = GetComponent<CharacterController>(); //Playerについているコンポーネントの取得
        animator = animatorBody.GetComponent<Animator>(); //子オブジェクトが持っているAnimatorコンポーネントの取得

        //現PlayerInputコンポーネントに設定されている現Mapは何かを取得=(Playerマップ)
        //↓Update内に書く書き方 OnMoveで外に書くのも有り
        InputActionMap actionMap = GetComponent<PlayerInput>().currentActionMap;
        moveAction = actionMap.FindAction("Move"); //現マップ上にある"Move"というアクションを取得
        jumpAction = actionMap.FindAction("Jump"); //現マップ上にある"Jump"というアクションを取得
    }

    // Update is called once per frame
    void Update()
    {
        //仕様としてCharacterControllerコンポーネントが接地判断を持っている
        if (controller.isGrounded)
        {
            //if (Input.GetAxis("Vertical") != 0.0f)
            //if (moveAction.ReadValue<Vector2>().y != 0.0f) //入力データとしてのVector2
            //{
            //    //上下キーによる前か後ろかの値の設定
            //    //後ろの場合(マイナス方向)にはVerticalで勝手にマイナスになる
            //    //moveDirection.z = Input.GetAxis("Vertical") * speedZ;
            //    moveDirection.z = moveAction.ReadValue<Vector2>().y * speedZ; //操作上の上下を3D空間の奥行に代入
            //}
            //else
            //{
            //    moveDirection.z = 0;
            //}

            //横回転 Rotateメソッドで直接回転させる
            //回転はY軸になる
            //transform.Rotate(0, Input.GetAxis("Horizontal") * 300 * Time.deltaTime, 0);
            //transform.Rotate(0, moveAction.ReadValue<Vector2>().x * 300 * Time.deltaTime, 0);
            
            //コルーチンが発動していない時だけ入力処理
            if (resetIntervalCol == null)
            {
                if (moveAction.ReadValue<Vector2>().x < 0) MoveToLeft(); //左のレーン番号に変更
                else if (moveAction.ReadValue<Vector2>().x > 0) MoveToRight(); //右のレーン番号に変更
            }

            //対象とするレーンをもとにmoveDirectionのX(横移動)を決める
            float ratioX = (targetLane * LaneWidth - transform.position.x) / LaneWidth;
            moveDirection.x = ratioX * speedX;

            //自動前進するためのmoveDirectionZ(前進加速)を決める
            float acceleratedZ = moveDirection.z + (accelerationZ * Time.deltaTime);
            moveDirection.z = Mathf.Clamp(acceleratedZ, 0, speedZ);

            //ジャンプボタンが押されている間(GetButtonDownなら押した瞬間)
            //if (Input.GetButton("Jump"))
            if (jumpAction.IsPressed())
            {
                moveDirection.y = speedJump; //y方向をセッティング
                //animator.SetTrigger("jump"); //ジャンプアニメへの切替
            }
        }

        //常に重力はかかっている
        moveDirection.y -= gravity; //後に* Time.deltaTimeするので不要

        //セッティングしたmoveDirectionの値を使ってMoveメソッドの発動
        //※実際にPlayerが動く Time.deltaTimeでPC差によらないように動作安定
        //回転を含む前進だと、グローバルな座標指定からズレるので、グローバルな座標になるように変換してからMoveに与える
        Vector3 globalDirection = transform.TransformDirection(moveDirection);
        //RigidbodyのAddForceみたいなもの
        controller.Move(globalDirection * Time.deltaTime);

        //移動後接地してたらY方向の速度はリセットする
        //地面にいるのであればY成分の力を毎回0にリセット 地面にめり込み防止
        if (controller.isGrounded) moveDirection.y = 0;

        //アニメの制御
        //animator.SetBool("run", moveDirection.z > 0.0f);
    }

    public void MoveToLeft()
    {
        if (targetLane > MinLane) //現レーンがまだ最大でなければ
        {
            targetLane--; //対象レーン番号をひとつ増やす
            //次の入力認識するまでのインターバルを保つコルーチンの発動
            resetIntervalCol=StartCoroutine(ResetIntervalCol()); //コルーチン:何かの処理を別で発生させる
        }
    }

    public void MoveToRight()
    {
        if (targetLane < MaxLane) //現レーンがまだ最大でなければ
        {
            targetLane++; //対象レーン番号をひとつ増やす
        }
    }

    //特定の時間(インターバル)だけ入力を待つコルーチン
    IEnumerator ResetIntervalCol()
    {
        yield return new WaitForSeconds(0.1f); //0.1秒待ってから
        resetIntervalCol = null; //コルーチン発動中である証明だった変数を解放
    }

}
