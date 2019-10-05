Shader "Unlit/Glitch04"
{
   
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex("Texture", 2D) = "white" {}
        _Alpha("Alpha", float) = 0
     [MaterialToggle]_IsGlitch("Is Glitch", Float) = 1
      _BGSlider("BGSlider", Range(0.,1.)) = 0
    }
    SubShader
    {
        // No culling or depth
         Tags { "RenderType"="Transparent" }
        LOD 100
        
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha 

 
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
 
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
             
            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float4 _MainTex_TexelSize;
            float _IsGlitch;
            float _Alpha;
            float _BGSlider;
            fixed4 frag (v2f i) : SV_Target {
                
                float4 result = tex2D( _MainTex, i.uv );
               
                
                float2 uv = i.uv;
                
                float block_x = floor(_ScreenParams.x * i.uv.x / 16); 
                float block_y = floor(_ScreenParams.y * i.uv.y / 16);
                float2 block = float2(block_x,block_y);
                
                
                
                float noiseScale = 128;
                float2 uv_noise = float2(block.x / noiseScale,block.y/noiseScale);
                uv_noise += float2(
                    floor(_Time.y * 1234.0 / 64),
                    floor(_Time.y * 3543.0 / 64)
                );
                
                float block_thresh = pow(frac(_Time.y * 1236.0453), 2.0) * 0.2;
                float line_thresh =  pow(frac(_Time.y * 2236.0453), 3.0) * 0.7;
               
                float2 uv_r = uv, uv_g = uv, uv_b = uv;
            
                // glitch some blocks and lines
                if (tex2D(_NoiseTex, uv_noise).r < block_thresh ||
                    tex2D(_NoiseTex, float2(uv_noise.y, 0.0)).g < line_thresh) {
            
                    float2 dist = (frac(uv_noise) - 0.5) * 0.8;
                    uv_r += dist * 0.1;
                    uv_g += dist * 0.2;
                    uv_b += dist * 0.125;
                }
            
            
                result.r = tex2D(_MainTex, uv_r).r;
                result.g = tex2D(_MainTex, uv_g).g;
                result.b = tex2D(_MainTex, uv_b).b;
            
                // loose luma for some blocks
                if (tex2D(_NoiseTex, uv_noise).g < block_thresh)
                    result.rgb = result.ggg;
            
                // discolor block lines
                if (tex2D(_NoiseTex, float2(uv_noise.y, 0.0)).b * 3.5 < line_thresh)
                    result.rgb = float3(0.0, dot(result.rgb, float3(1.0,1.0,1.0)), 0.0);
            
                // interleave lines in some blocks
                if (tex2D(_NoiseTex, uv_noise).g * 1.5 < block_thresh ||
                    tex2D(_NoiseTex, float2(uv_noise.y, 0.0)).g * 2.5 < line_thresh) {
                    float _line = frac(_ScreenParams.y * i.uv.y / 3.0);
                    float3 mask = float3(3.0, 0.0, 0.0);
                    if (_line > 0.333)
                        mask = float3(0.0, 3.0, 0.0);
                    if (_line > 0.666)
                        mask = float3(0.0, 0.0, 3.0);
                    
                    result.xyz *= mask;
                }
                
                if(_IsGlitch == 0) result = tex2D( _MainTex, i.uv );
                
                if(_BGSlider > i.uv.x && _BGSlider > 0)
                {
                    result.rgb = float3(1-result.r,1-result.g,1-result.b);
                    result.a = 1;
                }
                result.a *= _Alpha;
                 return result;
                
            }
            ENDCG
        }
    }
}
