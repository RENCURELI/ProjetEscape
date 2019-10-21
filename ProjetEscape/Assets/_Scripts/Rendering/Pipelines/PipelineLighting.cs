using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.RenderPipelines
{
    public static class PipelineLighting
    {
        public static List<LightSource> allSources = new List<LightSource>();
        public static bool enabled = true;
        public static bool shadowRender = false;

        private static Vector4[] positions = new Vector4[8];
        private static Vector4[] colors = new Vector4[8];
        private static float[] intensities = new float[8];
        private static float[] distances = new float[8];
        private static Vector4[] dirs = new Vector4[8];
        private static Vector4[] infos = new Vector4[8];
        private static Matrix4x4[] projections = new Matrix4x4[8];
        private static RenderTexture[] cookies = new RenderTexture[8];

        public static RenderTexture defaultShadowMap;

        public static void SetupShaderVariables()
        {
            if (defaultShadowMap == null)
            {
                defaultShadowMap = new RenderTexture(2, 2, 24, RenderTextureFormat.Depth);
                defaultShadowMap.Create();
            }

            if (!enabled)
            {
                Shader.SetGlobalInt("LightCount", 0);
                return;
            }

            Shader.SetGlobalInt("LightCount", allSources.Count);
            
            for (int i = 0; i < 8; i++)
            {
                if (i >= allSources.Count)
                {
                    positions[i] = Vector4.zero;
                    colors[i] = Vector4.zero;
                    intensities[i] = 0;
                    distances[i] = 0;
                    projections[i] = Matrix4x4.identity;
                    cookies[i] = defaultShadowMap;
                    continue;
                };
                positions[i] = allSources[i].Position;
                colors[i] = allSources[i].ColorVector;
                intensities[i] = allSources[i].Intensity;
                distances[i] = allSources[i].radius;
                dirs[i] = allSources[i].Dir;
                infos[i] = allSources[i].Info;
                projections[i] = allSources[i].Projection;
                Shader.SetGlobalTexture("LightTexture_" + i, allSources[i].StaticShadowMap);
            }
            Shader.SetGlobalVectorArray("LightPosition", positions);
            Shader.SetGlobalVectorArray("LightColor", colors);
            Shader.SetGlobalFloatArray("LightIntensity", intensities);
            Shader.SetGlobalFloatArray("LightDistance", distances);
            Shader.SetGlobalVectorArray("LightDir", dirs);
            Shader.SetGlobalVectorArray("LightInfo", infos);
            Shader.SetGlobalMatrixArray("LightProjection", projections);
        }

    }
}