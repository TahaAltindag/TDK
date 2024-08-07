using System.Collections.Generic;

namespace _TDK.Common
{
    /// <summary>
    /// The Fluent Builder Pattern is particularly useful here because it allows us to chain method calls in a readable manner. 
    /// </summary>
    public class AnalyticsEventBuilder
    {
        private Dictionary<string, object> _parameters;

        public AnalyticsEventBuilder()
        {
            _parameters = new Dictionary<string, object>();
        }

        public AnalyticsEventBuilder AddParameter(string key, object value)
        {
            _parameters[key] = value;
            return this;
        }

        public AnalyticsEventBuilder AddUserEmail(string userEmail)
        {
            _parameters[AnalyticConstants.p_UserEmail] = userEmail;
            return this;
        }

        public AnalyticsEventBuilder AddSceneName(string sceneName)
        {
            _parameters[AnalyticConstants.p_SceneName] = sceneName;
            return this;
        }

        public Dictionary<string, object> Build()
        {
            return _parameters;
        }
    }
}