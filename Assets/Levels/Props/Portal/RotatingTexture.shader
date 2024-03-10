Shader "Custom/RotatingTexture"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _RotationSpeed("Rotation Speed", Float) = 30
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }

            CGPROGRAM
            #pragma surface surf Lambert

            struct Input
            {
                float2 uv_MainTex;
            };

            sampler2D _MainTex;
            float _RotationSpeed;

            void surf(Input IN, inout SurfaceOutput o)
            {
                // Rotate the UV coordinates
                float2 rotatedUV = IN.uv_MainTex;
                float angle = _Time.y * _RotationSpeed; // Rotation speed
                float2x2 rotationMatrix = float2x2(cos(angle), -sin(angle), sin(angle), cos(angle));
                rotatedUV -= 0.5; // Translate UV origin to center
                rotatedUV = mul(rotationMatrix, rotatedUV);
                rotatedUV += 0.5; // Translate UV origin back
                o.Albedo = tex2D(_MainTex, rotatedUV).rgb;
            }
            ENDCG
        }
            FallBack "Diffuse"
}
