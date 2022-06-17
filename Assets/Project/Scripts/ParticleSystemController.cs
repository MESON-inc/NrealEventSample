using System.Collections;
using UnityEngine;

namespace NrealEventSample.Demo
{
    public class ParticleSystemController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private MessageMaker _messageMaker;

        [SerializeField] private string[] _messages = new string[]
        {
            "MESON is an XR Creative Company. We build and deliver services for augmenting human experiences.",
            "Stone, electricity, computer. Humanity augments possibilities and choices via inventing and combining these technologies. MESON augments human experiences through driving the dynamics of XR, Metaverse, and Web3 technologies.",
        };

        [SerializeField] private bool _autoPlay = true;

        private int _index = 0;

        private IEnumerator Start()
        {
            Application.targetFrameRate = 60;

            yield return new WaitForSeconds(1f);

            StartParticle();
        }

        public void StartParticle()
        {
            _particleSystem.Play();

            if (_autoPlay)
            {
                StartCoroutine(PlayLoop());
            }
        }

        private IEnumerator PlayLoop()
        {
            while (true)
            {
                ApplyNext();

                yield return new WaitForSeconds(5f);
            }
        }

        public void ApplyNext()
        {
            Texture2D texture = _messageMaker.Make(_messages[_index]);
            _particleSystem.SetTexture(texture);

            _index = (_index + 1) % _messages.Length;
        }

#if UNITY_EDITOR
        [UnityEditor.CustomEditor(typeof(ParticleSystemController))]
        private class MessageMakerEditor : UnityEditor.Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                if (!Application.isPlaying) return;

                if (!(target is ParticleSystemController controller)) return;

                if (GUILayout.Button("Apply"))
                {
                    controller.ApplyNext();
                }
            }
        }
#endif
    }
}