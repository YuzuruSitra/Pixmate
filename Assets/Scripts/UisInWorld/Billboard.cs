using UnityEngine;

/// <summary>
/// 常にカメラの方を向くオブジェクト回転をカメラに固定
/// </summary>
public class Billboard : MonoBehaviour 
{
	private Quaternion _currentRot;

	private void LateUpdate() 
	{
		// 回転をカメラと同期させる
		if(_currentRot == Camera.main.transform.rotation) return;
		transform.rotation = Camera.main.transform.rotation;
		_currentRot = Camera.main.transform.rotation;
	}
}
