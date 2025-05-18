Shader "Custom/DetailTest"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0

        _DetailAlbedoMap("Detail Albedo UV MAP #2", 2D) = "gray" {}
        _DetailStrength("Detail Strength", Range(0,1)) = 1.0

        _DetailUV("Detail UV Channel", Float) = 0
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 200

            CGPROGRAM
            #pragma surface surf Standard fullforwardshadows
            #pragma target 3.0

            sampler2D _MainTex;
            sampler2D _DetailAlbedoMap;

            half _Glossiness;
            half _Metallic;
            fixed4 _Color;
            half _DetailStrength;
            float _DetailUV;

            struct Input
            {
                float2 uv_MainTex;
                float2 uv2_DetailAlbedoMap: TEXCOORD1;
            };

            void surf(Input IN, inout SurfaceOutputStandard o)
            {
                fixed4 baseCol = tex2D(_MainTex, IN.uv_MainTex) * _Color;

                // Choose which UV to use for detail texture based on _DetailUV value
                fixed4 detailCol = tex2D(_DetailAlbedoMap, IN.uv2_DetailAlbedoMap);

                // Blend detail texture with base albedo (modulate RGB, keep alpha from base)
                fixed3 finalAlbedo = lerp(baseCol.rgb, baseCol.rgb * detailCol.rgb * 2, _DetailStrength);

                o.Albedo = finalAlbedo;
                o.Metallic = _Metallic;
                o.Smoothness = _Glossiness;
                o.Alpha = baseCol.a;
            }
            ENDCG
        }
            FallBack "Diffuse"
}
