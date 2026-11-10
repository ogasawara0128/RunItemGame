using UnityEngine;

public class TrapGenerator : MonoBehaviour
{
    //レーン番号は不要
    const float StageChipSize = 30.0f; //ステージの奥行(Z)のサイズ

    public GameObject trapPrefab; //生成するトラップ
    public float minInterval = 5.0f; //トラップを置く間隔(Z)の最小値
    public float maxInterval = 15.0f; //トラップを置く間隔(Z)の最大値
    public float offsetZ = 5.0f; //最初のトラップをどこから置くかのずらし
    public float posY = 0.5f; //トラップを置く高さ
    public float spawnRate = 0.3f; //トラップが出る確率（0〜1）

    void Start()
    {
        //ステージの手前の端(-15)から、奥の端(15)まで、ランダムな間隔でトラップを置く
        //1つ置くたびに、minInterval〜maxIntervalの間でランダムに進める
        for (float z = -StageChipSize / 2 + offsetZ; z < StageChipSize / 2; z += Random.Range(minInterval, maxInterval))
        {
            //確率に外れたら、今回は置かずに次へ
            if (Random.value > spawnRate) continue;

            //ステージの位置を基準に、生成する座標を決める
            Vector3 pos = transform.position + new Vector3(0, posY, z);

            //ステージの子として生成（ステージが消えると一緒に消える）
            Instantiate(trapPrefab, pos, Quaternion.identity, transform);
        }
    }
}
