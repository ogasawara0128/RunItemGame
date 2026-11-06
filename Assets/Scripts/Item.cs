using UnityEngine;
using UnityEngine.AdaptivePerformance;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        Life,
        Bullet
    }

    public ItemType itemType;
    public int value;

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
            }
            Destroy(gameObject);
        }
    }
}
