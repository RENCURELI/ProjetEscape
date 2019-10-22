#if !defined(LIGHTING_LIB)
#define LIGHTING_LIB

#include "Generic.hlsl"

#define MAX_VISIBLE_LIGHTS 8

CBUFFER_START(_LightBuffer)
float4 _VisibleLightColors[MAX_VISIBLE_LIGHTS];
float4 _VisibleLightDirections[MAX_VISIBLE_LIGHTS];
float4 _VisibleLightPositions[MAX_VISIBLE_LIGHTS];
float _VisibleLightTypes[MAX_VISIBLE_LIGHTS];
CBUFFER_END

/*
SamplerState sampler_point_repeat;

float4 GetProjection(float3 worldPosition, float4x4 projectionMatrix) {
	float4 projection = mul(projectionMatrix, float4(worldPosition, 1));
	projection.x += 1;
	projection.x *= 0.5;
	projection.y += 1;
	projection.y *= 0.5;
	return projection;
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
*/

struct Light {
	float3 direction;
	float3 color;
	float3 position;
	int type;

};

Light GetLight(int id) {
	Light l;
	l.color = _VisibleLightColors[id].rgb;
	l.direction = _VisibleLightDirections[id].xyz;
	l.position = _VisibleLightPositions[id].xyz;
	l.type = _VisibleLightTypes[id];
	return l;
}

/*
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

float3 DiffuseLight(Light l, float3 normal) {
	float3 lightColor = l.color;
	float3 lightDirection = l.direction;
	float diffuse = saturate(dot(normal, lightDirection));
	return diffuse * lightColor;
}

float GetSpecular(float3 position, float3 normal, float3 lightDir, float smoothness) {
	if (smoothness <= 0) return 0;
	float3 view = ViewDirection(position);
	float3 reflectionDir = reflect(normalize (-lightDir), normalize(normal));
	float vdotr = max(0,dot(normalize(-view), normalize(reflectionDir)));
	return pow(vdotr, smoothness * 100);
}

float3 GetLightDiffuseShadow(float3 position, float3 normal, float smoothness, float metallic, Light l) {
	float3 final = (float3)0;

	float4 projection = GetProjection(position, l.projection);
	float distanceToPixel = distance(position, l.position);
	float falloff = (1 - saturate(distanceToPixel / l.radius));
	float ndotl = dot(normalize(normal), normalize(-l.direction));
	float oneMinusReflectivity = 1 - metallic;
	if (smoothness <= 0) oneMinusReflectivity = 1;
	float attenuation = 1;
	float3 diffuse = (float3)0;
	float specular = 0;

	if (l.type == 0)// point
	{
		float3 lightDir = normalize(position - l.position);
		specular = GetSpecular(position, normal, -lightDir, smoothness) * l.color * l.intensity;
		ndotl = dot(normalize(normal), normalize(-lightDir));
		diffuse = max(0, l.color * l.intensity);
		attenuation = falloff * saturate(ndotl);
		specular *= diffuse * metallic;
		diffuse *= oneMinusReflectivity;
		final = diffuse * attenuation + specular * falloff;
	}
	else if (l.type == 1)
	{
		final += l.color * l.intensity;
	}
	else if (l.type == 2)
	{
		specular = GetSpecular(position, normal, l.direction, smoothness) * l.color * l.intensity;
		if (projection.x >= 0 && projection.x <= 1 && projection.y >= 0 && projection.y <= 1) {
			float shadow = l.shadow.Sample(sampler_point_repeat, projection.xy).r;
			float depth = distanceToPixel;
			if (depth > shadow + l.bias) {
			}
			else {
				diffuse = max(0, l.color * l.intensity);
				attenuation = falloff * saturate(ndotl);
				specular *= diffuse * metallic;
				diffuse *= oneMinusReflectivity;
				final = diffuse * attenuation + specular * falloff;
			}
		}
		else {
			diffuse = max(0, l.color * l.intensity) * oneMinusReflectivity;
			attenuation = falloff * saturate(ndotl);
			specular *= diffuse * metallic;
			diffuse *= oneMinusReflectivity;
			final = diffuse * attenuation + specular * falloff;
		}
	}
	return final;
}

float3 GetLighting(GenericFragmentInput input, float smoothness, float metallic)
{
	if (LightCount == 0)
		return float3(1, 1, 1);
	float3 final = float3(0, 0, 0);
	[unroll(8)]
	for (int i = 0; i < LightCount; i++)
	{
		Light l = GetLight(i);
		final += GetLightDiffuseShadow(input.worldPosition, input.normal, smoothness, metallic, l);
	}
	return final;
}
*/

float3 DiffuseLight(Light l, float3 normal, float3 position) {
	float3 lightColor = l.color;
	float3 lightDirection = l.direction;
	if (l.type == 2) {
		lightDirection = -normalize(position - l.position);
	}
	float diffuse = saturate(dot(normalize(normal), normalize(lightDirection)));
	return diffuse * lightColor;
}

float3 GetLighting(GenericFragmentInput input, float3 albedo)
{
	float3 diffuseLight = 0;
	for (int i = 0; i < 8; i++) {
		int lightIndex = i;
		Light l = GetLight(lightIndex);
		diffuseLight += DiffuseLight(l, input.normal, input.worldPosition);
	}
	float3 color = diffuseLight * albedo;
	return float4(color, 1);
}

#endif