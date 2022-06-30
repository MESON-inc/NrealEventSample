using UnityEngine;

namespace WorkRoom.Effects
{
    public class Vignette : MonoBehaviour
    {
        [SerializeField] private Material _material = null;

        #region ### ------------------------------ MonoBehaviour ------------------------------ ###

        private void Awake()
        {
            _material = Instantiate(_material);
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Graphics.Blit(source, destination, _material);
        }

        #endregion ### ------------------------------ MonoBehaviour ------------------------------ ###
    }
}