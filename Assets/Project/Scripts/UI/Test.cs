using System;
using UnityEngine;

namespace NrealEventSample
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private float _distance = 1.2f;
        [SerializeField] private float _angle = 25f;
        [SerializeField] private float _speed = 1f;

        private Camera _camera;
        private Transform _cameraTransform;

        private void Start()
        {
            _camera = Camera.main;
            _cameraTransform = _camera.transform;
        }

        private void Update()
        {
            if (!InView()) return;

            float t = Time.deltaTime * _speed;

            Vector3 forward = _cameraTransform.forward;
            float tan = Mathf.Tan(_angle * Mathf.Deg2Rad) * _distance;
            Vector3 right = _cameraTransform.right * tan;
            Vector3 up = _cameraTransform.up * tan;

            Vector3 lv = (forward * _distance) - right;
            Vector3 rv = (forward * _distance) + right;
            Vector3 tv = (forward * _distance) + up;
            Vector3 bv = (forward * _distance) - up;

            Vector3 tl = _cameraTransform.position + lv;
            Vector3 tr = _cameraTransform.position + rv;
            Vector3 tt = _cameraTransform.position + tv;
            Vector3 tb = _cameraTransform.position + bv;

            Vector3[] ps = { tl, tr, tt, tb };
            
            float dist = float.MaxValue;
            int index = -1;
            for (int i = 0; i < ps.Length; i++)
            {
                float d = (ps[i] - transform.position).sqrMagnitude;
                if (d < dist)
                {
                    dist = d;
                    index = i;
                }
            }

            transform.position = Vector3.Lerp(transform.position, ps[index], t);

            Vector3 dir = (transform.position - _cameraTransform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, dir, t);
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;
            
            Vector3 forward = _cameraTransform.forward;
            float tan = Mathf.Tan(_angle * Mathf.Deg2Rad) * _distance;
            Vector3 right = _cameraTransform.right * tan;
            Vector3 up = _cameraTransform.up * tan;
            
            Gizmos.color = Color.blue;
            Vector3 f = _cameraTransform.position + forward * _distance;
            Gizmos.DrawWireSphere(f, 0.05f);

            Vector3 r = f + right;
            Gizmos.DrawWireSphere(r, 0.05f);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(_cameraTransform.position, _cameraTransform.position + r);
            
            Vector3 lv = (forward * _distance) - right;
            Vector3 rv = (forward * _distance) + right;
            Vector3 tv = (forward * _distance) + up;
            Vector3 bv = (forward * _distance) - up;

            Vector3 tl = _cameraTransform.position + lv;// - transform.position;
            Vector3 tr = _cameraTransform.position + rv;// - transform.position;
            Vector3 tt = _cameraTransform.position + tv;// - transform.position;
            Vector3 tb = _cameraTransform.position + bv;// - transform.position;

            Gizmos.DrawWireSphere(tl, 0.05f);
            Gizmos.DrawWireSphere(tr, 0.05f);
            Gizmos.DrawWireSphere(tt, 0.05f);
            Gizmos.DrawWireSphere(tb, 0.05f);
        }

        private bool InView()
        {
            Vector3 delta = (transform.position - _cameraTransform.position).normalized;
            float d = Vector3.Dot(delta, _cameraTransform.forward);

            float a = Mathf.Acos(d) * Mathf.Rad2Deg;

            Debug.DrawLine(_cameraTransform.position, _cameraTransform.position + delta, Color.cyan, Time.deltaTime);
            
            return a > _angle;
        }
    }
}