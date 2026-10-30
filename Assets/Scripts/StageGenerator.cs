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
        int charaPositionIndex = (int)(character.position.z / StageChipSize);

        if (charaPositionIndex + preInstantiate > currentChipIndex)
        {
            UpdateStage(charaPositionIndex + preInstantiate);
        }
    }

    void UpdateStage(int toChipIndex)
    {
        //if (toChipIndex <= currentChipIndex) return; //記述不要

        for (int i = currentChipIndex + 1; i <= toChipIndex; i++)
        {
            GameObject stageObject = GenerateStage(i);

            generatedStageList.Add(stageObject);
        }

        while (generatedStageList.Count > preInstantiate + 2) DestroyOldestStage();

        currentChipIndex = toChipIndex;
    }

    GameObject GenerateStage(int chipIndex)
    {
        int nextStageChip = Random.Range(0, stageChips.Length);

        GameObject stageObject = Instantiate(stageChips[nextStageChip], new Vector3(0, 0, chipIndex * StageChipSize), Quaternion.identity);

        return stageObject;
    }

    void DestroyOldestStage()
    {
        GameObject oldStage = generatedStageList[0];
        generatedStageList.RemoveAt(0);
        Destroy(oldStage);
    }
}
