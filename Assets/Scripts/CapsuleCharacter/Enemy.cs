using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float wanderSpeed = 4.0f;  // 普段のうろうろ
    public float escapeSpeed = 5.5f;  // 逃げる時（あまり速すぎないように）
    public float detectRange = 4.0f;  // これを短くすると当たりやすくなる
    public float wanderInterval = 1.5f;

    private Transform ballTarget; // 一番近いボールを狙う
    private Vector3 wanderDirection;
    private float wanderTimer;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetRandomDirection();
    }

    void Update()
    {
        wanderTimer += Time.deltaTime;
        if (wanderTimer >= wanderInterval)
        {
            SetRandomDirection();
            wanderTimer = 0;
        }
    }

    void FixedUpdate()
    {
        //ゲームが始まっていないなら何もしない（止まる）
        if (GameManager.instance != null && !GameManager.instance.isGameStarted)
        {
            // ボールの場合は、現在の速度を一時的に保存しておくなどの工夫が必要ですが、
            // ひとまず rb.linearVelocity = Vector3.zero; でも止まります。
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            return;
        }
        // フィールド上の全てのボールを探し、一番近いやつを警戒する
        FindClosestBall();

        if (ballTarget != null)
        {
            float distance = Vector3.Distance(transform.position, ballTarget.position);

            if (distance < detectRange)
            {
                // 逃走
                Vector3 escapeDir = (transform.position - ballTarget.position).normalized;
                rb.linearVelocity = new Vector3(escapeDir.x, 0, escapeDir.z) * escapeSpeed;
                return;
            }
        }

        // 普段のうろうろ
        rb.linearVelocity = wanderDirection * wanderSpeed;
    }

    void FindClosestBall()
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        float closestDistance = Mathf.Infinity;
        ballTarget = null;

        foreach (GameObject b in balls)
        {
            float dist = Vector3.Distance(transform.position, b.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                ballTarget = b.transform;
            }
        }
    }

    void SetRandomDirection()
    {
        float angle = Random.Range(0, 360f);
        wanderDirection = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)).normalized;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall")) SetRandomDirection();
    }
}