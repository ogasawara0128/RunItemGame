using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGenerator : MonoBehaviour
{
    const int StageChipSize = 30; //ステージの奥行(z)のサイズ

    //現在どこまで作ったか
    int currentChipIndex; //生成済みのフロア番号

    //public Transform character;
    Transform character; //対象となるプレイヤーのTransform
    public GameObject[] stageChips; //生成されるフロアの種類
    public int startChipIndex = 1; //最初のフロア番号(1とする)
    public int preInstantiate = 5; //予備で作っておく数(5とする)

    //生成されたフロア情報が順番に記帳される管理リスト(通り過ぎると抹消される)
    public List<GameObject> generatedStageList = new List<GameObject>();

    void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player").transform; //Playerを検索し、そのTransformを取得
        currentChipIndex = startChipIndex - 1; //現在基準のフロアを-1にする
        UpdateStage(preInstantiate); //まず最初にいくつかのフロアを生成する(自作メソッド)
    }

    void Update()
    {
        //PlayerのZ位置をフロアのサイズ30で割ることで、何番目のフロアにいるのかを常に把握
        int charaPositionIndex = (int)(character.position.z / StageChipSize);

        //もし（Playerの現在フロア番号 + 予備を見越したフロア番号）が 生成したフロア番号を上回ってしまいそうなら
        if (charaPositionIndex + preInstantiate > currentChipIndex)
        {
            //新しく予備分の生成を補う（引数：ここまでつくっておいて欲しいというフロア番号を指定）
            UpdateStage(charaPositionIndex + preInstantiate);
        }
    }

    //フロア生成メソッド（引数：ここまでつくっておいて欲しいというフロア番号）
    void UpdateStage(int toChipIndex)
    {
        //if (toChipIndex <= currentChipIndex) return; //記述不要

        //現在生成済みフロア番号 +1　からはじめ、作るべきフロア番号にたどり着くまで繰り返し生成
        for (int i = currentChipIndex + 1; i <= toChipIndex; i++)
        {
            //実際にInstantiateするメソッドを発動、それにより生成されたフロア情報は変数に確保
            GameObject stageObject = GenerateStage(i);

            generatedStageList.Add(stageObject); //生成したフロア情報を管理リストに記帳する
        }

        //必要な数に減少するまで、フロア削除メソッドを繰り返す
        //管理リストに記帳されているフロア数が(予備数+2)を上回るくらい増えてしまったら古いフロアから削除
        while (generatedStageList.Count > preInstantiate + 2) DestroyOldestStage();

        currentChipIndex = toChipIndex;
    }

    //実際にフロアをInstantiate、GameObjectとして返す
    GameObject GenerateStage(int chipIndex)
    {
        //どのフロア種類を生成しようか、配列からランダムにチョイス
        int nextStageChip = Random.Range(0, stageChips.Length);

        //引数のフロア番号に対応した座標に、配列から選ばれたフロアを生成
        GameObject stageObject = Instantiate(stageChips[nextStageChip], new Vector3(0, 0, chipIndex * StageChipSize), Quaternion.identity);

        return stageObject; //生成されたフロア情報を戻り値として返す
    }

    //管理リストから古いフロアを ①リストから ②実際にオブジェクトも 削除するメソッド
    void DestroyOldestStage()
    {
        GameObject oldStage = generatedStageList[0]; //管理リストの先頭(0番)にいるフロア情報を参照
        generatedStageList.RemoveAt(0); //①まずリストから抹消
        Destroy(oldStage); //②対象オブジェクトを実際にヒエラルキーから削除
    }
}
