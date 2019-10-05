Shader "Unlit/Glitch01"
{
    Properties
    {
       
        _MainTex ("Texture", 2D) = "white" {}
//        _NoiseMap("NoiseTexture",2D) = "white" {}
        _NoiseSeedX("NoiseSeedX", float) = 0  
        _NoiseSeedY("NoiseSeedY", float) = 0  
        _Alpha("Alpha", float) = 0
        [MaterialToggle]_IsGlitch("Is Glitch", Float) = 0
        _BGSlider("BGSlider", Range(0.,1.)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100
        
        ZWrite Off
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
//            sampler2D _NoiseMap;
            float4 _MainTex_ST;
            float _NoiseSeedX;
            float _NoiseSeedY;
            float _IsGlitch;
            float _Alpha;
            float _BGSlider;
             float random (fixed2 p) 
            { 
                return frac(sin(dot(p, fixed2(12.9898,78.233))) * 43758.5453);
            }
    
            float noise(fixed2 st)
            {
                fixed2 p = floor(st);
                return random(p);
            }

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
               
                float t = fmod(_Time.y, 1.);
                t = step(t, 0.9);
                
                
                float c = noise( i.uv*32*random(fixed2(_NoiseSeedX+_Time.x*0.001,_NoiseSeedY)) * t);        
                if(c > 0.5) c= 0.;
//                if(c <= 0.5) c = 0.02;
                float2 newuv = i.uv;
                
                if(_IsGlitch == 1)newuv += float2(c,c) * (1.-distance(i.uv,float2(0.5,0.5))/0.5);
                fixed4 col = tex2D(_MainTex, newuv);
//                if(col.r < 1) col = fixed4(0,0,0,0);
                if(col.a < 0.3) col.a = 0;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);    
//                col = fixed4(c,c,c,1);

                if(_BGSlider > i.uv.x && _BGSlider > 0)
                {
//                    col = float4(1,1,1,1);
//                    float a = tex2D(_MainTex, i.uv).a;
                    col.rgb = float3(1-col.r,1-col.g,1-col.b);
                    col.a = 1;
                }
                col.a *= _Alpha;
                return col;
            }
            ENDCG
        }
    }
}
