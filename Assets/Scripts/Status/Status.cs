using UnityEngine;

public class Status : MonoBehaviour
{
  protected Enemy target;
  protected Rigidbody targetRigidbody;

  //状態セットアップ
  public void Setup(Enemy enemy, Rigidbody rb, float duration)
  {
    target = enemy;
  }
}
