using UnityEngine;

namespace NrealEventSample.Demo
{
    public class ParticleSystemController : MonoBehaviour
    {
        [SerializeField] private TextParticleSystem _particleSystem;
        [SerializeField] private MessageMaker _messageMaker;

        private void Start()
        {
            Texture2D texture = _messageMaker.Make("hogehoge");
            _particleSystem.SetTextTexture(texture);
        }
    }
}