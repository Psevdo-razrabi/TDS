using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Loader
{
    public bool IsLoad { get; private set; } = false;
    public async UniTask<T> LoadResources<T>(string nameResources)
    {   
        UniTaskCompletionSource<T> isTaskCompletion = new ();
        
        try
        {
            AsyncOperationHandle<T> operationHandle = Addressables.LoadAssetAsync<T>(nameResources);
            await operationHandle.Task.AsUniTask();

            if (operationHandle.Status == AsyncOperationStatus.Succeeded)
            {
                isTaskCompletion.TrySetResult(operationHandle.Result);
                IsLoad = true;
            }
            else isTaskCompletion.TrySetException(new Exception("Failed load asset"));
        }
        catch (Exception exception)
        {
            isTaskCompletion.TrySetException(exception);
        }

        return await isTaskCompletion.Task;
    }
}
