using System.Collections.Generic;
using Loading;
using UnityEngine;

public class AppStartup : MonoBehaviour
{
    private void Start()
    {
        var loadingOperations = new Queue<ILoadingOperation>();
        loadingOperations.Enqueue(new ConfigOperation());
        loadingOperations.Enqueue(new MenuLoadingOperation());
        LoadingScreen.Instance.Load(loadingOperations);
    }
}