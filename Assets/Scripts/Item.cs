using UnityEngine;
using UnityEngine.AdaptivePerformance;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        Life,
        Bullet,
        Upgrade
    }

    public ItemType itemType;
    public int value;
    public UpgradeData upgradeData; //生成したデータの情報を参照できる変数

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //if (itemType == ItemType.Life)
            switch (itemType)
            {
                case ItemType.Life:
                    other.gameObject.GetComponent<PlayerRun>().RecoverLife(value);
                    FindAnyObjectByType<UIController>().UpdateLife(); //UI Life更新(RecoverLife()内でも可)
                    break;
                case ItemType.Bullet:
                    other.gameObject.GetComponent<PlayerShooter>().AddBullet(value);
                    FindAnyObjectByType<UIController>().UpdateBullet(); //UI Bullet更新
                    break;
                case ItemType Upgrade:
                    GameData.AddUpgrade(upgradeData); //管理リストに加える
                    break;
            }
            Destroy(gameObject);
        }
    }
}
