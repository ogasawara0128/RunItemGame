using System.Collections.Generic;

//シーンをまたいで情報を持ち続けるためのクラス
//static（MonoBehaviourではない）なので、オブジェクトに付ける必要はない
public static class GameData
{
    public const int SlotCount = 2; //セットできるスロットの数

    //入手済みの拡張アイテム
    public static List<UpgradeData> ownedUpgrades = new List<UpgradeData>();

    //スロットにセット中の拡張アイテム（最大SlotCount個）
    public static List<UpgradeData> equippedUpgrades = new List<UpgradeData>();

    //拡張アイテムを入手する（同じものは2つ持たない）
    public static void AddUpgrade(UpgradeData data)
    {
        if (!ownedUpgrades.Contains(data))
        {
            ownedUpgrades.Add(data);
        }
    }

    //その種類の拡張アイテムをセットしているかどうか
    public static bool HasUpgrade(UpgradeType type)
    {
        foreach (UpgradeData data in equippedUpgrades)
        {
            if (data.upgradeType == type) return true;
        }
        return false;
    }

    //その種類の拡張アイテムの効果の大きさ（セットしていなければ0）
    public static int GetValue(UpgradeType type)
    {
        int total = 0;
        foreach (UpgradeData data in equippedUpgrades)
        {
            if (data.upgradeType == type) total += data.value;
        }
        return total;
    }
}