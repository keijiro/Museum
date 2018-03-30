#include "PostProcessing/Shaders/StdLib.hlsl"
#include "PostProcessing/Shaders/Colors.hlsl"
#include "../../../Common/Shaders/Math.hlsl"
#include "../../../Common/Shaders/SimplexNoise2D.hlsl"

TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);

half _Intensity;

half4 Frag(VaryingsDefault i) : SV_Target
{
    const float2 reso = _ScreenParams.xy / 32;
    const uint2 ireso = (uint2)reso;

    uint2 block = i.texcoord * reso;
    uint id = block.x + block.y * ireso.x;

    uint stride = 1 + 8 * floor((1 + snoise(_Time.y)) * 3);
    uint segment = id / stride;

    float n1 = 0.5 * (1 + snoise(float2(segment, _Time.y * 3)));
    float n2 = 0.5 * (1 + snoise(float2(segment, _Time.y + 100)));

    half mask = n1 < _Intensity;
    id += mask * floor(n2 * 16) / 8 * ireso.x;

    float2 uv1 = float2(id % ireso.x, id / ireso.x) / reso;
    uv1 += fmod(i.texcoord, 1 / reso);

    float2 uv2 = i.texcoord;
    uv2.x += (Random((uv2.y + _Time.y * 60) * reso.y) - 0.5) * 0.03;

    return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv1);
}
