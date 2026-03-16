using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections; // コルーチンに必要

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI設定")]
    public TextMeshProUGUI countText;       // 残り人数用
    public TextMeshProUGUI countdownText;   // ★カウントダウン用


    public bool isGameStarted = false; // ★ゲーム開始フラグ

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        UpdateCharacterCount();
        StartCoroutine(CountdownRoutine()); // ★カウントダウン開始
    }

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
        isGameStarted = true; // ★ここで全員動けるようになる

        yield return new WaitForSeconds(1.0f);
        countdownText.gameObject.SetActive(false); // テキストを消す
    }

    void Update()
    {
    }

    // --- 以下、既存のUpdateCharacterCount, SpawnNewBall, CheckGameOverなどはそのまま ---
    // ※ CheckGameOver の中の RestartGame などの処理は以前のままでOKです。
    
    public void UpdateCharacterCount()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] cpus = GameObject.FindGameObjectsWithTag("CPU");
        int totalCount = cpus.Length + (player != null ? 1 : 0);
        if (countText != null) countText.text = "SURVIVORS: " + totalCount;
    }

    public void CheckGameOver()
    {
        Invoke("UpdateCharacterCount", 0.1f);
        Invoke("CountRemainingCharacters", 0.15f);
    }

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

    void CountRemainingCharacters()
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
            if (countText != null) countText.text = "YOU WIN!";
            Invoke("RestartGame", 3f);
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}