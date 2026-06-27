using UnityEngine;

public class ElectronicDonut : Item
{
  [SerializeField]
  private GameObject electronicField; //ElectronicFieldプレハブ
  [SerializeField] private float fieldTime = 10f;

  protected override void OnGet(Collider player)
  {
    GameObject fieldObject = Instantiate(electronicField, player.transform.position, Quaternion.identity);

    fieldObject.transform.SetParent(player.transform);
    fieldObject.transform.localPosition = Vector3.zero;

    //生成する為（ElectronicFieldにInitialize関数がある為）にField型変数を生成
    Field field = fieldObject.GetComponent<Field>();
    if (field != null)
    {
      field.Initialize(fieldTime);
    }
    Destroy(gameObject);
  }
}