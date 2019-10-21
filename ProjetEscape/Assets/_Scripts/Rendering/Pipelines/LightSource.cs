using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.RenderPipelines
{
    [ExecuteInEditMode]
    public class LightSource : MonoBehaviour
    {
        public Color color;
        public float intensity;
        public float radius;
        public bool startDimmed = false;

        public enum SourceType { POINT, AMBIENT, SUN };

        public SourceType sourceType = SourceType.POINT;
        public Texture2D cookie;

        private float dimFactor = 1;
        private const float dimSpeed = 1.0f/3.0f;

        public bool castShadow = false;
        [Range(0f,1f)] public float bias = 0.01f;
        private ShadowRenderer shadowRenderer;

        public Vector4 Position
        {
            get
            {
                return transform.position;
            }
        }
        public Vector4 ColorVector
        {
            get
            {
                return new Vector4(color.r, color.g, color.b);
            }
        }
        public float Intensity
        {
            get
            {
                return intensity * dimFactor;
            }
        }
        public Vector4 Dir
        {
            get
            {
                return transform.forward;
            }
        }
        public Vector4 Info
        {
            get
            {
                return new Vector3((int)sourceType, bias, 0);
            }
        }
        public Matrix4x4 Projection
        {
            get
            {
                if (shadowRenderer != null)
                {
                    return shadowRenderer.Projection;
                }
                return Matrix4x4.Rotate(transform.rotation).inverse * Matrix4x4.Scale(Vector3.one * radius).inverse * Matrix4x4.Translate(transform.position).inverse;
            }
        }
        public RenderTexture StaticShadowMap
        {
            get
            {
                if (castShadow)
                {
                    if (shadowRenderer != null)
                    {
                        if (shadowRenderer.staticShadowMap != null)
                            return shadowRenderer.staticShadowMap;
                    }
                }
                return PipelineLighting.defaultShadowMap;
            }
        }

        private void InitializeShadowCamera()
        {
            shadowRenderer = new ShadowRenderer() { source = this };
        }

        private void Update()
        {
            if (castShadow)
            {
                RenderShadow();
            }
            if (sourceType == SourceType.SUN)
            {
                Vector3 targetPosition = Camera.main.transform.position + Camera.main.transform.forward * radius - transform.forward * radius * 2;
                transform.position = targetPosition;
            }
        }

        private void OnEnable()
        {
            PipelineLighting.allSources.Add(this);
        }

        private void Start()
        {
            if (startDimmed) dimFactor = 0;
        }

        private void OnDisable()
        {
            PipelineLighting.allSources.Remove(this);
        }

        public void Dim()
        {
            CancelInvoke("Light");
            if (dimFactor > 0)
            {
                dimFactor -= Time.deltaTime * dimSpeed;
                Invoke("Dim", 0);
            }
        }

        public void Light()
        {
            CancelInvoke("Dim");
            if (dimFactor < 1)
            {
                dimFactor += Time.deltaTime * dimSpeed;
                Invoke("Light", 0);
            }
        }

        public void RenderShadow()
        {
            if (shadowRenderer == null) InitializeShadowCamera();
            shadowRenderer.source = this;
            shadowRenderer.RenderStatic();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(color.r, color.g, color.b, 1);
            if (sourceType == SourceType.POINT)
            {
                Gizmos.DrawWireSphere(transform.position, radius);
            }
            else if (sourceType == SourceType.SUN)
            {
                Gizmos.DrawLine(Position, Position + Dir * 5);
            }

            Gizmos.color = Color.white;
        }
    }
}