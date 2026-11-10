using UnityEngine;

public enum UpgradeType
{
    LifeUp,       //最大ライフアップ
    PierceBullet, //貫通弾
    BulletUp      //初期弾数アップ
}

//拡張アイテム1つ分のデータ(TalkDataと同じScriptableObject)
[CreateAssetMenu(fileName = "NewUpgradeData", menuName = "ScriptableObjects/UpgradeData", order = 2)]

public class UpgradeData : ScriptableObject
{
    public string upgradeName;      //表示名(アイコン名)

    [TextArea(2, 5)]
    public string description;      //説明文

    public UpgradeType upgradeType; //種類
    public int value;               //効果の大きさ(LifeUpなら+いくつ、など)
}