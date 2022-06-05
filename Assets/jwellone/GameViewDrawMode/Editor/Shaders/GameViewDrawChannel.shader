Shader "jwellone/Editor/GameView/DrawChannel"
{
    Properties 
    {
        _MainTex ("Source", 2D) = "white" {}
    }

    SubShader
    {
        Tags{ "RenderType" = "Opaque" }
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma multi_compile _CHANNEL_R _CHANNEL_G _CHANNEL_B _CHANNEL_A

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

            sampler2D _MainTex;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 color = tex2D( _MainTex, i.uv );
                
#if   _CHANNEL_R
                color.g = color.b = 0;
#elif _CHANNEL_G
                color.r = color.b = 0;
#elif _CHANNEL_B
                color.r = color.g = 0;
#else
                color.r = color.g = color.b = color.a;
#endif

                return color;
            }

            ENDCG
        }
    }
}
