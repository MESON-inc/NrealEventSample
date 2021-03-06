using TMPro;
using UnityEngine;

namespace NrealEventSample
{
    public class MessageMaker : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private TMP_Text _text;

        /// <summary>
        /// Make a message texture by message strings.
        /// </summary>
        /// <param name="message">A message to show.</param>
        /// <returns>A message backed texture.</returns>
        public Texture2D Make(string message)
        {
            _text.text = message;
            
            _camera.Render();

            RenderTexture backup = RenderTexture.active;

            Texture2D result = new Texture2D(_camera.targetTexture.width, _camera.targetTexture.height);
            RenderTexture.active = _camera.targetTexture;
            result.ReadPixels(new Rect(0, 0, result.width, result.height), 0, 0);
            RenderTexture.active = backup;

            return result;
        }
    }
}