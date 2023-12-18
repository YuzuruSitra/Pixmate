using UnityEngine;

public class SeaFollowPlayer : MonoBehaviour
{
    [SerializeField]
    private Transform _player;
    private Vector3 _targetPosition = new Vector3(0, 0.25f, 0);

    void Update()
    {
        _targetPosition.x = _player.position.x;
        _targetPosition.z = _player.position.z;
        this.transform.position = _targetPosition;
    }
}
