// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/RGBShiftRect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Threshold("Threhold", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
            float _Threshold;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            
//            float4 rgbShift( in float2 p , in float4 shift) {
//                shift *= 2.0*shift.w - 1.0;
//                float2 rs = float2(shift.x,-shift.y);
//                float2 gs = float2(shift.y,-shift.z);
//                float2 bs = float2(shift.z,-shift.x);
//                
//                float r = tex2D(iChannel0, p+rs, 0.0).x;
//                float g = tex2D(iChannel0, p+gs, 0.0).y;
//                float b = tex2D(iChannel0, p+bs, 0.0).z;
//                
//                return float4(r,g,b,1.0);
//            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float4 col = fixed4(0,0,0,1);
//                float4 col_g = fixed4(0,0,0,1);
//                float4 col_b = fixed4(0,0,0,1);
                
                
                if(_Threshold > i.uv.x)
                {
                    col = fixed4(1,1,1,1);
                }
                
//                if(_Threshold > i.uv.x + 0.03)
//                {
//                    col_g = fixed4(1,1,1,1);
//                }
//                
//                if(_Threshold > i.uv.x + 0.01)
//                {
//                    col_b = fixed4(1,1,1,1);
//                }
//                
//                fixed4 col = fixed4(col_r.r,col_g.g,col_b.b,1);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
        
         // 1パス目の描画結果をテクスチャとして渡す
        GrabPass{}

        // 2パス目の最終出力
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
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

            sampler2D _GrabTexture;
            float4 _GrabTexture_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _GrabTexture);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
//                i.uv.x += sin(_Time.y)*0.01;
                fixed4 col = tex2D(_GrabTexture, i.uv);
//                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
    
}
