using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UniTaskTest
{
    public class UniTaskSample01 : MonoBehaviour
    {
        private async void Start()
        {
            await HelloWorldTask();
            
            // コールバックも続けて書ける
            Debug.Log("Finish!!");
        }
        
        private async UniTask HelloWorldTask()
        {
            await UniTask.Delay(5000);
            Debug.Log("Hello, World");
        }
    }
}