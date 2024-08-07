using System.Threading;
using Cysharp.Threading.Tasks;

namespace _TDK.Common
{
    public interface IAddressableBundleDownloadingService
    {
        public UniTask<bool> CheckSceneInCatalog(string bundleLabel, bool isEpisode);
        public UniTask<long> GetDownloadSize(string bundleLabel);
        public UniTask DownloadBundle(string bundleLabel, CancellationToken cancellationToken = default);
    }
}