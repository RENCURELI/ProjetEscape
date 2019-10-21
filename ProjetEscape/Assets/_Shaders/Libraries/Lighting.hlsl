#if !defined(LIGHTING_LIB)
#define LIGHTING_LIB

#include "Generic.hlsl"

int LightCount = 0;
float3 LightPosition[8];
float3 LightColor[8];
float LightDistance[8];
float LightIntensity[8];
float3 LightDir[8];
float3 LightInfo[8]; //x = type, y = bias
float4x4 LightProjection[8];

Texture2D LightTexture_0;
Texture2D LightTexture_1;
Texture2D LightTexture_2;
Texture2D LightTexture_3;
Texture2D LightTexture_4;
Texture2D LightTexture_5;
Texture2D LightTexture_6;
Texture2D LightTexture_7;

SamplerState sampler_point_repeat;

float4 GetProjection(float3 worldPosition, float4x4 projectionMatrix) {
	float4 projection = mul(projectionMatrix, float4(worldPosition, 1));
	projection.x += 1;
	projection.x *= 0.5;
	projection.y += 1;
	projection.y *= 0.5;
	return projection;
}

float3 GetLighting(GenericFragmentInput input)
{
    if (LightCount == 0)
        return float3(1, 1, 1);
    float3 final = float3(0, 0, 0);
    for (int i = 0; i < LightCount; i++)
    {
        float3 color = LightColor[i];
        float3 position = LightPosition[i];
        float radius = LightDistance[i];
        float intensity = LightIntensity[i];
        float3 info = LightInfo[i];
        float3 dir = LightDir[i];
        float distanceToPixel = distance(input.worldPosition, position);
        float falloff = (1 - saturate(distanceToPixel / radius));
        if (info.x == 0)
        {
            final += color * falloff * intensity;
        }
        else if (info.x == 1 || info.x == 2)
        {
            final += color * intensity;
        }
    }
    return final;
}

float3 GetLightingDiffuse(GenericFragmentInput input)
{
    if (LightCount == 0)
        return float3(1, 1, 1);
    float3 final = float3(0, 0, 0);
    for (int i = 0; i < LightCount; i++)
    {
        float3 color = LightColor[i];
        float3 position = LightPosition[i];
        float radius = LightDistance[i];
        float intensity = LightIntensity[i];
        float3 info = LightInfo[i];
        float3 dir = LightDir[i];
        float distanceToPixel = distance(input.worldPosition, position);
        float falloff = (1 - saturate(distanceToPixel / radius));
        if (info.x == 0)
        {
            float3 lightDir = normalize(input.worldPosition - position);
            float ndotl = dot(normalize(input.normal), normalize(-lightDir));
            final += max(0, color * falloff * intensity * saturate(ndotl));
        }
        else if (info.x == 1)
        {
            final += color * intensity;
        }
        else if (info.x == 2)
        {
            float ndotl = dot(normalize(input.normal), normalize(-dir));
            final += max(0, color * intensity * saturate(ndotl));
        }
    }
    return final;
}

float3 GetLightingProjection(GenericFragmentInput input)
{
	if (LightCount == 0)
		return float3(1, 1, 1);
	float3 final = float3(1,1,1);
	Texture2D cookies[8];
	cookies[0] = LightTexture_0;
	cookies[1] = LightTexture_1;
	cookies[2] = LightTexture_2;
	cookies[3] = LightTexture_3;
	cookies[4] = LightTexture_4;
	cookies[5] = LightTexture_5;
	cookies[6] = LightTexture_6;
	cookies[7] = LightTexture_7;
	[unroll(8)]
	for (int i = 0; i < LightCount; i++)
	{
		float3 color = LightColor[i];
		float3 position = LightPosition[i];
		float radius = LightDistance[i];
		float intensity = LightIntensity[i];
		float3 info = LightInfo[i];
		float3 dir = LightDir[i];
		float4 projection = GetProjection(input.worldPosition, LightProjection[i]);

		if (info.x == 2)
		{
			if (projection.x >= 0 && projection.x <= 1 && projection.y >= 0 && projection.y <= 1) {
				final.r = projection.x;
				final.g = projection.y;
				final.b = projection.z;
				float shadow = cookies[i].Sample(sampler_point_repeat, projection.xy).r;
				float depth = distance(input.worldPosition, position);
				if (depth > shadow + 1) {
					final.rgb = float3(0, 0, 0);
				}
				else {
					final.rgb = float3(1, 1, 1);
				}
			}
		}
	}
	return final;
}

float3 GetLightingDiffuseShadow(GenericFragmentInput input)
{
	if (LightCount == 0)
		return float3(1, 1, 1);
	float3 final = float3(0,0,0);
	Texture2D cookies[8];
	cookies[0] = LightTexture_0;
	cookies[1] = LightTexture_1;
	cookies[2] = LightTexture_2;
	cookies[3] = LightTexture_3;
	cookies[4] = LightTexture_4;
	cookies[5] = LightTexture_5;
	cookies[6] = LightTexture_6;
	cookies[7] = LightTexture_7;
	[unroll(8)]
	for (int i = 0; i < LightCount; i++)
	{
		float3 color = LightColor[i];
		float3 position = LightPosition[i];
		float radius = LightDistance[i];
		float intensity = LightIntensity[i];
		float3 info = LightInfo[i];
		float3 dir = LightDir[i];
		float4 projection = GetProjection(input.worldPosition, LightProjection[i]);

		float distanceToPixel = distance(input.worldPosition, position);
		float falloff = (1 - saturate(distanceToPixel / radius));
		if (info.x == 0)
		{
			float3 lightDir = normalize(input.worldPosition - position);
			float ndotl = dot(normalize(input.normal), normalize(-lightDir));
			final += max(0, color * falloff * intensity * saturate(ndotl));
		}
		else if (info.x == 1)
		{
			final += color * intensity;
		}
		else if (info.x == 2)
		{
			float ndotl = dot(normalize(input.normal), normalize(-dir));
			if (projection.x >= 0 && projection.x <= 1 && projection.y >= 0 && projection.y <= 1) {
				float shadow = cookies[i].Sample(sampler_point_repeat, projection.xy).r;
				float depth = distance(input.worldPosition, position);
				if (depth > shadow + info.y) {
				}
				else {
					final += max(0, color * intensity * saturate(ndotl));
				}
			}
			else {
				final += max(0, color * intensity * saturate(ndotl));
			}
		}
	}
	return final;
}

#endif