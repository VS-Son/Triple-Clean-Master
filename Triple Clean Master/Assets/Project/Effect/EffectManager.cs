using System;
using System.Collections;
using System.Collections.Generic;
using Project.Manager;
using UnityEngine;

namespace Project.Effect
{
    public static class EffectManager
    {
        public static bool IsLive;
        public static IEnumerator PlayEffectAndWait(List<ParticleSystem> effects, int index, Action onComplete = null)
        {
            if (effects == null) yield break;;

            if (index < 0 || index >= effects.Count)
            {
                Debug.LogWarning($"Effect index invalid: {index}");
                yield break;
            }

            ParticleSystem effect = effects[index];

            if (effect == null)
            {
                Debug.LogWarning($"Particle missing at index: {index}");
                yield break;
            }

            effect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            effect.Play();
            yield return new WaitWhile(() => effect.IsAlive(true));
            onComplete?.Invoke();
            IsLive = effect.IsAlive(true);
        }

       
    }
}
