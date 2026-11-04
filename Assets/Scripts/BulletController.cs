using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("弾の自動消滅時間")]
    public float deleteTime=3.0f;

    void Start()
    {
        Destroy(gameObject,deleteTime);
    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
