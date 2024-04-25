using System;
using System.Threading.Tasks;
using Common;
using UnityEngine.SceneManagement;

namespace Loading
{
    public class LevelMenuLoadingOperation : ILoadingOperation
    {
        public string Description => "Загрузка уровней...";

        public async Task Load(Action<float> onProgress)
        {
            onProgress?.Invoke(0.5f);
            var loadOp = SceneManager.LoadSceneAsync(Constants.Scenes.LEVEL_MENU,
                LoadSceneMode.Single);
            while (loadOp.isDone == false)
            {
                await Task.Delay(1);
            }
            onProgress?.Invoke(1f);
        }
    }
}