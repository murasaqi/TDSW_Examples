Shader "Unlit/SineWave"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Threshold("Threshold", float) = 0.5
        _Speed("Speed", float) = 3.
        _LineWidth("Line Width", Range(0.001,0.6)) = 0.456
        _Amp("Amp", float) = 2.4
        _Radius("Radius", float) = 0.13
        _Timer("Timer", float) = 0.
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
            float4 _MainTex_ST;
            float _Threshold;
            float _LineWidth;
            float _Radius;
            float _Amp;
            float _Timer;
            float _Speed;

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
//                _Threshold = _Time.y*_Speed % (1 + _LineWidth*1.3);
                fixed4 col = float4(1,1,1,1);
                float th = _Threshold + sin(i.uv.y * _Amp + _Timer*3.) * _Radius;
                if(i.uv.x < th) th +=  abs(cos(i.uv.y * _Amp + _Timer*3. * 10000)) * 0.01;
                float dist = clamp(distance( float2(th,i.uv.y), i.uv),0.0,_LineWidth);
                if(i.uv.x > th) dist = _LineWidth;
                float c = 1.-dist/_LineWidth;
                c = pow(c,4);
                col.a = c;
                return col;
            }
            ENDCG
        }
    }
}
