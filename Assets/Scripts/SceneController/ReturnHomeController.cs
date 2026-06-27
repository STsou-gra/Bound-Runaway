using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnHomeController : MonoBehaviour
{
    public void ReturnHome()
    {
        SceneManager.LoadScene("StageSelect", LoadSceneMode.Single);
    }
}
