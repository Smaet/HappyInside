using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

namespace VoxelPlay
{


    public struct VirtualVoxel
    {
        public int chunk9Index;
        public int voxelIndex;
    }

    public struct MeshJobData
    {
        public VoxelChunk chunk;
        public int totalVoxels;

        public List<Vector3> vertices;
        public List<Vector4> uv0;
        public List<Color32> colors;
        public List<Vector3> normals;

        public List<int>[] indexBuffers;
        public int subMeshCount;

        public List<Vector3> colliderVertices;
        public List<int> colliderIndices;
        public bool needsColliderRebuild;

        public List<int> navMeshIndices;
        public List<Vector3> navMeshVertices;

        // models in voxels
        public FastList<int> mivs;
    }

    public abstract class MeshingThread
    {
        protected const byte FULL_OPAQUE = (byte)15;
        const float PHI = 0.3535534f; // 0.5 / sqrt(2)
        public static Vector3 [] faceVerticesForward = {
            new Vector3 (0.5f, -0.5f, 0.5f),
            new Vector3 (0.5f, 0.5f, 0.5f),
            new Vector3 (-0.5f, -0.5f, 0.5f),
            new Vector3 (-0.5f, 0.5f, 0.5f)
        };
        public static Vector3 [] faceVerticesBack ={
            new Vector3 (-0.5f, -0.5f, -0.5f),
            new Vector3 (-0.5f, 0.5f, -0.5f),
            new Vector3 (0.5f, -0.5f, -0.5f),
            new Vector3 (0.5f, 0.5f, -0.5f)
        };
        public static Vector3 [] faceVerticesLeft = {
            new Vector3 (-0.5f, -0.5f, 0.5f),
            new Vector3 (-0.5f, 0.5f, 0.5f),
            new Vector3 (-0.5f, -0.5f, -0.5f),
            new Vector3 (-0.5f, 0.5f, -0.5f)
        };
        public static Vector3 [] faceVerticesRight = {
            new Vector3 (0.5f, -0.5f, -0.5f),
            new Vector3 (0.5f, 0.5f, -0.5f),
            new Vector3 (0.5f, -0.5f, 0.5f),
            new Vector3 (0.5f, 0.5f, 0.5f)
        };
        public static Vector3 [] faceVerticesTop =  {
            new Vector3 (-0.5f, 0.5f, -0.5f),
            new Vector3 (-0.5f, 0.5f, 0.5f),
            new Vector3 (0.5f, 0.5f, -0.5f),
            new Vector3 (0.5f, 0.5f, 0.5f)
        };
        public static Vector3 [] faceVerticesTopFlipped = { // water surface from below
			new Vector3 (-0.5f, 0.5f, -0.5f),
            new Vector3 (0.5f, 0.5f, -0.5f),
            new Vector3 (-0.5f, 0.5f, 0.5f),
            new Vector3 (0.5f, 0.5f, 0.5f)
        };
        public static Vector3 [] faceVerticesBottom = {
            new Vector3 (0.5f, -0.5f, -0.5f),
            new Vector3 (0.5f, -0.5f, 0.5f),
            new Vector3 (-0.5f, -0.5f, -0.5f),
            new Vector3 (-0.5f, -0.5f, 0.5f)
        };
        public static Vector3 [] faceVerticesCross1 =  {
            new Vector3 (-PHI, -0.5f, PHI),
            new Vector3 (-PHI, 0.5f, PHI),
            new Vector3 (PHI, -0.5f, -PHI),
            new Vector3 (PHI, 0.5f, -PHI)
        };
        public static Vector3 [] faceVerticesCross2 =  {
            new Vector3 (-PHI, -0.5f, -PHI),
            new Vector3 (-PHI, 0.5f, -PHI),
            new Vector3 (PHI, -0.5f, PHI),
            new Vector3 (PHI, 0.5f, PHI)
        };

        public static Vector3 [] [] faceVerticesCrossLeaves1;
        public static Vector3 [] [] faceVerticesCrossLeaves2;

        public static Vector3 [] normalsBack = {
            Misc.vector3back, Misc.vector3back, Misc.vector3back, Misc.vector3back
        };
        public static Vector3 [] normalsForward = {
            Misc.vector3forward, Misc.vector3forward, Misc.vector3forward, Misc.vector3forward
        };
        public static Vector3 [] normalsLeft = {
            Misc.vector3left, Misc.vector3left, Misc.vector3left, Misc.vector3left
        };
        public static Vector3 [] normalsRight = {
            Misc.vector3right, Misc.vector3right, Misc.vector3right, Misc.vector3right
        };
        public static Vector3 [] normalsUp = {
            Misc.vector3up, Misc.vector3up, Misc.vector3up, Misc.vector3up
        };
        public static Vector3 [] normalsDown =  {
            Misc.vector3down, Misc.vector3down, Misc.vector3down, Misc.vector3down
        };

