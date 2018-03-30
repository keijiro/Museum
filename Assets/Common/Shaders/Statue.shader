Shader "Museum/Statue"
{
    Properties
    {
        _Color("Color", Color) = (1, 1, 1, 1)
        _Glossiness("Smoothness", Range(0, 1)) = 0.9
        [Gamma] _Metallic("Metallic", Range(0, 1)) = 1

        [Space]
        _BumpMap("Normal Map", 2D) = "bump" {}
        _BumpScale("Scale", Range(0, 2)) = 1

        [Space]
        _OcclusionMap("Occlusion Map", 2D) = "white" {}
        _OcclusionStrength("Strength", Range(0, 1)) = 1

        [Space]
        _DetailNormalMap("Detail Normal", 2D) = "bump" {}
        _DetailNormalMapScale("Scale", Range(0, 2)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="TransparentCutout" "Queue"="AlphaTest" }

        Cull Off

        CGPROGRAM

        #pragma surface Surface Standard vertex:Vertex alphatest:_Statue_Cutoff addshadow noshadowmask nodynlightmap nodirlightmap nolightmap
        #pragma target 4.0

        #include "SimplexNoise3D.hlsl"

        struct Input
        {
            float2 uv_BumpMap;
            float2 uv_DetailNormalMap;
            float3 worldPos;
            float vface : VFACE;
        };

        half3 _Color;
        half _Glossiness;
        half _Metallic;

        sampler2D _BumpMap;
        half _BumpScale;

        sampler2D _OcclusionMap;
        half _OcclusionStrength;

        sampler2D _DetailNormalMap;
        half _DetailNormalMapScale;

        half _Statue_NoiseAmplitude;

        void Vertex(inout appdata_full v)
        {
            float3 np = v.vertex.xyz * 10 + float3(0, 0, -2) * _Time.y;
            v.vertex.xyz += v.normal * snoise(np) * 0.02 * _Statue_NoiseAmplitude;
        }

        void Surface(Input IN, inout SurfaceOutputStandard o)
        {
            half phi = atan2(IN.worldPos.z, IN.worldPos.x);
            float y = IN.worldPos.y * 8 + phi / UNITY_PI / 2 + _Time.y;
            y += snoise(IN.worldPos * 3 - float3(0, 0, _Time.y)) * _Statue_NoiseAmplitude;
            o.Alpha = frac(y);

            float flip = IN.vface < 0;
            o.Albedo = lerp(_Color, half3(1, 0, 0), flip);

            half4 ns1 = tex2D(_BumpMap, IN.uv_BumpMap);
            half4 ns2 = tex2D(_DetailNormalMap, IN.uv_DetailNormalMap);
            half3 n1 = UnpackScaleNormal(ns1, _BumpScale);
            half3 n2 = UnpackScaleNormal(ns2, _DetailNormalMapScale);
            o.Normal = lerp(BlendNormals(n1, n2), half3(0, 0, -1), flip);

            half4 os = tex2D(_OcclusionMap, IN.uv_BumpMap);
            o.Occlusion = LerpOneTo(os.g, _OcclusionStrength);

            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
        }

        ENDCG
    }

    FallBack "Diffuse"
}
