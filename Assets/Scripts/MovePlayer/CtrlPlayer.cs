using UnityEngine;

// 視点操作と移動を担当するクラス
public class CtrlPlayer : MonoBehaviour
{
    [SerializeField]
    private float _speed = 0.5f;

    private Rigidbody _rigidBody;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    public void CtrlMovePlayer(int calcX, int calcZ)
    {    
        // 入力に応じて移動ベクトルを作成
        Vector3 movement = new Vector3(calcX, 0f, calcZ) * _speed * Time.deltaTime;

        transform.position += transform.forward * movement.z + transform.right * movement.x;
    }

    public void MoveRot(float rotY)
    {
        Quaternion newRotation = Quaternion.Euler(0f, transform.localEulerAngles.y + rotY, 0f);
        transform.rotation = newRotation;
    }

}
