using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _TDK.Common
{
    public class AnalyticService : IAnalyticService
    {
        #region Fields

        private float _contentStartTime = 0f;
        private string UserEmail => PlayerPrefs.GetString(Constants.PlayerEmailKey, "null_email");

        #endregion

        #region Public Methods

        /// <summary>
        /// The ActionButtonPressed event is used to see what buttons are pressed how often and under what conditions.
        /// In Data Explorer on the Unity Dashboard, filter to any individual parameter, including different
        /// combinations of the same data (like buttonNameBySceneName, buttonNameByABGroup, and
        /// buttonNameBySceneNameAndABGroup) so that you can view these combinations in Data Explorer at a glance.
        /// Alternatively, you can include the single item parameters (i.e. buttonName, sceneName, and abGroup)
        /// and do advanced analysis on them using Data Export.
        /// </summary>
        /// <param name="buttonName"></param>
        public void SendActionButtonPressedEvent(string buttonName)
        {
            var parameters = new AnalyticsEventBuilder()
                .AddParameter(AnalyticConstants.p_ButtonName, buttonName)
                .AddSceneName(GetSceneName())
                .AddUserEmail(UserEmail)
                .AddParameter(AnalyticConstants.p_ButtonNameByUserEmail, $"{buttonName} - {UserEmail}")
                .Build();

            SendEvent(AnalyticConstants.e_ActionButtonPressed, parameters);
        }

        public void SendPlayerLoginEvent()
        {
            var userID = AnalyticsService.Instance.GetAnalyticsUserID();
            var parameters = new AnalyticsEventBuilder()
                .AddUserEmail(UserEmail)
                .AddParameter(AnalyticConstants.p_UserIDByUserEmail, $"{userID} - {UserEmail}")
                .Build();

            SendEvent(AnalyticConstants.e_PlayerLogin, parameters);
        }

        public void SendContentStartedEvent(string contentName)
        {
            _contentStartTime = Time.time;
            var parameters = new AnalyticsEventBuilder()
                .AddSceneName(GetSceneName())
                .AddUserEmail(UserEmail)
                .AddParameter(AnalyticConstants.p_ContentName, contentName)
                .Build();

            SendEvent(AnalyticConstants.e_ContentStarted, parameters);
        }

        public void SendContentEndedEvent(string contentName, bool isContentEndedByPauseMenu = false)
        {
            float contentEndTime = Time.time;
            float timeSpentInContent = contentEndTime - _contentStartTime;
            var parameters = new AnalyticsEventBuilder()
                .AddSceneName(GetSceneName())
                .AddUserEmail(UserEmail)
                .AddParameter(AnalyticConstants.p_ContentName, contentName)
                .AddParameter(AnalyticConstants.p_TimeSpentInContent, timeSpentInContent)
                .AddParameter(AnalyticConstants.p_IsContentEndedByPauseMenu, isContentEndedByPauseMenu)
                .AddParameter(AnalyticConstants.p_TimeSpentInContentByContentName,
                    $"{timeSpentInContent} - {contentName}")
                .Build();

            SendEvent(AnalyticConstants.e_ContentEnded, parameters);
        }

        public void LicensePurchased(string licenseSku, string price)
        {
            var parameters = new AnalyticsEventBuilder()
                .AddUserEmail(UserEmail)
                .AddParameter(AnalyticConstants.p_LicenseSku, licenseSku)
                .AddParameter(AnalyticConstants.p_LicensePrice, price)
                .AddParameter(AnalyticConstants.p_LicensePurchaseDate, DateTime.Now.ToUniversalTime().ToString())
                .AddParameter(AnalyticConstants.p_LicensePurchaseByEmail, $"{licenseSku} - {price} - {UserEmail}")
                .Build();

            SendEvent(AnalyticConstants.e_LicensePurchased, parameters);
        }

        public void LicenseConsumed(string licenseType)
        {
            var parameters = new AnalyticsEventBuilder()
                .AddUserEmail(UserEmail)
                .AddParameter(AnalyticConstants.p_LicenseSku, licenseType)
                .AddParameter(AnalyticConstants.p_LicenseConsumeDate, DateTime.Now.ToUniversalTime().ToString())
                .AddParameter(AnalyticConstants.p_LicenseConsumeByEmail, $"{licenseType} - {UserEmail}")
                .Build();

            SendEvent(AnalyticConstants.e_LicenseConsumed, parameters);
        }

        #endregion

        //ToDo:Change this
        private string GetSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }

        private void SendEvent(string eventName, Dictionary<string, object> parameters)
        {
            AnalyticsService.Instance.CustomData(eventName, parameters);
            AnalyticsService.Instance.Flush();
            Logman.Log($"{eventName} Event Sent! ");
            foreach (var parameter in parameters)
            {
                Logman.Log($"{parameter.Key} {parameter.Value}");
            }
        }
    }
}