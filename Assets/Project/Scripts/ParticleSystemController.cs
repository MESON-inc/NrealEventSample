using NrealEventSample.Utility;
using UnityEngine;

namespace NrealEventSample.Demo
{
    public class ParticleSystemController : MonoBehaviour
    {
        [SerializeField] private TextParticleSystem _particleSystem;
        [SerializeField] private TextTextureRepository _textTextureRepository;

        private void Start()
        {
            _particleSystem.SetTextTexture(_textTextureRepository.GetTextureAt(0));
        }
    }
}