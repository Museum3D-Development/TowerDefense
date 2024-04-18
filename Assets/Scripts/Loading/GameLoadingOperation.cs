using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using Common;

namespace Loading
{
    public class GameLoadingOperation : ILoadingOperation
    {
        //public string GetName => "Game loading...";
        public string Description => "Загрузка игры...";

        public async Task Load(Action<float> onProgress)
        {
            onProgress?.Invoke(0.5f);
            //var loadOp = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
            var loadOp = SceneManager.LoadSceneAsync(Constants.Scenes.GAME,
                LoadSceneMode.Single);
            while (loadOp.isDone == false)
            {
                //await Task.Delay(10);
                await Task.Delay(1);
            }
            onProgress?.Invoke(1f);
        }
    }
}