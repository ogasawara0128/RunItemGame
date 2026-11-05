using UnityEngine;

public class EnemyEffect : MonoBehaviour
{
    public float deleteTime = 1.0f;

    void Start()
    {
        Destroy(gameObject,deleteTime);
    }
}
