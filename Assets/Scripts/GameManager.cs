using Unity.Cinemachine;
using UnityEngine;

//自作の列挙型(ゲームのステータス)
public enum GameState
{
    None,
    IsPlay,
    GameClear,
    GameOver,
    GameEnd
}

public class GameManager : MonoBehaviour
{
    //自作した型を扱うstatic変数
    public static GameState gameState;

    private GameObject mixingCamera;

    void Start()
    {
        gameState = GameState.IsPlay;

        //mixingカメラを探索
        mixingCamera = GameObject.FindGameObjectWithTag("MixingCamera");
        //mixingカメラを非表示
        mixingCamera.SetActive(false);
        //mainカメラのCinemachineBrainを無効化
        Camera.main.GetComponent<CinemachineBrain>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.GameOver)
        {
            //UIにゲームオーバーの表示命令
            UIController controller = GameObject.FindAnyObjectByType<UIController>();
            controller.GameOverDisplay();

            mixingCamera.SetActive(true); //mixingカメラを復活
            Camera.main.GetComponent<CinemachineBrain>().enabled=true; //Brainを復活

            gameState = GameState.GameEnd;
        }
    }
}
