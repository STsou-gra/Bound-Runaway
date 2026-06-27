using TMPro;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultTime;

    void Start()
    {
        string clearTime = PlayerPrefs.GetString("ClearTime", "Unmeasurable");
        resultTime.text = clearTime;
    }
}
