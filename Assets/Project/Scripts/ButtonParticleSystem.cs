using UnityEngine;
using UnityEngine.EventSystems;

namespace NrealEventSample
{
    public class ButtonParticleSystem : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private Texture2D _buttonTexture;
        [SerializeField] private EventTrigger _eventTrigger;

        private void Start()
        {
            SetupEvent();

            PlayParticle();
        }

        private void SetupEvent()
        {
            EventTrigger.Entry clickEntry = new EventTrigger.Entry();
            clickEntry.eventID = EventTriggerType.PointerClick;
            clickEntry.callback.AddListener(_ => { Debug.Log("click"); });
            _eventTrigger.triggers.Add(clickEntry);
            
            EventTrigger.Entry enterEntry = new EventTrigger.Entry();
            clickEntry.eventID = EventTriggerType.PointerEnter;
            clickEntry.callback.AddListener(_ => { Debug.Log("enter"); });
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