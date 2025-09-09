Shader "Unlit/ToonLavaHeightMaskScrollDistortDisplaceEmit"
{
    Properties
    {
        _MainTex("Gradient Ramp", 2D) = "white" {}
        _MainScroll("Main Scroll (X,Y)", Vector) = (0,0,0,0)

        _HeightTex("Height Map", 2D) = "gray" {}
        _HeightScroll("Height Scroll (X,Y)", Vector) = (0,0,0,0)

        _NoiseTex("Vertex/Height Noise", 2D) = "gray" {}
        _NoiseScroll("Noise Scroll (X,Y)", Vector) = (0.1,0.05,0,0)
        _DisplacementStrength("Displacement Strength", Range(0,1)) = 0.1

        _HeightIntensity("Height Influence", Range(0,1)) = 0.5
        _HeightNoiseBlend("Height–Noise Blend", Range(0,1)) = 0.5

        _DistortionStrength("UV Distortion Strength", Range(0,0.2)) = 0.05
        _DistortionSpeed("UV Distortion Speed", Range(0,5)) = 1.0

        _EmissionColor("Emission Color", Color) = (1,0.5,0,1)
        _EmissionStrength("Emission Strength", Range(0,5)) = 1.0
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" "Queue" = "Geometry+1" }
            LOD 200

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
                    float3 normal : NORMAL;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float2 uvHeight : TEXCOORD1;
                    float2 uvNoise : TEXCOORD2;
                    UNITY_FOG_COORDS(3)
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float4 _MainScroll;

                sampler2D _HeightTex;
                float4 _HeightTex_ST;
                float4 _HeightScroll;

                sampler2D _NoiseTex;
                float4 _NoiseTex_ST;
                float4 _NoiseScroll;
                float _DisplacementStrength;

                float _HeightIntensity;
                float _HeightNoiseBlend;

                float _DistortionStrength;
                float _DistortionSpeed;

                float4 _EmissionColor;
                float _EmissionStrength;

                // Hash/noise functions
                float2 hash(float2 p)
                {
                    p = float2(dot(p, float2(127.1, 311.7)),
                               dot(p, float2(269.5, 183.3)));
                    return -1.0 + 2.0 * frac(sin(p) * 43758.5453);
                }

                float noise(float2 p)
                {
                    const float K1 = 0.366025404;
                    const float K2 = 0.211324865;

                    float2 i = floor(p + (p.x + p.y) * K1);
                    float2 a = p - i + (i.x + i.y) * K2;
                    float2 o = (a.x > a.y) ? float2(1.0,0.0) : float2(0.0,1.0);
                    float2 b = a - o + K2;
                    float2 c = a - 1.0 + 2.0 * K2;

                    float3 h = max(0.5 - float3(dot(a,a), dot(b,b), dot(c,c)), 0.0);
                    float3 n = h * h * h * h * float3(dot(a, hash(i + 0.0)),
                                                      dot(b, hash(i + o)),
                                                      dot(c, hash(i + 1.0)));
                    return dot(n, float3(70.0,70.0,70.0));
                }

                v2f vert(appdata v)
                {
                    v2f o;

                    // Scroll UVs for noise texture
                    float2 noiseUV = TRANSFORM_TEX(v.uv, _NoiseTex) + _Time.y * _NoiseScroll.xy;

                    // Sample displacement value
                    float noiseSample = tex2Dlod(_NoiseTex, float4(noiseUV,0,0)).r;

                    // Displace vertex along normal
                    float3 displaced = v.vertex.xyz + v.normal * (noiseSample - 0.5) * _DisplacementStrength;
                    o.vertex = UnityObjectToClipPos(float4(displaced,1.0));

                    // Base & height UVs with scroll
                    float2 mainUV = TRANSFORM_TEX(v.uv, _MainTex) + _Time.y * _MainScroll.xy;
                    float2 heightUV = TRANSFORM_TEX(v.uv, _HeightTex) + _Time.y * _HeightScroll.xy;

                    // Distortion
                    float t = _Time.y * _DistortionSpeed;
                    float2 distortMain = float2(noise(mainUV * 2.0 + t), noise(mainUV * 2.0 - t));
                    float2 distortHeight = float2(noise(heightUV * 2.0 - t), noise(heightUV * 2.0 + t));

                    o.uv = mainUV + distortMain * _DistortionStrength;
                    o.uvHeight = heightUV + distortHeight * _DistortionStrength;
                    o.uvNoise = noiseUV;

                    UNITY_TRANSFER_FOG(o,o.vertex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // Base ramp sample
                    fixed4 baseCol = tex2D(_MainTex, i.uv);

                // Height & noise
                float heightVal = tex2D(_HeightTex, i.uvHeight).r;
                float noiseVal = tex2D(_NoiseTex, i.uvNoise).r;

                // Blend them
                float blendedVal = lerp(heightVal, noiseVal, _HeightNoiseBlend);

                // Shifted ramp sample
                float shiftedV = saturate(i.uv.y + blendedVal * _HeightIntensity);
                fixed4 rampFromHeight = tex2D(_MainTex, float2(i.uv.x, shiftedV));

                // Blend between base and shifted ramp
                fixed4 finalCol = lerp(baseCol, rampFromHeight, blendedVal);

                // Emission from blended map
                fixed4 emission = _EmissionColor * blendedVal * _EmissionStrength;
                finalCol.rgb += emission.rgb;

                UNITY_APPLY_FOG(i.fogCoord, finalCol);
                return finalCol;
            }
            ENDCG
        }
        }
}
