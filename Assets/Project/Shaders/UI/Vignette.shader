Shader "NrealEventSample/Vignette"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Width("Width", Float) = 0.015
    }
    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _Direction;
            float _Width;
            int _EyeType;

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float2 uv = i.uv;

                float2 uv1 = min(1.0, uv / _Width);
                float2 uv2 = min(1.0, -(uv - 1.0) / _Width);
                float s = uv1.x * uv1.y * uv2.x * uv2.y;
                col *= s;

                return col;
            }
            ENDCG
        }
    }
}