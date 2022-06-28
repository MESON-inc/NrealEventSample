Shader "NrealEventSample/Invisible"
{
    Properties
    {
        _ColorMask ("Color Mask", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            ColorMask [_ColorMask]
            ZWrite Off
        }
    }
}
