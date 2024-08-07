namespace _TDK.Common
{
    public interface IAnalyticService
    {
        public void SendActionButtonPressedEvent(string buttonName);
        public void SendPlayerLoginEvent();

        public void SendContentStartedEvent(string contentName);
        public void SendContentEndedEvent(string contentName, bool isContentEndedByPauseMenu = false);

        public void LicensePurchased(string licenseSku, string price);
        public void LicenseConsumed(string licenseType);
    }
}