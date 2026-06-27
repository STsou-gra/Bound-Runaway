using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour
{
    public Transform target;
    public GameObject ballPrefab;
    public Transform firePoint;     // Z=3 くらいに置く
    public GameObject linePrefab;   // プレハブのCubeをX=90にしておく
    public Transform barrelGroup;   // これを回す

    public float rotationSpeed = 5f;
    public float fireInterval = 5f;

    void Start()
    {
        if (target == null) target = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(FireRoutine());
    }

    void Update()
    {
        if (target == null || barrelGroup == null) return;

        // 1. ターゲットへの方向
        Vector3 direction = target.position - barrelGroup.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            // 2. 素直にその方向を向く（Z軸を向ける）
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            barrelGroup.rotation = Quaternion.Slerp(barrelGroup.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    IEnumerator FireRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireInterval);

            // 生成（firePointの位置に出す）
            GameObject line = Instantiate(linePrefab, firePoint.position, barrelGroup.rotation);

            // 砲身の子にする（これで一緒に回る）
            line.transform.SetParent(barrelGroup);
            Debug.Log($"[Indicator生成] 名前: {line.name}, 位置: {line.transform.localPosition}, 親: {line.transform.parent.name}");

            // 向きだけをリセットし、位置はfirePointの場所に固定
            // これで、Indicatorプレハブ内で「前」にずらした分だけ先端から伸びます
            //line.transform.localPosition = firePoint.localPosition;
            line.transform.localRotation = Quaternion.identity;
            // FirePointの座標に、Indicatorの長さの半分(5.0など)を足して設定する
            float offsetZ = line.transform.localScale.z * 0.5f; // Indicator(Cylinder)の長さが10ならその半分を代入
            line.transform.localPosition = new Vector3(
            firePoint.localPosition.x,
            firePoint.localPosition.y + 0.1f, //y座標はCylinder部分に合わせる
            offsetZ
    );

            Debug.Log($"[Indicator上書き後] 位置: {line.transform.localPosition}, 回転: {line.transform.localEulerAngles}");
            float t = 0;
            while (t < 2.0f)
            {
                line.SetActive(!line.activeSelf);
                yield return new WaitForSeconds(0.1f);
                t += 0.1f;
            }

            line.SetActive(false);
            FireBall();
            Destroy(line, 0.1f);
        }
    }

    void FireBall()
    {
        // 1. そもそもプレハブが空なら何もしない（安全策）
        if (ballPrefab == null) return;

        // 2. ボールを生成（1回だけ！）
        GameObject ball = Instantiate(ballPrefab, firePoint.position, Quaternion.identity);

        // 3. 必要なコンポーネントを取得
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        Ball ballScript = ball.GetComponent<Ball>();

        if (rb != null && ballScript != null)
        {
            // 4. 方向は「大砲の出口の正面」へ、速度はボールの「burstSpeed」で発射！
            rb.linearVelocity = firePoint.forward * ballScript.burstSpeed;
        }
    }
}