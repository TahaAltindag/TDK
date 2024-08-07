using _TDK.Core;

namespace _TDK.Common.AuthEvents
{
    #region Authentication Events

    public struct UnityAuthSignedIn : IEvent
    {
    }

    public struct UnityAuthSignedOut : IEvent
    {
    }

    public struct UnityAuthSignInFailed : IEvent
    {
        public string err;
    }

    public struct UnityAuthExpired : IEvent
    {
    }

    #endregion
}