using UnityEngine;

public class Field : MonoBehaviour
{
  public virtual void Initialize(float lifeTime)
  {
    Destroy(gameObject, lifeTime);
  }
}