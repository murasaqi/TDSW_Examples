Shader "Unlit/TextAnimationTransparent"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Alpha("Alpha", Range(0.,1.)) = 1
//        [MaterialToggle] _IsBackground("Is Background", Float) = 0
        _BGSlider("BGSlider", Range(0.,1.)) = 0
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
            float _IsBackground;
            float _BGSlider;
            
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
                fixed4 col = tex2D(_MainTex, i.uv);
                float4 bgColor = float4(1,1,1,0);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                
                
//                if(_IsBackground)
//                {
                if(_BGSlider > i.uv.x && _BGSlider > 0)
                {
                    col = float4(1,1,1,1);
                    float a = tex2D(_MainTex, i.uv).a;
                    col.rgb -= float3(a,a,a);
                }
                    
                    
//                    col.a = 1;
//                }
                
                col.a *= _Alpha;
//                col = bgColor;
                return col;
            }
            ENDCG
        }
    }
}
