// Credit: https://youtu.be/FIP6I1x6lMA

void ToonShading_float(in float3 Normal, in float ToonRampSmoothness, in float3 ClipSpacePos, in float3 WorldPos,
                       in float4 ToonRampTinting, in float ToonRampOffset, out float3 ToonRampOutput, out float3 Direction)
{
    #ifdef SHADERGRAPH_PREVIEW
    ToonRampOutput = float3(0.5,0.5,0);
    Direction = float3(0.5,0.5,0);
    #else

    // Grab the shadow coordinates
    #if SHADOWS_SCREEN
    half4 shadowCoord = ComputeScreenPos(ClipSpacePos);
    #else
    half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
    #endif

    // Grab the main light
    #if _MAIN_LIGHT_SHADOWS_CASCADE || _MAIN_LIGHT_SHADOWS
    Light light = GetMainLight(shadowCoord);
    #else
    Light light = GetMainLight();
    #endif

    // N dot L (And increasing minimum range so nothing's pitch black)
    half d = dot(Normal, light.direction) * 0.5 + 0.5;
    
    half toonRamp = smoothstep(ToonRampOffset, ToonRampOffset + ToonRampSmoothness, d);
    toonRamp *= light.shadowAttenuation;
    ToonRampOutput = light.color * (toonRamp + ToonRampTinting);

    Direction = light.direction;
    #endif
}