using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectronicField : Field
{
  [SerializeField] private float freezeTime = 10f;
  [SerializeField] private GameObject fieldEffect;
  [SerializeField] private GameObject shockToEnemyEffect;
  //敵ごとに「コルーチン」と「生成したエフェクト」をペアで管理するための構造体
  private struct FreezeData
  {
    public Coroutine coroutine;
    public GameObject effectInstance;//付いているエフェクト
  }
  private readonly Dictionary<Enemy, FreezeData> freezeDataMap = new(); // すでに止めた敵を管理する
  public override void Initialize(float lifeTime)
  {
    base.Initialize(lifeTime);

    foreach (ElectronicField oldField in GameObject.FindObjectsByType<ElectronicField>(FindObjectsSortMode.None))
    {
      if (oldField != this)
      {
        Destroy(oldField.gameObject);
      }
    }

    if (fieldEffect != null)
    {
      GameObject electronicFieldEffect = Instantiate(fieldEffect, transform);
      electronicFieldEffect.transform.localPosition = Vector3.zero;
      electronicFieldEffect.transform.localRotation = Quaternion.identity;
      Destroy(electronicFieldEffect, lifeTime);
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    Freeze(other);
  }

  /*エフェクトを出す為に、重いStayをコメントアウト
    private void OnTriggerStay(Collider other)
    {
      Freeze(other);
    }*/


  private void Freeze(Collider other)
  {
    Enemy enemy = other.GetComponentInParent<Enemy>();
    if (enemy == null)
    {
      return;
    }
    if (freezeDataMap.ContainsKey(enemy))
    {
      return;
    }
    Rigidbody rb = enemy.GetComponent<Rigidbody>();
    if (rb != null)
    {
      rb.linearVelocity = Vector3.zero;
      rb.angularVelocity = Vector3.zero;
      rb.isKinematic = true; //吹っ飛ばされないための処理
    }

    enemy.enabled = false;

    GameObject spawnedEffect = null; //エフェクトの生成と保持

    if (shockToEnemyEffect != null && enemy.enabled == false)
    {
      spawnedEffect = Instantiate(shockToEnemyEffect, enemy.transform);
      spawnedEffect.transform.localPosition = Vector3.zero;
      spawnedEffect.transform.localRotation = Quaternion.identity;
    }
    FreezeData data = new FreezeData
    {
      coroutine = StartCoroutine(ToUnfreezeTime(enemy)),
      effectInstance = spawnedEffect
    };
    freezeDataMap[enemy] = data;
  }

  private IEnumerator ToUnfreezeTime(Enemy enemy)
  {
    yield return new WaitForSeconds(freezeTime);
    Unfreeze(enemy);
  }

  private void Unfreeze(Enemy enemy)
  {
    if (enemy == null)
    {
      return;
    }
    //フリーズ解除時にエフェクトを解除
    if (freezeDataMap.TryGetValue(enemy, out FreezeData data))
    {
      if (data.effectInstance != null)
      {
        Destroy(data.effectInstance);
      }
    }
    Rigidbody rb = enemy.GetComponent<Rigidbody>();
    if (rb != null)
    {
      rb.isKinematic = false;
    }

    enemy.enabled = true;
    freezeDataMap.Remove(enemy); //時間経過し敵の状態異常が解除
  }

  /* フィールドが途中で消滅した（次のアイテムを拾った）時の処理 */
  private void OnDestroy()
  {
    foreach (var pair in freezeDataMap)
    {
      Enemy enemy = pair.Key;
      FreezeData data = pair.Value;
      if (enemy != null)
      {
        Rigidbody rb = enemy.GetComponent<Rigidbody>();
        if (rb != null)
        {
          rb.isKinematic = false;
        }
        enemy.enabled = true;
      }

      if (data.effectInstance != null)
      {
        Destroy(data.effectInstance);
      }
    }
    freezeDataMap.Clear();
  }
}