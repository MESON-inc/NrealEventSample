using UnityEngine;

namespace NrealEventSample
{
    public class FollowUI : MonoBehaviour
    {
        [SerializeField] private float _distance = 1.5f;
        [SerializeField] private float _speed = 2f;
        [SerializeField] private bool _fixOnHorizontal = false;
        [SerializeField] private Vector3 _offset = new Vector3(0, -0.1f, 0);

        private Transform _cameraTransform;

        #region ### ------------------------------ MonoBehaviour ------------------------------ ###

        private void Start()
        {
            _cameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            float t = Time.deltaTime * _speed;

            Vector3 forward = GetForward();
            Vector3 target = (forward * _distance) + _offset;

            transform.forward = Vector3.Lerp(transform.forward, forward, t);
            transform.position = Vector3.Lerp(transform.position, target, t);
        }

        #endregion ### ------------------------------ MonoBehaviour ------------------------------ ###

        #region ### ------------------------------ Private methods ------------------------------ ###

        private Vector3 GetForward()
        {
            Vector3 forward = _cameraTransform.forward;

            if (!_fixOnHorizontal)
            {
                return forward;
            }

            return Vector3.ProjectOnPlane(forward, Vector3.up).normalized;
        }

        #endregion ### ------------------------------ Private methods ------------------------------ ###
    }
}