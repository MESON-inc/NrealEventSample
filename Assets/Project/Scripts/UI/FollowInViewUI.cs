using System.Collections;
using UnityEngine;

namespace NrealEventSample
{
    public class FollowInViewUI : MonoBehaviour
    {
        [SerializeField] private float _distance = 1.2f;
        [SerializeField] private float _angle = 25f;
        [SerializeField] private float _speed = 1f;

        private Camera _camera;
        private Transform _cameraTransform;
        private Coroutine _coroutine;
        private bool _lastInView = false;

        #region ### ------------------------------ MonoBehaviour ------------------------------ ###

        private void Start()
        {
            _camera = Camera.main;
            _cameraTransform = _camera.transform;

            _lastInView = InView();
        }

        private void Update()
        {
            bool inView = InView();

            if (!inView && _lastInView)
            {
                StartMove();
            }

            _lastInView = inView;
        }

        #endregion ### ------------------------------ MonoBehaviour ------------------------------ ###

        private bool InView()
        {
            Vector3 delta = (transform.position - _cameraTransform.position).normalized;
            float d = Vector3.Dot(delta, _cameraTransform.forward);

            float a = Mathf.Acos(d) * Mathf.Rad2Deg;

            return a < _angle;
        }

        private void StartMove()
        {
            StopCoroutineIfNeeded();
            
            Vector3 forward = _cameraTransform.forward;
            Vector3 target = forward * _distance;
            
            _coroutine = StartCoroutine(MoveToView(target));
        }

        private IEnumerator MoveToView(Vector3 point)
        {
            while (true)
            {
                float t = Time.deltaTime * _speed;

                transform.position = Vector3.Lerp(transform.position, point, t);

                yield return null;
            }
        }

        private void StopCoroutineIfNeeded()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            _coroutine = null;
        }
    }
}