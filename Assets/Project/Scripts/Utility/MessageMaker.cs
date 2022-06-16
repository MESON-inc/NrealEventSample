using UnityEngine;

namespace NrealEventSample
{
    public class MessageMaker : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        public Texture2D Make(string message)
        {
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