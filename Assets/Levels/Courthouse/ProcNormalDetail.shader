Shader "Custom/ProcNormalDetail"
{
    Properties{
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        [NoScaleOffset] _BumpMap("Normal Map", 2D) = "bump" {}
        _NormalStrength("Normal Strength", Range(0, 2)) = 1.0
        _Glossiness("Smoothness", Range(0, 1)) = 0.5
        [Gamma] _Metallic("Metallic", Range(0, 1)) = 0
        [NoScaleOffset] _OcclusionMap("Occlusion", 2D) = "white" {}
        _OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
    }
        SubShader{
            Tags { "RenderType" = "Opaque" }
            LOD 200

            CGPROGRAM
            #pragma surface surf Standard fullforwardshadows
            #pragma target 3.0

            #include "UnityStandardUtils.cginc"

            // Flip UVs for triplanar projection
            #define TRIPLANAR_CORRECT_PROJECTED_U
            //#define TRIPLANAR_UV_OFFSET

            half3 blend_rnm(half3 n1, half3 n2)
            {
                n1.z += 1;
                n2.xy = -n2.xy;
                return n1 * dot(n1, n2) / n1.z - n2;
            }

            sampler2D _MainTex;
            sampler2D _BumpMap;
            sampler2D _OcclusionMap;

            half _Glossiness;
            half _Metallic;
            half _OcclusionStrength;
            float _NormalStrength;

            struct Input {
                float2 uv_MainTex;     // mesh UVs for albedo
                float3 worldPos;
                float3 worldNormal;
                INTERNAL_DATA
            };

            float3 WorldToTangentNormalVector(Input IN, float3 normal) {
                float3 t2w0 = WorldNormalVector(IN, float3(1,0,0));
                float3 t2w1 = WorldNormalVector(IN, float3(0,1,0));
                float3 t2w2 = WorldNormalVector(IN, float3(0,0,1));
                float3x3 t2w = float3x3(t2w0, t2w1, t2w2);
                return normalize(mul(t2w, normal));
            }

            void surf(Input IN, inout SurfaceOutputStandard o) {
                // Fix Unity bug: IN.worldNormal is (0,0,0)
                IN.worldNormal = WorldNormalVector(IN, float3(0, 0, 1));

                // Sample albedo using mesh UVs
                fixed4 col = tex2D(_MainTex, IN.uv_MainTex);

                // Triplanar blending mask based on normal
                half3 triblend = saturate(pow(abs(IN.worldNormal), 4));
                triblend /= max(dot(triblend, half3(1, 1, 1)), 0.0001);

                // Generate triplanar UVs
                float2 uvX = IN.worldPos.zy;
                float2 uvY = IN.worldPos.xz;
                float2 uvZ = IN.worldPos.xy;

                // Axis sign correction
                half3 axisSign = IN.worldNormal < 0 ? -1 : 1;

                // Flip U axis if needed (correct back projection)
                uvX.x *= axisSign.x;
                uvY.x *= axisSign.y;
                uvZ.x *= -axisSign.z;

                // Occlusion (triplanar)
                half occX = tex2D(_OcclusionMap, uvX).g;
                half occY = tex2D(_OcclusionMap, uvY).g;
                half occZ = tex2D(_OcclusionMap, uvZ).g;
                half occ = LerpOneTo(occX * triblend.x + occY * triblend.y + occZ * triblend.z, _OcclusionStrength);

                // Normal maps (triplanar)
                half3 tnormalX = UnpackNormal(tex2D(_BumpMap, uvX));
                half3 tnormalY = UnpackNormal(tex2D(_BumpMap, uvY));
                half3 tnormalZ = UnpackNormal(tex2D(_BumpMap, uvZ));

                tnormalX.x *= axisSign.x;
                tnormalY.x *= axisSign.y;
                tnormalZ.x *= -axisSign.z;

                half3 absVertNormal = abs(IN.worldNormal);

                // Blend normals using RNM
                tnormalX = blend_rnm(half3(IN.worldNormal.zy, absVertNormal.x), tnormalX);
                tnormalY = blend_rnm(half3(IN.worldNormal.xz, absVertNormal.y), tnormalY);
                tnormalZ = blend_rnm(half3(IN.worldNormal.xy, absVertNormal.z), tnormalZ);

                tnormalX.z *= axisSign.x;
                tnormalY.z *= axisSign.y;
                tnormalZ.z *= axisSign.z;

                half3 worldNormal = normalize(
                    tnormalX.zyx * triblend.x +
                    tnormalY.xzy * triblend.y +
                    tnormalZ.xyz * triblend.z
                );

                // Final surface output
                o.Albedo = col.rgb;
                o.Metallic = _Metallic;
                o.Smoothness = _Glossiness;
                o.Occlusion = occ;
                float3 baseWorldNormal = WorldNormalVector(IN, float3(0, 0, 1));
                float3 blendedNormal = normalize(lerp(baseWorldNormal, worldNormal, _NormalStrength));
                o.Normal = WorldToTangentNormalVector(IN, blendedNormal);
            }
                ENDCG
        }
            FallBack "Diffuse"
}
