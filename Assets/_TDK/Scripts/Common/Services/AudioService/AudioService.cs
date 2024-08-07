using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;
using FMODUnity;
using System;
using Sirenix.OdinInspector;
using FMOD.Studio;
using System.Linq;

namespace _TDK.Common
{
    [Serializable]
    public struct InstanceAudioData
    {
        public string path;
        public EventInstance eventInstance;
    }
public class AudioService : SerializedMonoBehaviour, IAudioService
{
    #region Fields

    [SerializeField] private Dictionary<string, IList<TextAsset>> loadedAddressablesBankTexts =
        new Dictionary<string, IList<TextAsset>>();

    [SerializeField]
    private Dictionary<string, InstanceAudioData> soundEventInstances = new Dictionary<string, InstanceAudioData>();

    private const string SOUNDBANKS_SUFFIX = "_SoundBanks";
    private List<string> notRemoveKeys = new List<string>();

    #endregion

   

    #region Public Methods

    [Title("Functions")]

    //LoadBanks
    [Button]
    public async void LoadSoundBanks(string soundBankLabel)
    {
        var label = soundBankLabel + SOUNDBANKS_SUFFIX;
        await LoadAddressablesSoundBankFiles(label);
        await LoadFmodBanks(label);
    }

    [Button]
    public void UnloadSoundBanks(string banksLabel)
    {
        var label = banksLabel + SOUNDBANKS_SUFFIX;
        foreach (var textAsset in loadedAddressablesBankTexts[label])
        {
            RuntimeManager.UnloadBank(textAsset);
        }

        FindActiveSoundInstances();
        RemoveUnloadedSounds();

        loadedAddressablesBankTexts.Remove(label);
    }

    [Button]
    public void UnloadAllSoundBanks()
    {
        RuntimeManager.StudioSystem.unloadAll();
        loadedAddressablesBankTexts.Clear();
    }

    [Button]
    public void PlayOneShot(string soundEventPath)
    {
        RuntimeManager.PlayOneShot(soundEventPath);
    }

    [Button]
    public void PauseAllSounds()
    {
        RuntimeManager.PauseAllEvents(true);
    }

    [Button]
    public void ResumeAllSounds()
    {
        RuntimeManager.PauseAllEvents(false);
    }

    [Button]
    public async UniTask ReleaseAllSounds()
    {
        while (soundEventInstances.Count > 0)
        {
            ReleaseSound(soundEventInstances.First().Key);
            await UniTask.Yield();
        }

        soundEventInstances.Clear();
    }

    [Button]
    public void CreateSound(string soundKey, string eventPath)
    {
        if (soundEventInstances.ContainsKey(soundKey))
        {
            Logman.LogError("This tag exists. Enter a tag with a different name.");
            return;
        }

        var eventInstance = RuntimeManager.CreateInstance(eventPath);
        Logman.Log("GUID :" + RuntimeManager.PathToGUID(eventPath).ToString());
        soundEventInstances.Add(soundKey, new InstanceAudioData() { path = eventPath, eventInstance = eventInstance });
        ;
    }

    [Button]
    public void PlaySound(string soundKey)
    {
        soundEventInstances[soundKey].eventInstance.start();
    }

    [Button]
    public void PauseSound(string soundTag)
    {
        soundEventInstances[soundTag].eventInstance.setPaused(true);
    }

    [Button]
    public void ResumeSound(string soundKey)
    {
        soundEventInstances[soundKey].eventInstance.setPaused(false);
    }

    [Button]
    public void StopSound(string soundKey)
    {
        soundEventInstances[soundKey].eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    [Button]
    public void ReleaseSound(string soundKey)
    {
        soundEventInstances[soundKey].eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        soundEventInstances[soundKey].eventInstance.release();
        soundEventInstances[soundKey].eventInstance.clearHandle();
        soundEventInstances.Remove(soundKey);
    }

    public EventInstance GetSoundEventInstance(string soundTag)
    {
        return soundEventInstances[soundTag].eventInstance;
    }

    [Button]
    public void ChangeExistingSound(string soundKey, string newEventPath)
    {
        ReleaseSound(soundKey);
        CreateSound(soundKey, newEventPath);
    }

    #endregion

    #region Private Methods

    private async UniTask LoadAddressablesSoundBankFiles(string banksLabel)
    {
        try
        {
            if (loadedAddressablesBankTexts.ContainsKey(banksLabel))
            {
                Logman.Log(banksLabel + "already loaded.");
                return;
            }

            AsyncOperationHandle<IList<TextAsset>> addressablesSoundBankLoadOperation;

            addressablesSoundBankLoadOperation = Addressables.LoadAssetsAsync<TextAsset>(banksLabel,
                (bank) => Logman.Log(bank.name + " Bank Loaded from Addressables"),
                true);

            await addressablesSoundBankLoadOperation.Task;

            if (addressablesSoundBankLoadOperation.Status == AsyncOperationStatus.Succeeded)
            {
                loadedAddressablesBankTexts.Add(banksLabel, addressablesSoundBankLoadOperation.Result);
            }
        }
        catch (Exception e)
        {
            Logman.LogError(e);
        }
    }


    private async UniTask LoadFmodBanks(string banksLabel)
    {
        try
        {
            foreach (var textAsset in loadedAddressablesBankTexts[banksLabel])
            {
                if (!RuntimeManager.HasBankLoaded(textAsset.name))
                {
                    RuntimeManager.LoadBank(textAsset, true);
                }
            }

            while (!RuntimeManager.HaveAllBanksLoaded)
            {
                await UniTask.Yield();
            }

            RuntimeManager.WaitForAllSampleLoading();
        }
        catch (Exception e)
        {
            Logman.LogError(e);
        }
    }

    // = new List<string>();
    [Button]
    public void CheckAllEvents2()
    {
        //notRemoveKeys = new List<string>();
        //// RuntimeManager.StudioSystem.getBank()
        //RuntimeManager.StudioSystem.getBankList(out banks);
        //foreach (var bank in banks)
        //{
        //    EventDescription[] eventList;
        //    bank.getEventList(out eventList);
        //    foreach (var @event in eventList)
        //    {
        //        string eventPath2;
        //        @event.getPath(out eventPath2);

        //        foreach (var sound in soundEventInstances)
        //        {
        //            if (sound.Value.path == eventPath2)
        //            {
        //                notRemoveKeys.Add(sound.Key);
        //            }
        //        }

        //    }
        //}

        RemoveUnloadedSounds();
    }


    private void FindActiveSoundInstances()
    {
        notRemoveKeys.Clear();

        //Get Banks
        RuntimeManager.StudioSystem.getBankList(out var banks);

        foreach (var bank in banks)
        {
            //Get Bank EventList
            EventDescription[] eventList;
            bank.getEventList(out eventList);

            foreach (var @event in eventList)
            {
                string eventPath;
                @event.getPath(out eventPath);

                foreach (var sound in soundEventInstances.Where(sound => sound.Value.path == eventPath))
                {
                    notRemoveKeys.Add(sound.Key);
                }
            }
        }
    }


    private void RemoveUnloadedSounds()
    {
        var toRemoveKeys = (from sound in soundEventInstances where !notRemoveKeys.Contains(sound.Key) select sound.Key).ToList();

        foreach (var key in toRemoveKeys)
        {
            ReleaseSound(key);
            soundEventInstances.Remove(key);
        }
    }

    #endregion
}
    
}
