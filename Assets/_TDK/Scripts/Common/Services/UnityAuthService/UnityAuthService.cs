using System;
using _TDK.Common.AuthEvents;
using _TDK.Core;
using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace _TDK.Common
{
    public class UnityAuthService : IUnityAuthService
    {
        #region Fields

        private IAnalyticService _analyticService;
        private IInternetConnectionCheckingService _internetConnectionCheckingService;

        #endregion

        #region Public Methods

        public async UniTask Init()
        {
            ServiceLocator.Global
                .Get(out _analyticService)
                .Get(out _internetConnectionCheckingService);
            SetUpEvents();
            await SignInAnonymouslyAsync();
        }

        #endregion

        #region Private Methods

        private void SetUpEvents()
        {
            AuthenticationService.Instance.SignedIn += SignedIn;
            AuthenticationService.Instance.SignedOut += SignedOut;
            AuthenticationService.Instance.Expired += Expired;
            AuthenticationService.Instance.SignInFailed += SignInFailed;
        }

        private async UniTask SignInAnonymouslyAsync()
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Logman.Log("Sign in anonymously succeeded!");

                // Shows how to get the playerID
                Logman.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
            }
            catch (AuthenticationException ex)
            {
                // Compare error code to AuthenticationErrorCodes
                // Notify the player with the proper error message
                Logman.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                // Compare error code to CommonErrorCodes
                // Notify the player with the proper error message
                if (await _internetConnectionCheckingService.IsInternetValid(InternetConnectionCheckingService.Urls
                        .UnityUrl))
                {
                    Logman.LogException(ex);
                }
            }
        }


        private void SignedIn()
        {
            EventBus<UnityAuthSignedIn>.Raise(new UnityAuthSignedIn());
            Logman.Log($"Player Signed In to Unity with ID: {AuthenticationService.Instance.PlayerId}");
            _analyticService.SendPlayerLoginEvent();
        }

        private void SignedOut()
        {
            EventBus<UnityAuthSignedOut>.Raise(new UnityAuthSignedOut());
            Logman.Log($"Player Signed Out to Unity with ID: {AuthenticationService.Instance.PlayerId}");
        }

        private void Expired()
        {
            EventBus<UnityAuthExpired>.Raise(new UnityAuthExpired());
            Logman.LogError("Unity Auth Connection Expired");
        }

        private void SignInFailed(Exception error)
        {
            EventBus<UnityAuthSignInFailed>.Raise(new UnityAuthSignInFailed { err = error.Message });
            Logman.LogError($"Player Sign In Failed with a message : {error.Message}");
        }

        #endregion
    }
}