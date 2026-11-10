using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    const int MinLane = -2; //最小レーン番号
    const int MaxLane = 2; //最大レーン番号
    const float LaneWidth = 2.0f; //レーン幅
    const float StageChipSize = 30.0f; //ステージの奥行(Z)のサイズ

    public GameObject[] enemyPrefabs; //生成する敵の種類（ここを入れ替える）
    public float minInterval = 5.0f; //敵を置く間隔(Z)の最小値
    public float maxInterval = 15.0f; //敵を置く間隔(Z)の最大値
    public float offsetZ = 5.0f; //最初の敵をどこから置くかのずらし
    public float posY = 1.43f; //敵を置く高さ
    public float multiRate = 0.3f; //横に並ぶ確率（0〜1）
    public int maxCount = 3; //横に並ぶ最大数（4まで。5だと全レーンがふさがる）

    void Start()
    {
        //ステージの手前の端(-15)から、奥の端(15)まで、ランダムな間隔で敵を置く
        //1つ置くたびに、minInterval〜maxIntervalの間でランダムに進める
        for (float z = -StageChipSize / 2 + offsetZ; z < StageChipSize / 2; z += Random.Range(minInterval, maxInterval))
        {
            //横に並べる数を決める（ふだんは1体、たまに2〜maxCount体）
            //Random.valueは0～1の間(float)
            int count = 1;
            if (Random.value < multiRate) count = Random.Range(2, maxCount + 1);

            //並べはじめるレーン番号を決める（右端からはみ出さない範囲で）
            int startLane = Random.Range(MinLane, MaxLane - count + 2);

            //count体ぶん、となりのレーンに並べて置く
            for (int i = 0; i < count; i++)
            {
                int lane = startLane + i; //置くレーン番号

                //配列からランダムに敵の種類を選ぶ
                int index = Random.Range(0, enemyPrefabs.Length);

                //ステージの位置を基準に、生成する座標を決める
                Vector3 pos = transform.position + new Vector3(lane * LaneWidth, posY, z);

                //ステージの子として生成（ステージが消えると一緒に消える）
                Instantiate(enemyPrefabs[index], pos, Quaternion.identity, transform);
            }
        }
    }
}