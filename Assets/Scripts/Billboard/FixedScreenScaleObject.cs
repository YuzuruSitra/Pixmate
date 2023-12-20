using UnityEngine;

// オブジェクトをカメラとの距離を考慮してスケーリングするクラス
public class FixedScreenScaleObject : MonoBehaviour {
	private const float INVAILD_BASE_SCALE = float.MinValue;
	[SerializeField]
	private float _baseScale = INVAILD_BASE_SCALE;

	private void Start() {
		if (_baseScale != INVAILD_BASE_SCALE) return;
		_baseScale = transform.localScale.x / GetDistance();
	}

	private float GetDistance() {
		return (transform.position - Camera.main.transform.position).magnitude;
	}

	private void LateUpdate() {
		transform.localScale = Vector3.one * _baseScale * GetDistance();
	}
}