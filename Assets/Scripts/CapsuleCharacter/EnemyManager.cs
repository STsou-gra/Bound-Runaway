using UnityEngine;


public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab; // 敵のプレハブをインスペクターでアタッチ
    [SerializeField] private Transform[] spawnPoints; // 湧き出させたい場所の座標リスト
    void Start()
    {
        foreach (Transform point in spawnPoints)
        {
            Instantiate(enemyPrefab, point.position, Quaternion.identity);
        }
    }

}
