using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Project.Scripts.Manager;

namespace Project.Scripts.Effect
{
    public enum EffectTypes
    {
        Partical,
        Rotate,
        Parabola
    }
    public  class EffectManager : Singleton<EffectManager>
    {
        public static bool IsLive;
        public EffectTypes effectTypes;
        public int direction;
        public float speed;
        
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
        

        public void Update()
        {
            if (effectTypes == EffectTypes.Rotate)
            {
                transform.Rotate(0f,0f,direction * speed* Time.deltaTime);
            }
        }
    }
}
