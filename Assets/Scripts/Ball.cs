using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed = 5.0f; 
    public float maxSpeed = 20.0f; // スピードの上限
    public float speedIncreaseRate = 0.1f; // 1秒間に増える速さ
    public float burstSpeed = 30.0f; //発射時の速度
    public bool hasHitWall = false; // 壁に当たったかどうかのフラグ
    [Range(0f, 0.2f)] public float randomness = 0.1f;
    
    public GameObject hitEffectPrefab;
    private Rigidbody myRigidbody;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        //Vector3 direction = new Vector3(Random.Range(0.5f, 1f), 0, Random.Range(0.5f, 1f)).normalized;
        // myRigidbody.linearVelocity = direction * speed;
    }

    void Update()
    {
        // 時間とともにスピードをじわじわ上げる
        if (speed < maxSpeed)
        {
            speed += speedIncreaseRate * Time.deltaTime;
        }
    }

void FixedUpdate()
{
    // 1. カウントダウン中は停止
    if (GameManager.instance != null && !GameManager.instance.isGameStarted)
    {
        myRigidbody.linearVelocity = Vector3.zero;
        return;
    }

    // 2. 速度がゼロ（静止状態）なら何もしない（発射はCannonがやってくれるので）
    if (myRigidbody.linearVelocity == Vector3.zero) return;

    // 3. 速度を一定に保つ処理
    // 壁に当たった後は speed (5~20)、当たる前は burstSpeed (30) を維持
    float currentTargetSpeed = hasHitWall ? speed : burstSpeed;
    myRigidbody.linearVelocity = myRigidbody.linearVelocity.normalized * currentTargetSpeed;
}

    void OnCollisionEnter(Collision collision)
    {
        if(!hasHitWall){
            hasHitWall = true; // 最初の壁衝突を記録
        }
        // 反射の計算
        Vector3 currentVelocity = myRigidbody.linearVelocity;
        float deviation = Random.Range(-randomness, randomness);
        Quaternion bounceRot = Quaternion.Euler(0, deviation * Mathf.Rad2Deg, 0);
        myRigidbody.linearVelocity = (bounceRot * currentVelocity).normalized * speed;

        // 脱落判定
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("CPU"))
        {
            if (hitEffectPrefab != null)
            {
                ContactPoint contact = collision.contacts[0];
                Quaternion effectRot = Quaternion.FromToRotation(Vector3.up, contact.normal);
                GameObject effect = Instantiate(hitEffectPrefab, contact.point, effectRot);
                Destroy(effect, 1.0f);
            }

            Destroy(collision.gameObject);
            if (GameManager.instance != null) GameManager.instance.CheckGameOver();
        }
    }
}