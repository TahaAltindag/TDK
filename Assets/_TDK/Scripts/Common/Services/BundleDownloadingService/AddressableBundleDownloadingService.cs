using Cysharp.Threading.Tasks;
using System.Threading;
using _TDK.Core;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


namespace _TDK.Common
{
    #region Events

    public struct OnDownloadPercentChanged : IEvent
    {
        public float percentage;
    }

    public struct OnDownloadCompleted : IEvent
    {
    }

    public struct OnDownloadFailed : IEvent
    {
    }

    public struct OnDownloadedMegaBytesChanged : IEvent
    {
        public string downloadedMB;
        public string totalMB;
    }

    #endregion

    public class AddressableBundleDownloadingService : IAddressableBundleDownloadingService
    {
        #region Fields

        private AsyncOperationHandle _downloadHandler;

        #endregion


        #region Public Methods

        public async UniTask<bool> CheckSceneInCatalog(string bundleLabel, bool isEpisode)
        {
            if (isEpisode)
            {
                bundleLabel += "_Scene";
            }

            var sceneSizeHandler = Addressables.GetDownloadSizeAsync(bundleLabel);
            await sceneSizeHandler.Task;
            if (sceneSizeHandler.Status == AsyncOperationStatus.Succeeded)
            {
                return true;
            }
            else
            {
                Logman.LogError(bundleLabel + " not found");
                return false;
            }
        }

        public async UniTask<long> GetDownloadSize(string bundleLabel)
        {
            var bundleSizeHandler = Addressables.GetDownloadSizeAsync(bundleLabel);
            await bundleSizeHandler.Task;

            if (bundleSizeHandler.Status == AsyncOperationStatus.Succeeded)
            {
                return bundleSizeHandler.Result;
            }
            else
            {
                return -1;
            }
        }

        public async UniTask DownloadBundle(string bundleLabel, CancellationToken cancellationToken = default)
        {
            _downloadHandler = Addressables.DownloadDependenciesAsync(bundleLabel);

            while (_downloadHandler.IsValid() && _downloadHandler.Status != AsyncOperationStatus.Succeeded)
            {
                EventBus<OnDownloadedMegaBytesChanged>.Raise(new OnDownloadedMegaBytesChanged
                {
                    downloadedMB = _downloadHandler.GetDownloadStatus().DownloadedBytes.ConvertBytesToMegabytes(),
                    totalMB = _downloadHandler.GetDownloadStatus().TotalBytes.ConvertBytesToMegabytes()
                });

                EventBus<OnDownloadPercentChanged>.Raise(new OnDownloadPercentChanged
                {
                    percentage = _downloadHandler.GetDownloadStatus().Percent
                });
                await UniTask.Yield(cancellationToken);
            }

            await _downloadHandler.Task;

            if (_downloadHandler.Status == AsyncOperationStatus.Succeeded)
            {
                Logman.Log("Download Succeeded");
                EventBus<OnDownloadCompleted>.Raise(new OnDownloadCompleted());
            }
            else
            {
                Logman.Log("Download Failed");
                EventBus<OnDownloadFailed>.Raise(new OnDownloadFailed());
            }

            Addressables.Release(_downloadHandler);
        }

        public void CancelDownloadOperation()
        {
            if (!_downloadHandler.IsValid()) return;
            _downloadHandler.ToUniTask().SuppressCancellationThrow();
            //_downloadHandler.Task.Dispose();
            Addressables.Release(_downloadHandler);
            Logman.Log("Download Bundle Operation Canceled");
        }

        #endregion
    }
}