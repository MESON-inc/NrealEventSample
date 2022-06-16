using UnityEngine;

namespace NrealEventSample.Demo
{
    public class ParticleSystemController : MonoBehaviour
    {
        [SerializeField] private TextParticleSystem _particleSystem;
        [SerializeField] private MessageMaker _messageMaker;
        [SerializeField, Multiline] private string _message = "This is a sample message.";

        private void Start()
        {
            Apply();
        }

        private void Apply()
        {
            Texture2D texture = _messageMaker.Make(_message);
            _particleSystem.SetTextTexture(texture);
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
                    controller.Apply();
                }
            }
        }
#endif
    }
}