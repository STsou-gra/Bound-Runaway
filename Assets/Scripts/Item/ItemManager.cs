using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
  [SerializeField] private GameObject item;
  [SerializeField] private Transform[] spawnPoints;
  [SerializeField] private float spawnInterval = 25f;

  private float progressTimer;//経過時間

  void Update()
  {
    progressTimer += Time.deltaTime;
    if (progressTimer >= spawnInterval)
    {
      progressTimer = 0f;
      SpawnItem();
    }
  }

  private void SpawnItem()
  {
    if (item == null || spawnPoints == null || spawnPoints.Length == 0)
    {
      return;
    }
    int index = Random.Range(0, spawnPoints.Length);
    Transform point = spawnPoints[index];
    Instantiate(item, point.position, Quaternion.identity);
  }
}