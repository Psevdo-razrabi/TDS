using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Object = UnityEngine.Object;

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

    public async UniTask<T> LoadResourcesUsingReference<T>(AssetReferenceT<T> resource) where T : Object
    {
        UniTaskCompletionSource<T> isTaskComplete = new();

        try
        {
            var operationHandle = resource.LoadAssetAsync();
            await operationHandle;

            if (operationHandle.Status == AsyncOperationStatus.Succeeded)
            {
                isTaskComplete.TrySetResult(operationHandle.Result);
            }

            isTaskComplete.TrySetException(new Exception("Failed load asset"));
        }
        catch (Exception e)
        {
            isTaskComplete.TrySetException(e);
        }

        return await isTaskComplete.Task;
    }

    public async UniTask<List<T>> LoadAllResourcesUseLabel<T>(AssetLabelReference labelReference) where T : Object
    {
        UniTaskCompletionSource<List<T>> isTaskCompletionSource = new();

        try
        {
            var operationHandler = Addressables.LoadAssetsAsync<T>(labelReference,
                (objectLoad) => { Debug.Log($"{objectLoad.GetType()} is load"); });

            await operationHandler;

            if (operationHandler.Status == AsyncOperationStatus.Succeeded)
            {
                isTaskCompletionSource.TrySetResult((List<T>)operationHandler.Result);
            }

            isTaskCompletionSource.TrySetException(new Exception("Failed load asset"));
        }
        catch (Exception e)
        {
            isTaskCompletionSource.TrySetException(e);
        }

        return await isTaskCompletionSource.Task;
    } 
    
    public void ClearMemory<T>(T objectClear)
    {
        Addressables.Release(objectClear);
    }

    public void ClearMemoryInstance(GameObject objectClear)
    {
        Addressables.ReleaseInstance(objectClear);
    }
}
