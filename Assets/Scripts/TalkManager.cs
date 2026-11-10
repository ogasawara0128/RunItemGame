using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class TalkManager : MonoBehaviour
{
    public TalkData currentTalkData; //取り扱いたいデータベース

    //UI側のパーツ
    public GameObject talkPanel; //会話枠パネル
    public TextMeshProUGUI nameText; //名前UI
    public TextMeshProUGUI messageText; //本文UI
    public Image faceImage; //顔グラフィックUI

    public float textSpeed = 0.04f; //文字送りのスピード

    private int currentIndex = -1; //現在表示している会話の番号
    private bool isTyping = false; //文字送り中かどうか
    private string currentFullText = "";
    private Coroutine typingCoroutine; //文字送りコルーチン参照

    public string nextScene; //次のシーン名

    //ボタン入力
    InputAction submitAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //現マップ(UI)の情報を取得
        InputActionMap actionMap = GetComponent<PlayerInput>().currentActionMap;
        submitAction = actionMap.FindAction("Submit"); //UIマップ上で定義されているSubmitアクションを参照

        //最初のメッセージ更新
        AdvanceTalk();
    }

    // Update is called once per frame
    void Update()
    {
        if (submitAction.WasPressedThisFrame())
        {
            //メッセージを更新
            AdvanceTalk();
        }
    }

    public void AdvanceTalk()
    {
        if (currentTalkData == null || currentTalkData.talks.Count == 0) return;

        //コルーチン止める
        if (isTyping)
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            messageText.text = currentFullText;
            isTyping = false; //フラグOFF
            return;
        }

        currentIndex++; //番号を1更新

        //対象としているTalkData(の持っているリスト)の数をIndexが上回ったら終了
        if (currentIndex >= currentTalkData.talks.Count)
        {
            EndTalk(); //終了メソッド
            return;
        }

        //トークパネルが実在していれば
        if (talkPanel != null && talkPanel.activeSelf)
        {
            talkPanel.SetActive(true); //トークUIの表示
        }
        //対象データSingleTalkクラスには名前・メッセージ・顔イラスト
        SingleTalk talk = currentTalkData.talks[currentIndex];

        if (nameText != null) nameText.text = talk.name; //データベースから抜き出したSingleTalkクラスのnameをUIに代入※空なら何も表示されない

        if (faceImage != null)
        {
            //データベースから抜き出したSingleTalkクラスの顔イメージがあれば出す/なければ出さない
            faceImage.gameObject.SetActive(talk.faceImage != null);
            faceImage.sprite = talk.faceImage; //あれば差し替え nullならnull
        }

        currentFullText = talk.message; //抜き出したSingleTalkクラスのメッセージを変数に
        messageText.text = currentFullText; //メッセージUIに反映
        typingCoroutine = StartCoroutine(TypeTextCoroutine(currentFullText));
    }
    
    IEnumerator TypeTextCoroutine(string text)
    {
        isTyping = true;
        messageText.text = ""; //まずリセット

        //引数に渡したメッセージ全文を分解
        foreach (char c in text)
        {
            messageText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping=false;
    }

    void EndTalk()
    {
        currentIndex = -1; //再利用できるようにリセット

        if (talkPanel != null)
        {
            talkPanel.SetActive(false);
        }

        if (nextScene != "") //次のシーンの名前があれば
        {
            StartCoroutine(NextScene());
        }
    }

    //時間差で次のシーン
    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(nextScene);
    }
}