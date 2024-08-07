using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace _TDK.Common
{
    public class InternetConnectionCheckingService : IInternetConnectionCheckingService
    {
        #region Fields

        private const string UnityUrl = "http://unity3d.com";
        private const string AmazonUrl = "https://s3.amazonaws.com";

        public enum Urls
        {
            AmazonUrl,
            UnityUrl
        }

        #endregion

        #region Public Methods

        public async UniTask<bool> IsInternetValid(Urls url = Urls.AmazonUrl)
        {
            if (!NetworkReachabilityCheck()) return false;

            return await SendRequest(url);
        }

        #endregion


        #region Private Methods

        private bool NetworkReachabilityCheck()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Logman.LogError("Network Not Reachable");
                return false;
            }

            return true;
        }

        private async UniTask<bool> SendRequest(Urls url)
        {
            var _url = url switch
            {
                Urls.AmazonUrl => AmazonUrl,
                Urls.UnityUrl => UnityUrl,
                _ => AmazonUrl
            };
            Logman.Log($"Sending request to {_url}");

            var request = new UnityWebRequest(_url);
            request.timeout = 20;
            try
            {
                await request.SendWebRequest();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Logman.Log("Data:" + request.result);
            if (request.error != null)
            {
                Logman.LogError("Connection Error");
                return false;
            }

            if (request.result == UnityWebRequest.Result.Success && request.responseCode == 200)
            {
                Logman.Log("Url reachable");
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}