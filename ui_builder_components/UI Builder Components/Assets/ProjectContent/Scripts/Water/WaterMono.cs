using UnityEngine;

namespace ProjectContent.Scripts.Water
{
    [RequireComponent(typeof(MeshFilter))]
    public class WaterMono : MonoBehaviour
    {
        private Vector3 waveSource1 = new Vector3(2.0f, 0.0f, 2.0f);
        [SerializeField] float waveFrequency = 0.53f;
        [SerializeField] float waveHeight = 0.48f;
        [SerializeField] float waveLength = 0.71f;
        [SerializeField] bool edgeBlend = true;
        Vector3[] verts;

        private MeshFilter mf;
        private Mesh mesh;

        void Start()
        {
            if (UnityEngine.Camera.main != null) UnityEngine.Camera.main.depthTextureMode |= DepthTextureMode.Depth;
            mf = MakeMeshLowPoly(GetComponent<MeshFilter>());
        }

        MeshFilter MakeMeshLowPoly(MeshFilter mf)
        {
            mesh = mf.sharedMesh; //Change to sharedmesh? 
            Vector3[] oldVerts = mesh.vertices;
            int[] triangles = mesh.triangles;
            Vector3[] vertices = new Vector3[triangles.Length];
            for (int i = 0; i < triangles.Length; i++)
            {
                vertices[i] = oldVerts[triangles[i]];
                triangles[i] = i;
            }
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            verts = mesh.vertices;
            return mf;
        }

        void Update()
        {
            CalcWave();
            SetEdgeBlend();
        }

        void CalcWave()
        {
            for (int i = 0; i < verts.Length; i++)
            {
                Vector3 v = verts[i];
                v.y = 0.0f;
                float dist = Vector3.Distance(v, waveSource1);
                dist = (dist % waveLength) / waveLength;
                v.y = waveHeight * Mathf.Sin(Time.time * Mathf.PI * 2.0f * waveFrequency
                                             + (Mathf.PI * 2.0f * dist));
                verts[i] = v;
            }
            mesh.vertices = verts;
            mesh.RecalculateNormals();
            mesh.MarkDynamic();

            mf.mesh = mesh;
        }

        void SetEdgeBlend()
        {
            if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
            {
                edgeBlend = false;
            }
            if (edgeBlend)
            {
                Shader.EnableKeyword("WATER_EDGEBLEND_ON");
                if (UnityEngine.Camera.main)
                {
                    UnityEngine.Camera.main.depthTextureMode |= DepthTextureMode.Depth;
                }
            }
            else
            {
                Shader.DisableKeyword("WATER_EDGEBLEND_ON");
            }
        }
    }
}