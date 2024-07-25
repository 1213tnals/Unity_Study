Shader "Custom/RainbowShader"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Speed ("Speed", Range(0.0, 10.0)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        sampler2D _MainTex;
        float _Speed;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        half4 GenerateRainbow(float time)
        {
            float3 color = float3(
                0.5 + 0.5 * sin(time + 0.0),
                0.5 + 0.5 * sin(time + 2.0),
                0.5 + 0.5 * sin(time + 4.0)
            );
            return half4(color, 1.0);
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float time = _Time.y * _Speed;
            half4 rainbow = GenerateRainbow(time);
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * rainbow.rgb;
            o.Alpha = 1.0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
