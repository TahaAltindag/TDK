using Cysharp.Threading.Tasks;

namespace _TDK.Common
{
    public interface IUniversalTechnicService
    {
        public UniTask<T> InstantiateUniversalTechnic<T>(UTType utType) where T : IUniversalTechnic;
        public void UnloadAllAddressableUniversalTechnics();
    }
}