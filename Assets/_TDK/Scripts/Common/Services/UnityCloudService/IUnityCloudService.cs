using Cysharp.Threading.Tasks;

namespace _TDK.Common
{
    public interface IUnityCloudService
    {
        public string TargetEnvironmentID { get; }
        public bool IsValid { get; }
        public Environments TargetEnvironment { get; }
        public void SetTargetCloudEnvironment();
        public UniTask InitUnityServices();
    }
}