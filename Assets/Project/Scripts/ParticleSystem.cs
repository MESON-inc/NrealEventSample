using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NrealEventSample.Utility;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NrealEventSample
{
    [Flags]
    public enum ActivateType : uint
    {
        targetPosition = 1 << 0,
        color = 1 << 1,
        scale = 1 << 2,
        velocity = 1 << 3,
    }

    public class ParticleSystem : MonoBehaviour
    {
        [SerializeField] private ComputeShader _shader;
        [SerializeField] private Material _material;
        [SerializeField] private float _stickDistance = 0.2f;
        [SerializeField] private float _stickForce = 200f;
        [SerializeField] private float _attentionForce = 200f;
        [SerializeField] private float _attentionSpeed = 5f;
        [SerializeField] private float _suppress = 4.0f;
        [SerializeField] private float _threshold = 0.1f;
        [SerializeField] private Transform _targetTransform;
        [SerializeField] private Vector2 _areaSize = new Vector2(1.2f, 0.6f);
        [SerializeField] private float _magnification = 5.0f;
        [SerializeField] private float _distribution = 1f;
        [SerializeField] private float _fixedDeltaTime = 0.016f;

        [Header("==== Setting for particle ====")] [SerializeField]
        private int _particleCount = 120_000;

        [SerializeField] private Mesh _particleMesh;
        [SerializeField] private Color _particleColor = new Color(0.123f, 0.873f, 0.9333f, 1f);
        [SerializeField] private float _particleScale = 0.01f;

        private int _initializeKernelIndex = -1;
        private int _updateKernelIndex = -1;

        private GraphicsBuffer _particleBuffer;
        private GraphicsBuffer _argsBuffer;
        private GraphicsBuffer _particleDataBuffer;
        private List<Vector4> _textPositions;
        private ParticleData[] _particleData;

        private bool _running = false;

        #region ### ------------------------------ MonoBehaviour ------------------------------ ###

        private void Awake()
        {
            _initializeKernelIndex = _shader.FindKernel("Initialize");
            _updateKernelIndex = _shader.FindKernel("Update");
            InitializeBuffers();
            InitializeParticle();
        }

        private void Update()
        {
            if (!_running) return;
            
            UpdateParticles();
            DrawParticles();
        }

        private void OnDestroy()
        {
            _particleBuffer?.Release();
            _argsBuffer?.Dispose();
            _particleDataBuffer?.Release();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Matrix4x4 tmp = Gizmos.matrix;

            Gizmos.matrix = _targetTransform.localToWorldMatrix;
            
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(_areaSize.x, 0, _areaSize.y));

            Gizmos.matrix = tmp;
        }

        #endregion ### ------------------------------ MonoBehaviour ------------------------------ ###

        private void InitializeBuffers()
        {
            _particleBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, _particleCount, Marshal.SizeOf<Particle>());
            _particleDataBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, _particleCount, Marshal.SizeOf<ParticleData>());

            _particleData = new ParticleData[_particleCount];
            _textPositions = new List<Vector4>(_particleCount);

            uint[] args = new uint[]
            {
                _particleMesh.GetIndexCount(0),
                (uint)_particleCount,
                _particleMesh.GetIndexStart(0),
                _particleMesh.GetBaseVertex(0),
                0,
            };

            _argsBuffer = new GraphicsBuffer(GraphicsBuffer.Target.IndirectArguments, 1, sizeof(uint) * args.Length);
            _argsBuffer.SetData(args);
        }

        private void InitializeParticle()
        {
            Particle[] particles = new Particle[_particleCount];

            for (int i = 0; i < particles.Length; ++i)
            {
                Vector3 pos = _targetTransform.position + Random.insideUnitSphere * _distribution;

                Vector4 color = new Vector4(_particleColor.r, _particleColor.g, _particleColor.b, _particleColor.a);

                Particle p = new Particle
                {
                    active = 1,
                    scale = _particleScale,
                    position = pos,
                    targetPosition = pos,
                    color = color,
                };

                particles[i] = p;
            }

            _particleBuffer.SetData(particles);

            _shader.SetBuffer(_updateKernelIndex, "_ParticleBuffer", _particleBuffer);
            _shader.SetBuffer(_initializeKernelIndex, "_ParticleBuffer", _particleBuffer);

            _material.SetBuffer("_ParticleBuffer", _particleBuffer);
        }

        private void UpdateParticles()
        {
            _shader.SetFloat("_Time", Time.time);
            _shader.SetFloat("_DeltaTime", _fixedDeltaTime);
            _shader.SetFloat("_StickDistance", _stickDistance);
            _shader.SetFloat("_StickForce", _stickForce);
            _shader.SetFloat("_AttentionForce", _attentionForce);
            _shader.SetFloat("_AttentionSpeed", _attentionSpeed);
            _shader.SetFloat("_Suppress", _suppress);
            _shader.SetMatrix("_TargetMatrix", _targetTransform.localToWorldMatrix);
            _shader.Dispatch(_updateKernelIndex, _particleCount / 8, 1, 1);
        }

        private void UpdateParticleData(int? targetCount = null)
        {
            int particleCount = targetCount ?? _particleCount;
            
            _particleDataBuffer.SetData(_particleData);

            _shader.SetInt("_ParticleCount", particleCount);

            _shader.SetBuffer(_initializeKernelIndex, "_ParticleDataBuffer", _particleDataBuffer);

            _shader.Dispatch(_initializeKernelIndex, _particleCount / 8, 1, 1);
        }

        private void DrawParticles()
        {
            Graphics.DrawMeshInstancedIndirect(
                _particleMesh,
                0,
                _material,
                new Bounds(_targetTransform.position, Vector3.one * 32f),
                _argsBuffer,
                0,
                null,
                UnityEngine.Rendering.ShadowCastingMode.Off,
                false,
                gameObject.layer);
        }

        public void Play()
        {
            _running = true;
        }

        public void Stop()
        {
            _running = false;
        }

        public void SetTexture(Texture2D texture)
        {
            _textPositions.Clear();

            TextBufferMaker.Make(texture, 0, ref _textPositions, _threshold);
            
            for (int i = 0; i < _textPositions.Count; i++)
            {
                Vector4 p = _textPositions[i];
                p.x *= _areaSize.x;
                p.z *= _areaSize.y;

                ParticleData px = new ParticleData
                {
                    activateTypes = (uint)(ActivateType.targetPosition | ActivateType.color | ActivateType.velocity | ActivateType.scale),
                    scale = _particleScale,
                    targetPosition = p,
                    velocity = Random.insideUnitSphere * _magnification,
                    color = _particleColor,
                };

                _particleData[i] = px;
            }

            if (_textPositions.Count > _particleCount)
            {
                Debug.LogWarning(
                    $"Particle count is not enough to show the textures . Particle count is {_particleCount.ToString()}. Total texture pixel count is {_textPositions.Count.ToString()}");
                return;
            }

            UpdateParticleData(_textPositions.Count);
        }
    }
}