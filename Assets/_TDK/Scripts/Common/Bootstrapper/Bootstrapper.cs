using _TDK.Core;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UT.UT_ImmersiveBreathing.Scripts;

namespace _TDK.Common
{
    public class Bootstrapper : SerializedMonoBehaviour
    {
        [Header("Services")] 
        [SerializeField] private IUnityCloudService unityCloudService;
        private IRemoteConfigService _remoteConfigService;
        private IUnityAuthService _unityAuthService;
        private IInternetConnectionCheckingService _internetConnectionService;
        private IAnalyticService _analyticService;
        private IUniversalTechnicService _universalTechnicService;

        private void Awake()
        {
            ServiceLocator.Global
                .Register<IInternetConnectionCheckingService>(new InternetConnectionCheckingService())
                .Register<IUnityCloudService>(unityCloudService = ScriptableObject.CreateInstance<UnityCloudService>())
                .Register<IAnalyticService>(new AnalyticService())
                .Register<IUnityAuthService>(new UnityAuthService())
                .Register<IRemoteConfigService>(new RemoteConfigService())
                .Register<IUniversalTechnicService>(new UniversalTechnicService());
        }

        private async UniTaskVoid Start()
        {
            ServiceLocator.For(this)
                .Get(out _internetConnectionService)
                .Get(out unityCloudService)
                .Get(out _analyticService)
                .Get(out _unityAuthService)
                .Get(out _remoteConfigService)
                .Get(out _universalTechnicService);


            await _internetConnectionService.IsInternetValid();
            unityCloudService.SetTargetCloudEnvironment();
            await unityCloudService.InitUnityServices();
            await _unityAuthService.Init();
            await _remoteConfigService.Init();
        }

        [Button]
        public async UniTask Instantiate()
        {
            
            var ib =
                await _universalTechnicService.InstantiateUniversalTechnic<ImmersiveBreathing>(
                    UTType.ImmersiveBreathing);

            ib
                .SetLoopCount(5)
                .SetBreathInDuration(3)
                .SetHoldDuration(2)
                .SetBreathOutDuration(6)
                .SetFadeOutDuration(2)
                .SetIsInfiniteLoop(false)
                .SetDistanceAboveHead(2)
                .SetTransformInFrontOfThePlayer(true)
                .SetDistance(3)
                .StartImmersiveBreathing().Forget();

        }

        [Button]
        public void Release()
        {
            _universalTechnicService.UnloadAllAddressableUniversalTechnics();
        }
    }
}