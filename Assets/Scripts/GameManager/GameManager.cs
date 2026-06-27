using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using Unity.VisualScripting; // コルーチンに必要

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI設定")]
    public TextMeshProUGUI countText;       // 残り人数用
    public TextMeshProUGUI countdownText;   // カウントダウン用


    public bool isGameStarted = false; // ゲーム開始フラグ

    protected virtual void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        UpdateCharacterCount();
        StartCoroutine(CountdownRoutine()); // カウントダウン開始
    }

    /* スタート時の処理（子クラスで変更可能） */
    protected virtual void OnStart()
    {
        UpdateCharacterCount();
    }

    /*スタート時のカウントダウン*/
    IEnumerator CountdownRoutine()
    {
        isGameStarted = false; // 最初は動けない
        Time.timeScale = 1;    // ヒットストップ等で止まっている可能性のリセット

        countdownText.gameObject.SetActive(true);

        countdownText.text = "3";
        yield return new WaitForSeconds(1.0f);

        countdownText.text = "2";
        yield return new WaitForSeconds(1.0f);

        countdownText.text = "1";
        yield return new WaitForSeconds(1.0f);

        countdownText.text = "START!";
        isGameStarted = true; // ここで全員動けるようになる

        yield return new WaitForSeconds(1.0f);
        countdownText.gameObject.SetActive(false); // テキストを消す
    }

    /*残りカプセル数カウント（左上に出ている残り人数）*/
    public virtual void UpdateCharacterCount()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] cpus = GameObject.FindGameObjectsWithTag("CPU");
        int totalCount = cpus.Length + (player != null ? 1 : 0);
        if (countText != null) countText.text = "SURVIVORS: " + totalCount;
    }

    /*ゲームオーバーチェック（子クラスでの上書き・変更可能）*/
    public virtual void CheckGameOver()
    {
        StartCoroutine(CheckGameOverRoutine());
    }

    protected virtual IEnumerator CheckGameOverRoutine()
    {
        yield return new WaitForSecondsRealtime(0.15f);
        UpdateCharacterCount();
        CountRemainingCharacters();
    }

    /*ヒットストップ（共通）*/
    public void HitStop()
    {
        StartCoroutine(HitStopRoutine());
    }
    IEnumerator HitStopRoutine()
    {
        Time.timeScale = 0.05f;
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 1.0f;
    }

    /* カプセルが撃破された際の処理（ゲームオーバーやゲームクリア管理など）*/
    protected virtual void CountRemainingCharacters()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] cpus = GameObject.FindGameObjectsWithTag("CPU");

        if (player == null)
        {
            if (countText != null) countText.text = "GAME OVER";
            Invoke("RestartGame", 3f);
        }
        else if (cpus.Length == 0)
        {
            SceneManager.LoadScene("StageClear");//ステージクリア
        }
    }

    /*プレイヤーがゲームオーバーになった際、元ステージからやり直す*/
    protected void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}