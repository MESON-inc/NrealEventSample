using UnityEngine;

namespace NrealEventSample
{
    public class ButtonParticleSystem : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private Texture2D _buttonTexture;

        private void Start()
        {
            Vector2 areaSize = _particleSystem.AreaSize;
            areaSize.y = areaSize.x * (_buttonTexture.height / (float)_buttonTexture.width);
            _particleSystem.AreaSize = areaSize;
            _particleSystem.SetTexture(_buttonTexture);
            _particleSystem.Play();
        }
    }
}