        public MeshJobData [] meshJobs;
        public volatile int meshJobMeshLastIndex;
        public volatile int meshJobMeshDataGenerationIndex;
        public volatile int meshJobMeshDataGenerationReadyIndex;
        public volatile int meshJobMeshUploadIndex;

        public int threadId;
        public AutoResetEvent waitEvent;
        public Thread meshGenerationThread;
        public readonly object indicesUpdating = new object ();

        public VoxelPlayGreedyMesher greedyCollider, greedyNavMesh;

        public Voxel [] [] chunk9;
        public VoxelChunk [] neighbourChunks;

        // Voxels
        public List<Vector3> tempChunkVertices;
        public List<Vector4> tempChunkUV0;
        public List<Vector4> tempChunkUV2;
        public List<Color32> tempChunkColors32;
        public List<Vector3> tempChunkNormals;

        // Collider support
        public List<int> meshColliderIndices;
        public List<Vector3> meshColliderVertices;

        // Navmesh support
        public List<int> navMeshIndices;
        public List<Vector3> navMeshVertices;

        public bool allowAO;
        protected bool enableColliders, enableNavMesh, enableTinting;
        protected VirtualVoxel [] virtualChunk;
        protected VoxelPlayEnvironment env;
        protected Color32 [] faceColors;

        public virtual void Init (int threadId, int poolSize, VoxelPlayEnvironment env)
        {

            this.threadId = threadId;
            this.env = env;
            this.enableColliders = env.enableColliders;
            this.enableNavMesh = env.enableNavMesh;
            this.enableTinting = env.enableTinting;
            this.virtualChunk = env.virtualChunk;
            bool lowMemoryMode = env.lowMemoryMode;
            faceColors = new Color32 [4];
            for (int k = 0; k < faceColors.Length; k++) { faceColors [k] = Misc.color32White; }
            InitLeavesFacesRotation ();

#if UNITY_WEBGL
			meshJobs = new MeshJobData[384];
#else
            meshJobs = new MeshJobData [poolSize];
#endif
            meshJobMeshLastIndex = poolSize - 1;
            meshJobMeshDataGenerationIndex = poolSize - 1;
            meshJobMeshDataGenerationReadyIndex = poolSize - 1;
            meshJobMeshUploadIndex = poolSize - 1;
            int initialCapacity = 15000;
            for (int k = 0; k < meshJobs.Length; k++) {
                meshJobs [k].vertices = Misc.GetList<Vector3> (lowMemoryMode, initialCapacity);
                meshJobs [k].uv0 = Misc.GetList<Vector4> (lowMemoryMode, initialCapacity);
                meshJobs [k].colors = Misc.GetList<Color32> (lowMemoryMode, enableTinting ? initialCapacity : 4);
                meshJobs [k].normals = Misc.GetList<Vector3> (lowMemoryMode, initialCapacity);
                meshJobs [k].indexBuffers = new List<int>[VoxelPlayEnvironment.MAX_MATERIALS_PER_CHUNK];
                for (int j = 0; j < meshJobs [k].indexBuffers.Length; j++) {
                    meshJobs [k].indexBuffers [j] = new List<int> ();
                }
                if (enableColliders) {
                    meshJobs [k].colliderVertices = Misc.GetList<Vector3> (lowMemoryMode, 2700);
                    meshJobs [k].colliderIndices = Misc.GetList<int> (lowMemoryMode, 4000);
                }
                if (enableNavMesh) {
                    meshJobs [k].navMeshVertices = Misc.GetList<Vector3> (lowMemoryMode, 2700);
                    meshJobs [k].navMeshIndices = Misc.GetList<int> (lowMemoryMode, 4000);
                }
                meshJobs [k].mivs = new FastList<int> ();
            }

            greedyCollider = new VoxelPlayGreedyMesher ();
            if (enableNavMesh) {
                greedyNavMesh = new VoxelPlayGreedyMesher ();
            }
            chunk9 = new Voxel [27] [];
            neighbourChunks = new VoxelChunk [27];

        }


