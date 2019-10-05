Shader "Unlit/TextAnimation_Slash"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "black" {}
        _Alpha("Alpha", Range(0.,1.)) = 1
        _LineWidth("LineWidth", Range(0.,0.2)) = 0.05
        _LineStart("LineStart",Range(0,1)) = 0
        _LineEnd("LineEnd",Range(0,1)) = 0
        
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        LOD 100
    
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha 
        Cull Off

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
            float _Alpha;
            float _LineWidth;
            float _LineStart;
            float _LineEnd;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) * float4(0,0,0,1);
                // apply fog
//                UNITY_APPLY_FOG(i.fogCoord, col);
                
                
                float dist = clamp(distance(float2(0.5,i.uv.y), i.uv), 0.,_LineWidth);
                
                if(dist < _LineWidth &&
                    1-i.uv.y > _LineStart &&
                    i.uv.y > 1-_LineEnd)col = float4(1,1,1,1);
                
                
                col.a *= _Alpha;
                return col;
            }
            ENDCG
        }
    }
}
