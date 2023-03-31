Shader "Unlit/Cutoff"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SecondaryTex("Texture", 2D) = "white" {}
        _Cutoff ("Cutoff", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _SecondaryTex;
            float4 _SecondaryTex_ST;
            float _Cutoff;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float4 when_gt(float4 a, float4 b)
            {
                return max(sign(a - b), 0.0);
            }

            float when_lte(float4 a, float b)
            {
                return 1.0 - when_gt(a, b);
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_SecondaryTex, i.uv);
                col.a *= when_gt(col.b, _Cutoff);
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                return fixed4(0, 0, 0, col.a);
            }
            ENDCG
        }
    }
}
