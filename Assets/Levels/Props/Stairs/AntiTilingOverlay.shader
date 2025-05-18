Shader "Custom/AntiTilingOverlayFull"
{
    Properties
    {
        _MainTex("Base Albedo", 2D) = "white" {}
        _MainTexUV("Base UV Channel (0 or 1)", Float) = 0

        _NormalMap("Normal Map", 2D) = "bump" {}
        _NormalMapUV("Normal UV Channel (0 or 1)", Float) = 0

        _OverlayTex("Overlay Grunge", 2D) = "gray" {}
        _OverlayStrength("Overlay Blend", Range(0,1)) = 0.5
        _OverlayUV("Overlay UV Channel (0 or 1)", Float) = 0

        _Smoothness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
        _NormalStrength("Normal Strength", Range(0,2)) = 1.0

    }

        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            CGPROGRAM
            #pragma target 3.0
            #pragma surface surf Standard fullforwardshadows

            sampler2D _MainTex;
            sampler2D _NormalMap;
            sampler2D _OverlayTex;

            float _OverlayTiling;
            float _OverlayStrength;
            
            float _MainTexUV;
            float _NormalMapUV;
            float _OverlayUV;

            float _Smoothness;
            float _Metallic;
            float _NormalStrength;


            struct Input
            {
                float2 uv_MainTex;
                float2 uv2_MainTex2;

                float2 uv_NormalMap;
                float2 uv2_NormalMap2;

                float2 uv_OverlayTex;
                float2 uv2_OverlayTex2;

                float3 worldPos;
                INTERNAL_DATA
            };

            void surf(Input IN, inout SurfaceOutputStandard o)
            {
                // === Select UV sets ===
                float2 uvBase = (_MainTexUV < 0.5) ? IN.uv_MainTex : IN.uv2_MainTex2;
                float2 uvNormal = (_NormalMapUV < 0.5) ? IN.uv_NormalMap : IN.uv2_NormalMap2;

                // === Base Albedo ===
                float4 baseCol = tex2D(_MainTex, uvBase);

                // === Overlay (world-space grunge) ===
                //float2 overlayUV = IN.worldPos.xz * _OverlayTiling;
                float2 uvOverlay = (_OverlayUV < 0.5) ? IN.uv_OverlayTex : IN.uv2_OverlayTex2;
                float4 overlay = tex2D(_OverlayTex, uvOverlay * _OverlayTiling);
                baseCol.rgb = lerp(baseCol.rgb, overlay.rgb, _OverlayStrength);

                // Normal
                float3 normalTS = UnpackNormal(tex2D(_NormalMap, uvNormal));
                normalTS.xy *= _NormalStrength;
                o.Normal = normalTS;

                // Final
                o.Albedo = baseCol.rgb;
                o.Metallic = _Metallic;
                o.Smoothness = _Smoothness;
                o.Alpha = 1;
            }
            ENDCG
        }
            FallBack "Diffuse"
}
