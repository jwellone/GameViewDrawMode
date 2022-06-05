Shader "jwellone/Editor/GameView/DrawMeshInfo"
{
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma multi_compile _MESHINFO_UV0 _MESHINFO_UV1 _MESHINFO_VERTEXCOLOR _MESHINFO_NORMALS _MESHINFO_TANGENTS _MESHINFO_BITANGENTS
            
            struct appdata
            {
                float4 vertex       : POSITION;
                float2 texcoord0    : TEXCOORD0;
                float2 texcoord1    : TEXCOORD1;
                float4 color        : COLOR;
                float4 normal       : NORMAL;
                float4 tangent      : TANGENT;
            };
         
            struct v2f 
            {
                float4 pos : SV_POSITION;
                fixed4 color : COLOR;
                float4 uv : TEXCOORD0;
            };
            
            v2f vert (appdata v)
            {
                v2f o;
                UNITY_INITIALIZE_OUTPUT(v2f,o);

                o.pos = UnityObjectToClipPos(v.vertex);

#if _MESHINFO_UV0
                o.uv = float4( v.texcoord0.xy, 0, 0 );
#elif _MESHINFO_UV1
                o.uv = float4( v.texcoord1.xy, 0, 0 );
#elif _MESHINFO_NORMALS
                o.color.xyz = v.normal * 0.5 + 0.5;
                o.color.w = 1.0;
#elif _MESHINFO_TANGENTS
                o.color = v.tangent * 0.5 + 0.5;
#elif _MESHINFO_BITANGENTS
                float3 bitangent = cross( v.normal, v.tangent.xyz ) * v.tangent.w;
                o.color.xyz = bitangent * 0.5 + 0.5;
                o.color.w = 1.0;
#else
                o.color = v.color;
#endif

                return o;
            }

            fixed4 frag (v2f i) : SV_Target 
            {
#if _MESHINFO_UV0 || _MESHINFO_UV1
                half4 c = frac( i.uv );
                if (any(saturate(i.uv) - i.uv))
                {
                    c.b = 0.5;
                }
                return c;
#endif
                return i.color;
            }
            ENDCG
        }
    } 
}