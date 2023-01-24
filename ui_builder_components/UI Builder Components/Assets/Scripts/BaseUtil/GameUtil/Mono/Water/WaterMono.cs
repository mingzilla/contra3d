using UnityEngine;

namespace BaseUtil.GameUtil.Mono.Water
{
    /// <summary>
    /// As long as this is added to a plain, which has a MeshFilter by default, then it just works.
    /// Consider adding a transparent mesh to the plain, so that the water looks transparent.
    /// Be aware it has performance impact.
    /// Originally from https://github.com/codewithkyle/Unity-Tutorials/blob/master/low-poly-water/Assets/Scripts/Water.cs
    /// </summary>
    [RequireComponent(typeof(MeshFilter))]
    public class WaterMono : MonoBehaviour
    {
        [SerializeField] float waveFrequency = 0.53f;
        [SerializeField] float waveHeight = 0.48f;
        [SerializeField] float waveLength = 0.71f;

        private readonly Vector3 waveSource = new Vector3(2.0f, 0.0f, 2.0f);
        private Vector3[] verts;
        private MeshFilter mf;
        private Mesh mesh;

        void Start()
        {
            mf = MakeMeshLowPoly(GetComponent<MeshFilter>());
        }

        MeshFilter MakeMeshLowPoly(MeshFilter mf)
        {
            mesh = mf.sharedMesh; 
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
        }

        void CalcWave()
        {
            for (int i = 0; i < verts.Length; i++)
            {
                Vector3 v = verts[i];
                v.y = 0.0f;
                float dist = Vector3.Distance(v, waveSource);
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
    }
}