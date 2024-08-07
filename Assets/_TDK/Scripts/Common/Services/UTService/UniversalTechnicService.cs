using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _TDK.Common
{
    public class UniversalTechnicService : IUniversalTechnicService
    {
        private List<AsyncOperationHandle> _instantiatedUniversalTechnics;

        private Lazy<List<AsyncOperationHandle>> _lazyInstantiatedUniversalTechnics =
            new Lazy<List<AsyncOperationHandle>>();

        public async UniTask<T> LoadAddressableUniversalTechnicAsync<T>(UTType utType) where T : IUniversalTechnic
        {
            throw new NotImplementedException();
        }

        public async UniTask<T> InstantiateUniversalTechnic<T>(UTType utType) where T : IUniversalTechnic
        {
            try
            {
                //Make sure that the UT's addressable label has the same typo with UTType
                var handler =
                    Addressables.InstantiateAsync(Enum.GetName(typeof(UTType), utType) /*, trackHandle: false*/);
                await handler.Task;
                Logman.Log($"UT typeof {handler.Result} instantiated successfully ");
                _lazyInstantiatedUniversalTechnics.Value.Add(handler);
                var ut = handler.Result.GetComponent<IUniversalTechnic>();
                return (T)ut;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void UnloadAllAddressableUniversalTechnics()
        {
            if (_lazyInstantiatedUniversalTechnics.IsValueCreated == false) return;
            if (_lazyInstantiatedUniversalTechnics.Value.Count == 0) return;
            Logman.Log("Unloading all addressable universal technics");
            foreach (var handle in _lazyInstantiatedUniversalTechnics.Value)
            {
                Addressables.ReleaseInstance(handle);
            }
            
            _lazyInstantiatedUniversalTechnics.Value.Clear();
            _lazyInstantiatedUniversalTechnics = null;
        }
    }
}