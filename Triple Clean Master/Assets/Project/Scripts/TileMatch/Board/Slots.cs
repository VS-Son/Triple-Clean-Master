using UnityEngine;

namespace Project.Scripts.TileMatch.Board
{
    public class Slots : MonoBehaviour
    {
        [SerializeField] private ParticleSystem effectStar;
        public void PlayEffect()
        {
            if (effectStar != null)
            {
                effectStar.Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear);
                effectStar.Play();
            }        }
    }
}
