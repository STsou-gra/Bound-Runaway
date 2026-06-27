using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartController : MonoBehaviour
{
    public void StageSelect(int stageNumber)
    {
        SceneManager.LoadScene("Stage" + stageNumber, LoadSceneMode.Single);
    }

    public void EndlessStageSelect(int stageNumber)
    {
        SceneManager.LoadScene("EndlessStage" + stageNumber, LoadSceneMode.Single);
    }
}
