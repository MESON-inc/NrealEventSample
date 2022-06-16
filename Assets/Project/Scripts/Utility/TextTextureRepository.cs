using System;
using UnityEngine;

namespace NrealEventSample.Utility
{
    public class TextTextureRepository : MonoBehaviour
    {
        [SerializeField] private Texture2D[] _textures;

        public int Count => _textures.Length;

        public Texture2D GetTextureAt(int index)
        {
            if (index >= Count)
            {
                throw new IndexOutOfRangeException("Index is out of range of the textures.");
            }
            
            return _textures[index];
        }
    }
}