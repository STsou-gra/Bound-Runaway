using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
  bool isGet; //獲得済みフラグ
  protected void OnTriggerEnter(Collider player)
  {
    if (!isGet && player.CompareTag("Player"))
    {
      isGet = true;
      OnGet(player);
    }
  }

  protected virtual void OnGet(Collider player)
  {
    Destroy(gameObject);
  }

}