using UnityEngine;

namespace NrealEventSample
{
    public class FollowUI : MonoBehaviour
    {
        [SerializeField] private float _distance = 1.5f;
        [SerializeField] private float _speed = 2f;
        
        private Transform _cameraTransform;

        private void Start()
        {
            _cameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            float t = Time.deltaTime * _speed;
            
            Vector3 forward = _cameraTransform.forward;
            transform.forward = Vector3.Lerp(transform.forward, forward, t);
            
            Vector3 target = forward * _distance;
            transform.position = Vector3.Lerp(transform.position, target, t);
        }
    }
}