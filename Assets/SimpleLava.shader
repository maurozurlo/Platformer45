Shader "Custom/LavaShader_Gradient"
{
    Properties
    {
        _HeightMap("Height Map", 2D) = "gray" {}
        _NoiseTex("Noise Texture", 2D) = "white" {}

        _FlowSpeed("Flow Speed", Float) = 0.2
        _DistortionStrength("Distortion Strength", Float) = 0.2

        _GradientTex("Gradient Ramp", 2D) = "white" {} // 1D horizontal gradient
        _EmissionColor("Emission Color", Color) = (1,0.4,0,1)
        _EmissionIntensity("Emission Intensity", Float) = 2.0

        _NoiseMul("Noise Tiling", Float) = 2.0
        _DisplaceStrength("Displacement Strength", Float) = 0.2
        _DisplaceSpeed("Displacement Speed", Float) = 1.0
    }

        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 200

            CGPROGRAM
            #pragma surface surf Standard vertex:vert noshadow

            sampler2D _HeightMap;
            sampler2D _NoiseTex;
            sampler2D _GradientTex;

            float _FlowSpeed;
            float _DistortionStrength;
            float4 _EmissionColor;
            float _EmissionIntensity;
            float _NoiseMul;

            float _DisplaceStrength;
            float _DisplaceSpeed;

            struct Input
            {
                float2 uv_HeightMap;
            };

            void vert(inout appdata_full v)
            {
                float2 noiseUV = v.texcoord.xy * _NoiseMul + _Time.y * _DisplaceSpeed;
                float noiseSample = tex2Dlod(_NoiseTex, float4(noiseUV, 0, 0)).r;
                float displacement = (noiseSample - 0.5) * _DisplaceStrength;
                v.vertex.y += displacement;
            }

            void surf(Input IN, inout SurfaceOutputStandard o)
            {
                float heightVal = tex2D(_HeightMap, IN.uv_HeightMap).r;
                float finalHeight = saturate(heightVal);

                // Force sample gradient ramp
                float4 col = tex2Dlod(_GradientTex, float4(finalHeight, 0.5, 0, 0));

                o.Albedo = col.rgb;
                o.Emission = col.rgb * _EmissionIntensity;
                o.Metallic = 0.0;
                o.Smoothness = 0.4;
            }

            ENDCG
        }
            FallBack "Diffuse"
}
