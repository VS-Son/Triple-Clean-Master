using UnityEngine;

namespace Project.Games.TileMatch.Board.Scripts
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
