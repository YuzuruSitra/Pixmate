using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInstantiater : MonoBehaviour {
    
    private Vector2 _displayCenter;

    private Vector3 _pos;

    void Start () 
    {
        // 画面中央の平面座標を取得する
        _displayCenter = new Vector2(Screen.width / 2, Screen.height / 2);
	}

    public void OutRay(GameObject predictInsCube, GameObject predictMatCube)
    {
        // カメラからのレイを画面中央の平面座標から飛ばす
        Ray ray = Camera.main.ScreenPointToRay(_displayCenter);
        // 当たったオブジェクト情報を格納する変数
        RaycastHit hit;

        // Physics.Raycast() でレイを飛ばす
        if (Physics.Raycast(ray, out hit, 3f)) {
            // 生成位置の変数の値を「ブロックの向き + ブロックの位置」
            _pos = hit.normal + hit.collider.transform.position;
            // 予測座標-修正予定
            predictInsCube.transform.position = _pos;
            predictMatCube.transform.position = hit.collider.transform.position;
            
            // すでに置いてあるオブジェクトには置けなくする

            // 触れているオブジェクトの取得
            CubeBunker.InstanceCubeBunker.NowHaveCube = hit.collider.gameObject;

            // 左クリック
            if (Input.GetKeyDown("c")) {
                // ↓ レイが当たっているオブジェクトを削除
                Destroy(hit.collider.gameObject);
            }
        }
    }
    
    // オブジェクトの生成
    public void GenerateCube(GameObject blockPrefab)
    {
        Instantiate(blockPrefab, _pos, Quaternion.identity);
    }
}
