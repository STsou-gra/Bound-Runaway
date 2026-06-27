using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       // 追いかける対象（Player）
    public Vector3 offset = new Vector3(0, 10, -10); // プレイヤーとの距離感
    public float smoothSpeed = 0.125f; // 追従の滑らかさ（0.1〜1.0）

    void LateUpdate()
    {
        if (target == null) return; // プレイヤーが消えたら何もしない

        // 理想的な目的地の計算
        Vector3 desiredPosition = target.position + offset;
        
        // 現在地から目的地まで滑らかに移動させる（線形補間）
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        
        // カメラの位置を更新
        transform.position = smoothedPosition;

        // 常にプレイヤーの方向を向く（必要に応じて）
        // transform.LookAt(target); 
    }
}