        // Empty mesh jobs
        public void Clear ()
        {
            if (meshJobs != null) {
                for (int k = 0; k < meshJobs.Length; k++) {
                    if (meshJobs [k].vertices != null) {
                        meshJobs [k].vertices.Clear ();
                        meshJobs [k].vertices = null;
                    }
                    if (meshJobs [k].uv0 != null) {
                        meshJobs [k].uv0.Clear ();
                        meshJobs [k].uv0 = null;
                    }
                    if (meshJobs [k].colors != null) {
                        meshJobs [k].colors.Clear ();
                        meshJobs [k].colors = null;
                    }
                    if (meshJobs [k].normals != null) {
                        meshJobs [k].normals.Clear ();
                        meshJobs [k].normals = null;
                    }
                    if (meshJobs [k].indexBuffers != null) {
                        for (int j = 0; j < meshJobs [k].indexBuffers.Length; j++) {
                            if (meshJobs [k].indexBuffers [j] != null) {
                                meshJobs [k].indexBuffers [j].Clear ();
                                meshJobs [k].indexBuffers [j] = null;
                            }
                        }
                    }
                    if (meshJobs [k].colliderVertices != null) {
                        meshJobs [k].colliderVertices.Clear ();
                        meshJobs [k].colliderVertices = null;
                    }
                    if (meshJobs [k].colliderIndices != null) {
                        meshJobs [k].colliderIndices.Clear ();
                        meshJobs [k].colliderIndices = null;
                    }
                    if (meshJobs [k].navMeshVertices != null) {
                        meshJobs [k].navMeshVertices.Clear ();
                        meshJobs [k].navMeshVertices = null;
                    }
                    if (meshJobs [k].navMeshIndices != null) {
                        meshJobs [k].navMeshIndices.Clear ();
                        meshJobs [k].navMeshIndices = null;
                    }
                    if (meshJobs [k].mivs != null) {
                        meshJobs [k].mivs.Clear ();
                        meshJobs [k].mivs = null;
                    }
                }
            }
        }

        public bool CreateChunkMeshJob (VoxelChunk chunk, bool generationThreadsRunning)
        {
            lock (indicesUpdating) {
                int newJobIndex = meshJobMeshLastIndex + 1;
                if (newJobIndex >= meshJobs.Length) {
                    newJobIndex = 0;
                }

                if (newJobIndex == meshJobMeshDataGenerationIndex || newJobIndex == meshJobMeshUploadIndex) {
                    // no more jobs possible atm
                    return false;
                }
                meshJobs [newJobIndex].chunk = chunk;
                meshJobMeshLastIndex = newJobIndex;
                if (generationThreadsRunning)
                    waitEvent.Set ();
            }
            return true;
        }

        public abstract void GenerateMeshData ();

        void InitLeavesFacesRotation ()
        {
            const float DENSE_LEAVES_SIZE_MULTIPLER = 1.65f;

            Vector3 [] facesLeaves1 = new Vector3 [] {
                new Vector3 (-0.5f, -0.5f, 0f) * DENSE_LEAVES_SIZE_MULTIPLER,
            new Vector3 (-0.5f, 0.5f, 0f) * DENSE_LEAVES_SIZE_MULTIPLER,
            new Vector3 (0.5f, -0.5f, 0f) * DENSE_LEAVES_SIZE_MULTIPLER,
            new Vector3 (0.5f, 0.5f, 0f) * DENSE_LEAVES_SIZE_MULTIPLER
            };
            faceVerticesCrossLeaves1 = new Vector3 [16] [];
            for (int k = 0; k < 16; k++) {
                faceVerticesCrossLeaves1 [k] = new Vector3 [4];
                float angle = (k / 16f) * Mathf.PI * 2f;
                for (int j = 0; j < 4; j++) {
                    faceVerticesCrossLeaves1 [k] [j] = RotateVector (facesLeaves1 [j], angle);
                }
            }

            Vector3 [] facesLeaves2 = new Vector3 []  {
                new Vector3 (0f, -0.5f, -0.5f) * DENSE_LEAVES_SIZE_MULTIPLER,
            new Vector3 (0f, 0.5f, -0.5f) * DENSE_LEAVES_SIZE_MULTIPLER,
            new Vector3 (0f, -0.5f, 0.5f) * DENSE_LEAVES_SIZE_MULTIPLER,
            new Vector3 (0f, 0.5f, 0.5f) * DENSE_LEAVES_SIZE_MULTIPLER
            };
            faceVerticesCrossLeaves2 = new Vector3 [16] [];
            for (int k = 0; k < 16; k++) {
                faceVerticesCrossLeaves2 [k] = new Vector3 [4];
                float angle = (k / 16f) * Mathf.PI * 2f;
                for (int j = 0; j < 4; j++) {
                    faceVerticesCrossLeaves2 [k] [j] = RotateVector (facesLeaves2 [j], angle);
                }
            }

        }

        Vector3 RotateVector (Vector3 p, float angle)
        {
            Vector3 q = p;
            q.y = p.y;
            float cos = Mathf.Cos (angle);
            float sin = Mathf.Sin (angle);
            q.x = p.x * cos - p.z * sin;
            q.z = p.z * cos + p.x * sin;
            return q;
        }

    }
}
