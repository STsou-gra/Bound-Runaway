using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.TextCore.Text;

public interface ICharacter
{
  float SpeedMultiplier { get; }
  bool IsActionable { get; }
  void OnUpdate(Character character);
}