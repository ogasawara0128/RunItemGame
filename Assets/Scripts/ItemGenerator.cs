using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    const int MinLane = -2; //最小レーン番号
    const int MaxLane = 2; //最大レーン番号
    const float LaneWidth = 2.0f; //レーン幅
    const float StageChipSize = 30.0f; //ステージの奥行(Z)のサイズ

    public GameObject[] itemPrefabs; //生成するアイテムの種類（ここを入れ替える）
    public float minInterval = 5.0f; //アイテムを置く間隔(Z)の最小値
    public float maxInterval = 15.0f; //アイテムを置く間隔(Z)の最大値
    public float offsetZ = 0.0f; //最初のアイテムをどこから置くかのずらし
    public float posY = 1.0f; //アイテムを置く高さ
    public float spawnRate = 0.2f; //アイテムが出る確率（0〜1）

    void Start()
    {
        //ステージの手前の端(-15)から、奥の端(15)まで、ランダムな間隔でアイテムを置く
        //1つ置くたびに、minInterval〜maxIntervalの間でランダムに進める
        for (float z = -StageChipSize / 2 + offsetZ; z < StageChipSize / 2; z += Random.Range(minInterval, maxInterval))
        {
            //確率に外れたら、今回は置かずに次へ
            if (Random.value > spawnRate) continue;

            //ランダムなレーン番号（-2〜2）を決める ※Range(-2, 3)で-2〜2になる
            int lane = Random.Range(MinLane, MaxLane + 1);

            //配列からランダムにアイテムの種類を選ぶ
            int index = Random.Range(0, itemPrefabs.Length);

            //ステージの位置を基準に、生成する座標を決める
            Vector3 pos = transform.position + new Vector3(lane * LaneWidth, posY, z);

            //ステージの子として生成（ステージが消えると一緒に消える）
            Instantiate(itemPrefabs[index], pos, Quaternion.identity, transform);
        }
    }
}
