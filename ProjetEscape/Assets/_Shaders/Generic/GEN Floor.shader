﻿Shader "LOCAL/GEN/Floor"
{
    Properties
    {
		_MainTex("Texture", 2D) = "white" {}
		_MainColor("Color (Human)", color) = (1,1,1,1)
		_SpiritColor("Color (Spirit)", color) = (1,1,1,1)

		_Radius("Radius", float) = 5
		_Blend("Blend", float) = 1
		_Width("Width", float) = 1
		_Height("Height", float) = 1
		_Amplitude("Amplitude", float) = 1
		_Speed("Speed", float) = 1
		_Spread("Spread", float) = 1

		_PearlTex("Pearl Texture", 2D) = "white" {}
		_PearlColor("Pearl Color", color) = (1,1,1,1)

		TessellationLimit("Tessellation Limit", Range(10, 200)) = 30
		TessellationUniform("Tessellation Uniform", Range(1, 64)) = 1
		TessellationEdgeLength("Tessellation Edge Length", Range(5, 100)) = 50
	}
	SubShader
	{
		HLSLINCLUDE
		#define GENERIC_GEOMETRY
		#include "../Libraries/Generic.hlsl"

		float _SpiritWorldRadius;
		float3 _PlayerPosition;

		float TransitionFactor(GenericFragmentInput input){
			float3 pos = input.worldPosition;
			float m = 8;
			pos.x += cos(input.worldPosition.z * m);
			pos.z += sin(input.worldPosition.x * m);
			pos.y += cos(input.worldPosition.y * m);
			float distToPlayer = distance(pos, _PlayerPosition);
			if (distToPlayer < _SpiritWorldRadius) return 1;
			return 0;
		}

		ENDHLSL
	
		UsePass "LOCAL/GEN/Diffuse/Human"
		UsePass "LOCAL/GEN/Diffuse/Spirit"
		UsePass "LOCAL/GEN/Diffuse/Shadow"
		UsePass "LOCAL/GEN/Diffuse/Transition"

			Pass{

				Name "Pearls"
				Tags{"LightMode" = "Pearl" "Queue" = "Transparent" "RenderType" = "Opaque"}

				ZWrite Off
				ZTest Less
				Blend One One
				Cull Off

				HLSLPROGRAM

				#pragma target 3.5
				#pragma vertex TessellationVertexProgram
				#pragma fragment FragmentProgram
				#pragma hull HullProgram
				#pragma domain DomainProgram
				#pragma geometry GeometryProgram
				#pragma require geometry
				#pragma require tessellation tessHW

				#define TESSELLATION_SHARP
				//#define TESSELLATION_EDGE

				#include "../Libraries/Tessellation.hlsl"
				#include "../Libraries/Lighting.hlsl"

				float _Width;
				float _Height;
				float _Blend;
				float _Amplitude;
				float _Speed;
				float _Spread;

				const float _PearlDistance = 5;
				float3 _PearlFocusPoint[8];
				int _PearlFocusCount = 0;

				[maxvertexcount(6)]
				void GeometryProgram(triangle GenericFragmentInput i[3], inout TriangleStream<GenericFragmentInput> stream) {

					float3 p0 = i[0].worldPosition;
					float3 p1 = i[1].worldPosition;
					float3 p2 = i[2].worldPosition;

					float3 center = (p0 + p1 + p2) / 3.0;
					float3 normal = i[0].normal + i[1].normal + i[2].normal;
					normal = normalize(normal);

					float distToPlayer = distance(center, _PlayerPosition);

					if (distToPlayer > _SpiritWorldRadius) return;

					float3 toObjCenter = center - ObjectPosition();
					float rdm = (cos(toObjCenter.x) + sin(toObjCenter.z) + tan(toObjCenter.y)) * 10;

					float lifetime = 1 - fmod(time * _Speed + rdm, 2);
					i[0].color.r = lifetime;
					i[0].color.g = 0;

					float3 pos = center
						+ float3(1, 0, 0) * cos(lifetime + center.x + rdm) * 5
						+ float3(0, 0, 1) * sin(lifetime + center.y + rdm) * 5
						+ float3(0, 1, 0) * cos(lifetime) * 0.5;
					
					float pearlDistance = 8;
					for (int index = 0; index < _PearlFocusCount; index++) {
						float distToFocus = distance(_PearlFocusPoint[index], pos);
						if (distToFocus < pearlDistance) {
							float f = 1 - distToFocus / pearlDistance;
							float3 gravitPos = _PearlFocusPoint[index] + (float3(1, 0, 0) * cos(f+time) + float3(0, 0, 1) * sin(f + time))*(pearlDistance - distToFocus);
							pos = lerp(pos, gravitPos, f*0.8);
							i[0].color.g = f*2;
						}
					}

					float3 dir = normalize(-_CameraDirection);

					float size = 0.5;

					GenerateQuad(i[0], stream, pos, float2(1,1)*size, dir, float3(0,1,0));//normalize(-_CameraDirection)
				}

				sampler2D _PearlTex;
				float4 _PearlColor;

				float4 FragmentProgram(GenericFragmentInput input) : SV_TARGET{
					float4 color = tex2D(_PearlTex, input.uv) * _PearlColor * (1-abs(input.color.r-0.5)*2) * (0.2+input.color.g);
					//if (color.a < 0.5) discard;
					return saturate(color);
				}

				ENDHLSL
			}
    }
}
