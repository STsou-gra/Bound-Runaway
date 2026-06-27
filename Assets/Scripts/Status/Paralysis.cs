using UnityEngine;

public class Paralysis : Status
{
    private Enemy target;
    private Rigidbody targetRigidbody;

    //麻痺状態セットアップ
    public void Setup(Enemy enemy, Rigidbody rb, float duration)
    {
        target = enemy;
    }
}
