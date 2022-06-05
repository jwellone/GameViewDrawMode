Shader "jwellone/Editor/GameView/DrawNormals"
{
    SubShader
    {
        Tags{ "RenderType" = "Opaque" }
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            
            struct v2f
            {
                half3 worldNormal : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 color = 0;
                color.rgb = i.worldNormal * 0.5 + 0.5; 
                return color;
            }
            ENDCG
        }
    }
}
