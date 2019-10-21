#if !defined(POST_PROCESS_LIB)
#define POST_PROCESS_LIB

#include "Generic.hlsl"

// Types

struct PostProcessVertexInput {
	float4 position : POSITION;
};

struct PostProcessFragmentInput {
	float4 position : POSITION;
	float2 uv : TEXCOORD0;
};

// Variables

float4 _ProjectionParams;
float4 _ZBufferParams;

TEXTURE2D(_CameraColorTexture);
SAMPLER(sampler_CameraColorTexture);
TEXTURE2D(_CameraDepthTexture);
SAMPLER(sampler_CameraDepthTexture);

// Functions

PostProcessFragmentInput GetPostProcessFragmentInput(PostProcessVertexInput input) {
	PostProcessFragmentInput output;
	output.position = float4(input.position.xy, 0.0, 1.0);
	output.uv = input.position.xy * 0.5 + 0.5;
	if (_ProjectionParams.x < 0) {
		output.uv.y = 1 - output.uv.y;
	}
	return output;
}

float4 GetCurrentPassColor(PostProcessFragmentInput input) : SV_TARGET{
	return SAMPLE_TEXTURE2D(
		_CameraColorTexture, sampler_CameraColorTexture, input.uv
	);
}

float4 GetCurrentPassDepth(PostProcessFragmentInput input) : SV_TARGET{
	float rawDepth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, input.uv);
	float linearDepth = LinearEyeDepth(rawDepth, _ZBufferParams);
	return float4(linearDepth / _ZBufferParams.z, rawDepth, 0, 0); // pow(sin(linearDepth*3.14), 2);
}

#endif