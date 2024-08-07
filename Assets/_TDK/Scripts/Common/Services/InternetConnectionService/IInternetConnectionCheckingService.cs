using Cysharp.Threading.Tasks;

namespace _TDK.Common
{
    public interface IInternetConnectionCheckingService
    {
        
        public UniTask<bool> IsInternetValid(InternetConnectionCheckingService.Urls url = InternetConnectionCheckingService.Urls.AmazonUrl);
    }
}
