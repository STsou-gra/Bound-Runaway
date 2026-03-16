using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 7.0f;
    private Rigidbody rb;
    private Vector3 moveInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //キーボードの入力を受け取る
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        //動きたい方向を計算
        moveInput = new Vector3(moveX, 0, moveZ).normalized;
    }

    void FixedUpdate(){
        //ゲームが始まっていないなら何もしない（止まる）
        if (GameManager.instance != null && !GameManager.instance.isGameStarted)
        {
            // ボールの場合は、現在の速度を一時的に保存しておくなどの工夫が必要ですが、
            // ひとまず rb.linearVelocity = Vector3.zero; でも止まります。
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            return;
        }
        Debug.Log("現在の入力: " + moveInput);//デバッグ
       //物理演算を使って移動させる
       rb.linearVelocity = moveInput * moveSpeed;
        /*Vector3 velocity = moveInput * moveSpeed;
        velocity.y = rb.linearVelocity.y; 
        rb.linearVelocity = velocity;*/
    }
}
