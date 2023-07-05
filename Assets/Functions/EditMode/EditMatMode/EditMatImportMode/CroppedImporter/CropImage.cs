using System;
using UnityEngine;
using UnityEngine.UI;

public class CropImage : MonoBehaviour
{
    private Image _imageToCrop;
    private Vector2 _cropOffset = Vector2.zero;
    private Vector2 _croppedSize;

    [SerializeField]
    private float _moveSpeed = 100f;
    [SerializeField]
    private float _scaleSpeed = 100f;

    private Texture2D _originalTexture;
    private Texture2D _croppedTexture;
    public Texture2D CroppedTexture => _croppedTexture;


    public void DoCropImage(GameObject cropFlame)
    {
        _imageToCrop = cropFlame.GetComponent<Image>();

        // 画像を読み込んでテクスチャとして取得する
        _originalTexture = (Texture2D)_imageToCrop.mainTexture;

        // アスペクト比1:1を保てる最大サイズを計算する
        int minDimension = Mathf.Min(_originalTexture.width, _originalTexture.height);
        _croppedSize = new Vector2(minDimension, minDimension);

        DoCrop(_originalTexture, _cropOffset, _croppedSize);
    }

    public void CropInput(bool active)
    {
        if (!active) return;

        Vector2 prevCropOffset = _cropOffset;
        Vector2 prevCroppedSize = _croppedSize;
        bool keyInput = false;

        float speedFactor = Mathf.Max(_croppedSize.x, _croppedSize.y) / 1000f;

        float speedAdjusted = _moveSpeed * speedFactor;
        float scaleSpeedAdjusted = _scaleSpeed * speedFactor;

        #if UNITY_EDITOR
            if (Input.GetKey(KeyCode.LeftArrow)) {
                _cropOffset.x -= speedAdjusted * Time.deltaTime;
                keyInput = true;
            }
            if (Input.GetKey(KeyCode.RightArrow)) {
                _cropOffset.x += speedAdjusted * Time.deltaTime;
                keyInput = true;
            }
            if (Input.GetKey(KeyCode.UpArrow)) {
                _cropOffset.y += speedAdjusted * Time.deltaTime;
                keyInput = true;
            }
            if (Input.GetKey(KeyCode.DownArrow)) {
                _cropOffset.y -= speedAdjusted * Time.deltaTime;
                keyInput = true;
            }

            float newWidth = _croppedSize.x;
            float newHeight = _croppedSize.y;

            if (Input.GetKey(KeyCode.W)) {
                newWidth += scaleSpeedAdjusted * Time.deltaTime;
                newHeight += scaleSpeedAdjusted * Time.deltaTime;
                keyInput = true;
            }
            if (Input.GetKey(KeyCode.S)) {
                newWidth -= scaleSpeedAdjusted * Time.deltaTime;
                newHeight -= scaleSpeedAdjusted * Time.deltaTime;
                keyInput = true;
            }

            if (newWidth <= _originalTexture.width && newHeight <= _originalTexture.height)
            {
                _croppedSize.x = newWidth;
                _croppedSize.y = newHeight;
            }

        #elif UNITY_IOS

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved)
                {
                    Vector2 touchDeltaPosition = touch.deltaPosition;
                    _cropOffset.x -= touchDeltaPosition.x * speedAdjusted * Time.deltaTime;
                    _cropOffset.y += touchDeltaPosition.y * speedAdjusted * Time.deltaTime;
                    keyInput = true;
                }
            }

            if (Input.touchCount == 2)
            {
                Touch touch0 = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);

                Vector2 prevTouch0Pos = touch0.position - touch0.deltaPosition;
                Vector2 prevTouch1Pos = touch1.position - touch1.deltaPosition;
                float prevTouchDeltaMag = (prevTouch0Pos - prevTouch1Pos).magnitude;
                float touchDeltaMag = (touch0.position - touch1.position).magnitude;

                float deltaMagnitudeDiff = touchDeltaMag - prevTouchDeltaMag;

                float newWidth = _croppedSize.x - deltaMagnitudeDiff * scaleSpeedAdjusted * Time.deltaTime;
                float newHeight = _croppedSize.y - deltaMagnitudeDiff * scaleSpeedAdjusted * Time.deltaTime;

                if (newWidth <= _originalTexture.width && newHeight <= _originalTexture.height)
                {
                    _croppedSize.x = newWidth;
                    _croppedSize.y = newHeight;
                }
                keyInput = true;
            }

        #endif

        _croppedSize.x = Mathf.Clamp(_croppedSize.x, 1, _originalTexture.width);
        _croppedSize.y = Mathf.Clamp(_croppedSize.y, 1, _originalTexture.height);
        _cropOffset.x = Mathf.Clamp(_cropOffset.x, 0, _originalTexture.width - _croppedSize.x);
        _cropOffset.y = Mathf.Clamp(_cropOffset.y, 0, _originalTexture.height - _croppedSize.y);

        if (keyInput && (prevCropOffset != _cropOffset || prevCroppedSize != _croppedSize)) {
            DoCrop(_originalTexture, _cropOffset, _croppedSize);
        }
    }

    void DoCrop(Texture2D sourceTexture, Vector2 cropLange, Vector2 croppedSize)
    {
        // クロップ範囲を設定する
        int startX = Mathf.Clamp((int)_cropOffset.x, 0, sourceTexture.width - (int)croppedSize.x);
        int startY = Mathf.Clamp((int)_cropOffset.y, 0, sourceTexture.height - (int)croppedSize.y);

        // テクスチャをクロップする
        _croppedTexture = new Texture2D((int)croppedSize.x, (int)croppedSize.y, TextureFormat.RGBA32, false);

        // ソーステクスチャからクロップ範囲のピクセルデータを一度に取得
        Color[] pixels = sourceTexture.GetPixels(startX, startY, (int)croppedSize.x, (int)croppedSize.y);

        // クロップされたテクスチャにピクセルデータを一度に設定
        _croppedTexture.SetPixels(pixels);
        _croppedTexture.Apply();

        // クロップされたテクスチャを表示する
        _imageToCrop.sprite = Sprite.Create(_croppedTexture, new Rect(0f, 0f, _croppedTexture.width, _croppedTexture.height), new Vector2(0.5f, 0.5f));
    }

    // 左右反転処理
    public void ReverseTexture2D()
    {
        // 元のテクスチャのピクセルデータを取得
        Color32[] pixels = _originalTexture.GetPixels32();
        int width = _originalTexture.width;
        int height = _originalTexture.height;

        // 反転後のピクセルデータを格納する配列
        Color32[] flippedPixels = new Color32[pixels.Length];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // 元の位置のインデックス
                int index = y * width + x;
                // 反転後の位置のインデックス
                int flippedIndex = y * width + (width - 1 - x);
                // ピクセルデータを反転後の位置に格納
                flippedPixels[flippedIndex] = pixels[index];
            }
        }
        // 反転後のピクセルデータをテクスチャに設定
        _originalTexture.SetPixels32(flippedPixels);
        // テクスチャの変更を適用
        _originalTexture.Apply();

        DoCrop(_originalTexture, _cropOffset, _croppedSize);
    }

    // 反時計回り90度回転
    public void RotateTexture2D()
    {
        // 元のテクスチャのピクセルデータを取得
        Color32[] pixels = _originalTexture.GetPixels32();
        int width = _originalTexture.width;  
        int height = _originalTexture.height;

        // 回転後のピクセルデータを格納する配列
        Color32[] rotatedPixels = new Color32[pixels.Length]; 

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // 元の位置のインデックス
                int index = y * width + x;  
                // 回転後の位置のインデックス
                int rotatedIndex = x * height + (height - 1 - y);
                // ピクセルデータを回転後の位置に格納
                rotatedPixels[rotatedIndex] = pixels[index];
            }
        }
        // テクスチャのサイズを回転後のサイズに変更
        _originalTexture.Reinitialize(height, width);
        // 回転後のピクセルデータをテクスチャに設定
        _originalTexture.SetPixels32(rotatedPixels); 
        // テクスチャの変更を適用
        _originalTexture.Apply();

        DoCrop(_originalTexture, _cropOffset, _croppedSize);
    }


}
