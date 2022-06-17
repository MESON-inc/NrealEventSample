using NrealEventSample.Demo;
using NRKernal;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NrealEventSample
{
    public class ButtonParticleSystem : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private Texture2D _buttonTexture;
        [SerializeField] private EventTrigger _eventTrigger;
        [SerializeField] private float _hoverMagnification = 0.5f;
        [SerializeField] private float _clickMagnification = 0.6f;
        [SerializeField] private ParticleSystemController _particleSystemController;

        private void Start()
        {
            SetupEvent();

            PlayParticle();
        }

        private void SetupEvent()
        {
            EventTrigger.Entry clickEntry = new EventTrigger.Entry();
            clickEntry.eventID = EventTriggerType.PointerClick;
            clickEntry.callback.AddListener(_ =>
            {
                _particleSystemController.ApplyNext();
                _particleSystem.ApplyRandomVelocity(_hoverMagnification * 1.2f);
                NRInput.TriggerHapticVibration(0.2f, 200f, 0.8f);
            });
            _eventTrigger.triggers.Add(clickEntry);

            EventTrigger.Entry enterEntry = new EventTrigger.Entry();
            enterEntry.eventID = EventTriggerType.PointerEnter;
            enterEntry.callback.AddListener(_ =>
            {
                _particleSystem.ApplyRandomVelocity(_hoverMagnification);
                NRInput.TriggerHapticVibration(0.1f, 200f, 0.8f);
            });
            _eventTrigger.triggers.Add(enterEntry);
        }

        private void PlayParticle()
        {
            Vector2 areaSize = _particleSystem.AreaSize;
            areaSize.y = areaSize.x * (_buttonTexture.height / (float)_buttonTexture.width);
            _particleSystem.AreaSize = areaSize;
            _particleSystem.SetTexture(_buttonTexture);
            _particleSystem.Play();
        }
    }
}