using UnityEngine;

// オブジェクトを常にカメラの方へ向けるクラス
public class BillboardAligner : MonoBehaviour 
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
