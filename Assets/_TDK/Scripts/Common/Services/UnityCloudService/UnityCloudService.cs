using System;
using _TDK.Core;
using Cysharp.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

namespace _TDK.Common
{
    public enum Environments
    {
        Development,
        Production
    }

    [CreateAssetMenu(fileName = "UnityCloudEnvironmentService",
        menuName = "TDK/Services/UnityCloudEnvironmentService")]
    [Serializable]
    public class UnityCloudService : ScriptableObject, IUnityCloudService
    {
        #region Fields

        [SerializeField] private Environments targetEnvironment;
        public Environments TargetEnvironment => targetEnvironment;

        public string TargetEnvironmentID
        {
            get
            {
                return targetEnvironment switch
                {
                    Environments.Development => DevEnvironmentID,
                    Environments.Production => ProdEnvironmentID,
                    _ => DevEnvironmentID
                };
            }
        }

        private const string DevelopmentEnvironmentName = "development";
        private const string ProductionEnvironmentName = "production";

        //todo set these ID's
        private const string DevEnvironmentID = "";
        private const string ProdEnvironmentID = "";

        private bool _isValid;
        private UniTask _init = default;
        private IInternetConnectionCheckingService _internetConnectionCheckingService;
        private InitializationOptions _options;

        #endregion

        public bool IsValid => _isValid;

        public void SetTargetCloudEnvironment()
        {
            _options = new InitializationOptions();
            _options.SetEnvironmentName(PickEnvironmentName(targetEnvironment));
            Debug.Log($"Target Environment set to {targetEnvironment}");
        }

        public async UniTask InitUnityServices()
        {
            try
            {
                _init = UnityServices.InitializeAsync(_options).AsUniTask();
                await _init;
                Debug.Log("Unity Services initialized");
            }
            catch (Exception e)
            {
                ServiceLocator.Global.Get(out _internetConnectionCheckingService);
                //check if the internet connection is valid
                if (await _internetConnectionCheckingService.IsInternetValid(InternetConnectionCheckingService.Urls
                        .UnityUrl))
                {
                    Debug.LogException(e);
                }
                //todo send message with pop up ui about there is no internet connection
            }

            _isValid = _init.Status == UniTaskStatus.Succeeded;
        }


        private string PickEnvironmentName(Environments environments)
        {
            var environmentName = environments switch
            {
                Environments.Development => DevelopmentEnvironmentName,
                Environments.Production => ProductionEnvironmentName,
                _ => ProductionEnvironmentName
            };

            return environmentName;
        }
    }
}