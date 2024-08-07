using System;
using _TDK.Core;
using Cysharp.Threading.Tasks;
using Unity.Services.RemoteConfig;
using UnityEngine;
using UnityRcs = Unity.Services.RemoteConfig.RemoteConfigService;

namespace _TDK.Common
{
    public struct UserAttributes
    {
    }

    public class RemoteConfigService : IRemoteConfigService
    {
        #region Fields

        private IUnityCloudService _ucs;
        private IInternetConnectionCheckingService _ics;
        public AppAttributes AppAttributes { get; private set; }

        #endregion

        #region Public Methods

        public async UniTask Init()
        {
            ServiceLocator.Global.Get(out _ucs);

            var rcs = UnityRcs.Instance;

            rcs.FetchCompleted += ApplyRemoteSettings;
            rcs.SetEnvironmentID(_ucs.TargetEnvironmentID);
            await FetchConfigs();
        }

        public async UniTask FetchConfigs()
        {
            try
            {
                UniTask fetchingTask = UnityRcs.Instance.FetchConfigsAsync(new UserAttributes(), new AppAttributes())
                    .AsUniTask();
                await fetchingTask;
            }
            catch (Exception e)
            {
                ServiceLocator.Global.Get(out _ics);
                if (await _ics.IsInternetValid())
                {
                    Logman.LogError(e);
                }
                else
                {
                    //todo pop up ui and stop upcoming tasks
                }
            }
        }

        #endregion

        #region Private Methods

        // Create a function to set your variables to their keyed values:
        void ApplyRemoteSettings(ConfigResponse configResponse)
        {
            // Conditionally update settings, depending on the response's origin:
            switch (configResponse.requestOrigin)
            {
                case ConfigOrigin.Default:
                    Logman.Log("No settings loaded this session; using default values.");
                    break;
                case ConfigOrigin.Cached:
                    Logman.Log("No settings loaded this session; using cached values from a previous session.");
                    break;
                case ConfigOrigin.Remote:
                    Logman.Log("New settings loaded this session; update values accordingly.");
                    SetConfigData();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void SetConfigData()
        {
            var attributes = AppAttributes;
            attributes.appVersion = UnityRcs.Instance.appConfig.GetString("appVersion");
            AppAttributes = attributes;
        }

        #endregion
    }
}