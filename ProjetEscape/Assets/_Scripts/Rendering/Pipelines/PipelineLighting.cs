using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.RenderPipelines
{
    public static class PipelineLighting
    {
        //public static List<LightSource> allSources = new List<LightSource>();
        public static bool enabled = true;

        public const int maxVisibleLights = 16;

        public static int visibleLightColorsId =
            Shader.PropertyToID("_VisibleLightColors");
        public static int visibleLightDirectionsId =
            Shader.PropertyToID("_VisibleLightDirections");
        public static int visibleLightPositionsId =
            Shader.PropertyToID("_VisibleLightPositions");
        public static int visibleLightTypesId =
            Shader.PropertyToID("_VisibleLightTypes");
        public static int lightIndicesOffsetAndCountID =
            Shader.PropertyToID("unity_LightIndicesOffsetAndCount");

        public static Vector4[] visibleLightColors = new Vector4[maxVisibleLights];
        public static Vector4[] visibleLightDirections = new Vector4[maxVisibleLights];
        public static Vector4[] visibleLightPositions = new Vector4[maxVisibleLights];
        public static float[] visibleLightTypes = new float[maxVisibleLights];
    }
}