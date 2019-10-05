Shader "Custom/GpuInstanceText"
{
     Properties
    {
        //_Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _SourceTex("Source (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _CellSize("Cell size", float) = 4
        _FrameCount("Frame Count", int) =1
        _Cell_U("Cell U", float) = 1
        _Cell_V("Cell V", float) = 1
        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _SourceTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        int _CellSize;
        int _FrameCount;
        float _Cell_U;
        float _Cell_V;
        //fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
            UNITY_DEFINE_INSTANCED_PROP(fixed4, _Color)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            //fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            
            
////            
            
            
            
            int size = floor(_CellSize);
            
            
            
            float4 source = tex2D(_SourceTex, float2(_Cell_U,_Cell_V));
            float powerLevel = 2;
            float whiteLevel = (pow(source.x,powerLevel)+pow(source.y,powerLevel)+pow(source.z,powerLevel))/3.0;
            
            float step = 1.0/(size*size-1);
            float whiteLevelCount = 0.0;
            while(whiteLevelCount < whiteLevel)
            {
                whiteLevelCount += step;
            }
//            whiteLevelCount = clamp(whiteLevelCount -step,0,1);
            int frame = floor(whiteLevelCount/step);
//            frame = 4;
            float offset_x = fmod(frame,size)/size;
            float offset_y = floor(frame / size)/size;
            //int frame = floor(_FrameCount);
            
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex / _CellSize + float2(offset_x,offset_y));
            
            
            
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
