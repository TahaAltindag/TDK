using Cysharp.Threading.Tasks;

namespace _TDK.Common
{
    public interface IRemoteConfigService
    {
        public UniTask Init();
        public UniTask FetchConfigs();
    }
}