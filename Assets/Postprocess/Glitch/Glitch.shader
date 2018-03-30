Shader "Hidden/Museum/Glitch"
{
    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment Frag
            #include "Glitch.hlsl"
            ENDHLSL
        }
    }
}
