using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System; // コルーチンに必要

public class EndlessGameManager : GameManager
{
    /*GameManagerの処理のまま、インスタンスはEndlessManagerを入れる*/
    protected override void Awake()
    {
        base.Awake();
    }

    protected virtual void Onstart()
    {
        //エンドレスモードではStart時のUpdateCharacterCount呼び出しをしない（登場カプセルがプレイヤーしかいないため）
    }

    /* ゲームオーバー時の処理変更、
    /* Destroy関数より早く実行しリザルト画面への遷移ができない問題を解決するために
    /* 0.15f待ってからCountRemainingCharacters()実行に変更、UpdateCharacterCount()はEndlessGameManagerでは不要*/
    protected override IEnumerator CheckGameOverRoutine()
    {
        yield return new WaitForSecondsRealtime(0.15f);
        CountRemainingCharacters();
    }


    protected override void CountRemainingCharacters()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log(player);
        if (player == null)
        {
            Debug.Log("player==null");
            SceneManager.LoadScene("StageResult");//ステージリザルト画面への遷移
        }
    }
}