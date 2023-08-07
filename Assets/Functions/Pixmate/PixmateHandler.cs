using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pixmates
{
    public class PixmateHandler : MonoBehaviour
    {
        public enum PixmateAiState
        {
            WAIT,
            MOVE,
            Jump,
            Avoid
        }

        private PixmateAiState _currentState;

        public event Action<PixmateAiState> OnAIStateChanged;

        public void ChangeAIState(PixmateAiState newState)
        {
            // 同じステートを弾く
            if(_currentState == newState) return;
            _currentState = newState;
            OnAIStateChanged?.Invoke(_currentState);
        }

        // 目の前にオブジェクトがある時の処理
        public void HitRayActiion(GameObject hitObj)
        {
            string hitObjTag = hitObj.tag;
            switch (hitObjTag)
            {   
                case "Player":
                    break;
                default:
                    Vector3 hitObjOrigin = hitObj.transform.position;
                    Vector3 hitObjDirection = new Vector3(0.0f, 1.0f, 0.0f);
                    float rayLength = 1.0f;
                    Ray hitObjRay = new Ray(hitObjOrigin, hitObjDirection * rayLength);
                    // 前に障害物があり縦1マスならジャンプ、違う場合避ける
                    Pixmates.PixmateHandler.PixmateAiState state = Pixmates.PixmateHandler.PixmateAiState.Jump;
                    if (Physics.Raycast(hitObjRay, out RaycastHit hitObjHit)) state = Pixmates.PixmateHandler.PixmateAiState.Avoid;

                    ChangeAIState(state);

                    Debug.DrawRay(hitObjRay.origin, hitObjRay.direction * 1, Color.blue);
                    break;
            }
        }

    }
}