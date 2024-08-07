using System;
using System.Threading;
using _TDK.Common;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UT.UT_ImmersiveBreathing.Scripts
{
    public class ImmersiveBreathing : MonoBehaviour, IUniversalTechnic
    {
        #region Fields

        [SerializeField] private GameObject breathInParticlesParent;
        [SerializeField] private GameObject breathOutParticlesParent;
        [SerializeField] private ParticleSystem[] otherParticles;

        [Header("Durations")] [SerializeField] private float breathInDuration = 4;
        [SerializeField] private float holdDuration = 1;
        [SerializeField] private float breathOutDuration = 5;

        [SerializeField] private float fadeOutDuration = 2;

        [Space] [SerializeField] public int loopCount = 5;

        [Header("Settings")] [SerializeField] private bool isInfiniteLoop = false;
        public bool destroyOnEnd = true;

        [Header("Object Recenter Settings")] [SerializeField]
        private float heightAboveHead = 0;

        [SerializeField] private float distance = 3;
        [SerializeField] private bool setTransformInFrontOfThePlayer = true;

        [HideInInspector] public bool isDestroyed = false;

        private CancellationTokenSource _cts;
        private Camera _mainCam;

        #endregion


        #region Unity Events

        private void Start()
        {
            _mainCam = Camera.main;
            _cts = new CancellationTokenSource();
        }

        private void OnDestroy()
        {
            _cts.Cancel();
            isDestroyed = true;
        }

        private void OnDisable()
        {
            isDestroyed = true;
            _cts.Cancel();
            _cts.Dispose();
        }

        #endregion


        #region Public Methods

        [Button()]
        public async UniTask StartImmersiveBreathing()
        {
            SetParticleDurations(breathInParticlesParent, breathInDuration);
            SetParticleDurations(breathOutParticlesParent, breathOutDuration);
            if (setTransformInFrontOfThePlayer)
            {
                SetImmersiveBreathingTransform();
            }
            Debug.Log("Immersive Breathing Started");
            foreach (var particle in otherParticles)
            {
                particle.Play();
            }

            if (!isInfiniteLoop)
            {
                for (int i = 0; i < loopCount; i++)
                {
                    StartParticles(breathInParticlesParent);
                    await UniTask.Delay(TimeSpan.FromSeconds(breathInDuration + holdDuration),
                        cancellationToken: _cts.Token);
                    StartParticles(breathOutParticlesParent);
                    await UniTask.Delay(TimeSpan.FromSeconds(breathOutDuration), cancellationToken: _cts.Token);
                }

                FadeOutAndStopParticles();

                if (destroyOnEnd)
                {
                    Destroy(gameObject, fadeOutDuration);
                }
            }
            else if (isInfiniteLoop)
            {
                while (!isDestroyed)
                {
                    StartParticles(breathInParticlesParent);
                    await UniTask.Delay(TimeSpan.FromSeconds(breathInDuration + holdDuration),
                        cancellationToken: _cts.Token);
                    StartParticles(breathOutParticlesParent);
                    await UniTask.Delay(TimeSpan.FromSeconds(breathOutDuration), cancellationToken: _cts.Token);
                }
            }
        }

        [Button()]
        public void StopImmediately()
        {
            isDestroyed = true;
        }

        #region Builder

        public ImmersiveBreathing SetLoopCount(int loopCount)
        {
            this.loopCount = loopCount;
            return this;
        }

        public ImmersiveBreathing SetBreathInDuration(int breathInDuration)
        {
            this.breathInDuration = breathInDuration;
            return this;
        }

        public ImmersiveBreathing SetBreathOutDuration(int breathOutDuration)
        {
            this.breathOutDuration = breathOutDuration;
            return this;
        }

        public ImmersiveBreathing SetHoldDuration(int holdDuration)
        {
            this.holdDuration = holdDuration;
            return this;
        }

        public ImmersiveBreathing SetFadeOutDuration(int fadeOutDuration)
        {
            this.fadeOutDuration = fadeOutDuration;
            return this;
        }

        public ImmersiveBreathing SetIsInfiniteLoop(bool isInfiniteLoop)
        {
            this.isInfiniteLoop = isInfiniteLoop;
            return this;
        }

        public ImmersiveBreathing SetDistanceAboveHead(float distanceAboveHead)
        {
            this.heightAboveHead = distanceAboveHead;
            return this;
        }

        public ImmersiveBreathing SetDistance(float distance)
        {
            this.distance = distance;
            return this;
        }

        public ImmersiveBreathing SetTransformInFrontOfThePlayer(bool setTransformInFrontOfThePlayer)
        {
            this.setTransformInFrontOfThePlayer = setTransformInFrontOfThePlayer;
            return this;
        }

        #endregion

        #endregion


        #region Private Methods

        private void SetParticleDurations(GameObject particleParent, float duration)
        {
            ParticleSystem ps;
            for (int i = 0; i < particleParent.transform.childCount; i++)
            {
                ps = particleParent.transform.GetChild(i).GetComponent<ParticleSystem>();
                var main = ps.main;
                main.startLifetime = duration;
            }
        }

        private void StartParticles(GameObject particleParent)
        {
            for (int i = 0; i < particleParent.transform.childCount; i++)
            {
                particleParent.transform.GetChild(i).GetComponent<ParticleSystem>().Play();
            }
        }

        private async UniTask FadeOutAndStopParticles()
        {
            foreach (var particle in otherParticles)
            {
                particle.gameObject.GetComponent<Renderer>().material.DOFloat(0, "_Alpha", fadeOutDuration);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(fadeOutDuration), cancellationToken: _cts.Token);

            foreach (var particle in otherParticles)
            {
                particle.gameObject.GetComponent<ParticleSystem>().Stop();
            }
        }

        private void SetImmersiveBreathingTransform()
        {
            transform.position = _mainCam.transform.position + (_mainCam.transform.forward * distance);
            transform.position = new Vector3(transform.position.x, _mainCam.transform.position.y + heightAboveHead,
                transform.position.z);
            transform.rotation = Quaternion.Euler(0, _mainCam.transform.rotation.eulerAngles.y, 0);
        }

        #endregion
    }
}