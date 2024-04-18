using System;
using System.Threading.Tasks;
using Common;
using UnityEngine.SceneManagement;

namespace Loading
{
    public class MenuLoadingOperation : ILoadingOperation
    {
        //public string GetName => "Main menu loading...";
        public string Description => "Загрузка меню...";

        public async Task Load(Action<float> onProgress)
        {
            onProgress?.Invoke(0.5f);
            //var loadOp = SceneManager.LoadSceneAsync(Constants.Scenes.MAIN_MENU, LoadSceneMode.Additive);
            var loadOp = SceneManager.LoadSceneAsync(Constants.Scenes.MAIN_MENU,
                LoadSceneMode.Additive);
            while (loadOp.isDone == false)
            {
                await Task.Delay(1);
            }
            onProgress?.Invoke(1f);
        }
    }
}