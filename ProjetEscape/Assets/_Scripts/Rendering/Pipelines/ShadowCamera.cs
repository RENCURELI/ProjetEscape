using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.RenderPipelines
{
    [System.Serializable]
    public class ShadowRenderer
    {
        public int renderWidth = 1024;
        public int renderHeight = 1024;
        public bool renderStatic = true;
        public bool renderDynamic = false;
        public RenderTexture staticShadowMap;
        public RenderTexture dynamicShadowMap;

        private bool initialized
        {
            get
            {
                return camera != null;
            }
        }

        private Camera camera;

        public LightSource source;

        public static Matrix4x4 currentVP;

        public Matrix4x4 Projection
        {
            get
            {
                Initialize();
                return camera.projectionMatrix * camera.worldToCameraMatrix;
            }
        }

        /// <summary>
        /// Call this in every public function to make sure rendering happens.
        /// </summary>
        private void Initialize()
        {
            if (initialized) return;

            // setup camera
            if (camera != null) Object.Destroy(camera.gameObject);
            camera = new GameObject("Shadow Camera").AddComponent<Camera>();
            camera.enabled = false;
            camera.transform.position = Vector3.up * 20;
            camera.transform.forward = Vector3.down;
            camera.orthographic = true;
            camera.orthographicSize = 5;

            if (renderStatic)
            {
                staticShadowMap = new RenderTexture(renderWidth, renderHeight, 24, RenderTextureFormat.RFloat);
                staticShadowMap.Create();
            }
            // same for dynamic shadow map

            //initialized = true;
        }

        public void RenderStatic()
        {
            Initialize();
            if (!renderStatic) return;
            currentVP = source.Projection;
            camera.transform.parent = source.transform;
            camera.orthographicSize = source.radius;
            camera.transform.localPosition = Vector3.zero;
            camera.transform.localRotation = Quaternion.identity;
            OldConsolePipeline.isRenderingShadows = true;
            camera.targetTexture = staticShadowMap;
            camera.Render();
            OldConsolePipeline.isRenderingShadows = false;
        }
    }
}