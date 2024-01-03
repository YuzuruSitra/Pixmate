using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 予測線の位置を担当するクラス
public class PredictionAdjuster : MonoBehaviour
{
    private GameObject _nowHaveCube;
    public  GameObject NowHaveCube => _nowHaveCube;

    [SerializeField]
    private GameObject _predictAdjCube;
    [SerializeField]
    private GameObject _predictSameCube;
    private bool _inLange;
    public bool InLange => _inLange;
    
    public Vector3 AdjCubePos => _predictAdjCube.transform.position;
    public Vector3 SameCubePos => _predictSameCube.transform.position;

    void Update()
    {
        MovePredictCube();
    }

    // 隣接オブジェクトへのRay
    void MovePredictCube()
    {
        // 画面中央の平面座標を取得する
        Vector2 displayCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        // カメラからのレイを画面中央の平面座標から飛ばす
        Ray ray = Camera.main.ScreenPointToRay(displayCenter);
        // 当たったオブジェクト情報を格納する変数
        RaycastHit hit;

        _inLange = false;
        // 対象レイヤーの指定
        int targetLayerMask = LayerMask.GetMask("TouchLayer");
        // Physics.Raycast() でレイを飛ばす
        if (Physics.Raycast(ray, out hit, 3f, targetLayerMask)) 
        {   
            _nowHaveCube = hit.collider.gameObject;
            // 生成位置の変数の値を「ブロックの向き + ブロックの位置」
            _predictAdjCube.transform.position = hit.normal + hit.collider.transform.position;
            // EditPredict
            _predictSameCube.transform.position = hit.collider.transform.position;
            
            // 対象の角度に合わせて予測線を回転
            _predictSameCube.transform.rotation = _nowHaveCube.transform.rotation;
            
            _inLange = true;
        }
    }
}
