using Cysharp.Threading.Tasks;
using FMOD.Studio;

namespace _TDK.Common
{
    public interface IAudioService
    {
        void LoadSoundBanks(string soundBankLabel);
        void UnloadSoundBanks(string soundBankLabel);
        void UnloadAllSoundBanks();

        void PlayOneShot(string eventPath);

        void PauseAllSounds();
        void ResumeAllSounds();

        void CreateSound(string soundKey, string eventPath);
        void PlaySound(string soundKey);
        void PauseSound(string soundKey);
        void ResumeSound(string soundKey);
        void StopSound(string soundKey);
        void ReleaseSound(string soundKey);
        UniTask ReleaseAllSounds();

        EventInstance GetSoundEventInstance(string soundKey);
        void ChangeExistingSound(string soundKey, string newEventPath);

    }
}
