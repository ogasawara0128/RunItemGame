using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    //ゲームのステータス用のテキスト
    public GameObject statusText;

    //スライダー関連
    public GameObject sliderPanel;
    public Slider lifeSlider;

    //バレット関連
    public GameObject bulletPanel;
    public TextMeshProUGUI bulletText;

    //ボタン関連
    public GameObject buttonPanel;
    public GameObject retryButton;

    void Start()
    {
        //ボタンを非表示
        buttonPanel.SetActive(false);

        //時間差でステータス文字を非表示
        StartCoroutine(DisplayChange());

        UpdateLife(); //プレイヤー体力を反映
        UpdateBullet();
    }

    IEnumerator DisplayChange()
    {
        yield return new WaitForSeconds(1.0f);
        statusText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //ライフUIを更新
    public void UpdateLife()
    {
        //スクリプトを持ってるオブジェクトを探す
        //PlayerRunスクリプトを参照できるようにする
        PlayerRun player = GameObject.FindAnyObjectByType<PlayerRun>();

        //残Lifeをスライダー値に代入
        lifeSlider.value = player.Life();
    }

    //Bulletの残弾数を更新
    public void UpdateBullet()
    {
        //PlayerShooterスクリプトを参照できるようにする
        PlayerShooter player = GameObject.FindAnyObjectByType<PlayerShooter>();

        //残団を文字列に変換してテキストに代入
        bulletText.text = player.GetRemaining().ToString();
    }

    //リトライボタン
    //publicにすることでOnClickで選択可能
    public void Retry()
    {
        //現シーンの情報を取得
        string currentScene = SceneManager.GetActiveScene().name;
        //取得した現シーンを読み直す
        SceneManager.LoadScene(currentScene);
    }

    //ゲームオーバー時、GameManagerから呼び出されるメソッド
    public void GameOverDisplay()
    {
        buttonPanel.SetActive(true); //ボタンの表示
        //ステータス「GAME OVER」の表示
        statusText.SetActive(true);
        statusText.GetComponent<TextMeshProUGUI>().text = "GAME OVER";

        sliderPanel.SetActive(false); //スライダーパネルの非表示
        bulletPanel.SetActive(false); //Bulletパネルの非表示
    }
}
