using System.Collections;
using UnityEngine;

namespace NrealEventSample.Demo
{
    public class ParticleSystemController : MonoBehaviour
    {
        #region ### ------------------------------ Serialize Fields ------------------------------ ###

        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private MessageMaker _messageMaker;

        [SerializeField] private string[] _messages = new string[]
        {
            "MESON is an XR Creative Company. We build and deliver services for augmenting human experiences.",
            "Stone, electricity, computer. Humanity augments possibilities and choices via inventing and combining these technologies. MESON augments human experiences through driving the dynamics of XR, Metaverse, and Web3 technologies.",
        };

        #endregion ### ------------------------------ Serialize Fields ------------------------------ ###

        #region ### ------------------------------ Members ------------------------------ ###

        private int _index = 0;

        #endregion ### ------------------------------ Members ------------------------------ ###

        #region ### ------------------------------ MonoBehaviour ------------------------------ ###

        private IEnumerator Start()
        {
            Application.targetFrameRate = 60;

            yield return new WaitForSeconds(1f);

            StartParticle();

            yield return new WaitForSeconds(0.1f);

            ApplyNext();
        }

        #endregion ### ------------------------------ MonoBehaviour ------------------------------ ###

        #region ### ------------------------------ Public methods ------------------------------ ###

        /// <summary>
        /// Start a particle system.
        /// </summary>
        public void StartParticle()
        {
            _particleSystem.Play();
        }

        /// <summary>
        /// Apply a message texture to transition to next.
        /// </summary>
        public void ApplyNext()
        {
            Texture2D texture = _messageMaker.Make(_messages[_index]);
            _particleSystem.SetTexture(texture);

            _index = (_index + 1) % _messages.Length;
        }

        #endregion ### ------------------------------ Public methods ------------------------------ ###

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