using UnityEngine;


namespace VoxelPlay
{

    public partial class VoxelPlayEnvironment : MonoBehaviour
    {

        FastList<LightmapRemovalNode> torchLightmapRemovalQueue;


        void ClearTorchLightmap (VoxelChunk chunk, int voxelIndex)
        {
            int light = chunk.voxels [voxelIndex].torchLight;
            if (light == 0) return;

            chunk.voxels [voxelIndex].torchLight = 0;

            ChunkRequestRefresh (chunk, false, true);
            RebuildNeighboursIfNeeded (chunk, voxelIndex);

            torchLightmapRemovalQueue.Add (new LightmapRemovalNode { chunk = chunk, voxelIndex = voxelIndex, light = light });
        }

        void RemoveTorchLightFromNeighbourVoxel (VoxelChunk nchunk, int nindex, int light, int decrement)
        {
            if ((object)nchunk == null) return;

            int nlight = nchunk.voxels [nindex].torchLight;
            if (nlight <= 0) return;

            light -= decrement + nchunk.voxels [nindex].opaque;

            if (nlight <= light) {
                nchunk.voxels [nindex].torchLight = 0;
                ChunkRequestRefresh (nchunk, false, true);
                RebuildNeighboursIfNeeded (nchunk, nindex);
                torchLightmapRemovalQueue.Add (new LightmapRemovalNode { chunk = nchunk, voxelIndex = nindex, light = nlight });
            } else {
                torchLightmapSpreadQueue.Add (new LightmapAddNode { chunk = nchunk, voxelIndex = nindex });
            }
        }

        void ProcessTorchLightmapRemoval ()
        {

            int lightAtten = world.lightTorchAttenuation;

            for (int k = 0; k < torchLightmapRemovalQueue.count; k++) {
                VoxelChunk chunk = torchLightmapRemovalQueue.values [k].chunk;
                int voxelIndex = torchLightmapRemovalQueue.values [k].voxelIndex;
                int light = torchLightmapRemovalQueue.values [k].light;

                // Spread on neighbours
                int px, py, pz;
                VoxelChunk nchunk;
                int nindex;
                GetVoxelChunkCoordinates (voxelIndex, out px, out py, out pz);

                // bottom voxel
                if (py > 0) {
                    nchunk = chunk; nindex = voxelIndex - ONE_Y_ROW;
                } else {
                    nchunk = chunk.bottom; nindex = voxelIndex + ONE_Y_ROW * CHUNK_SIZE_MINUS_ONE;
                }
                RemoveTorchLightFromNeighbourVoxel (nchunk, nindex, light, lightAtten);

                // left voxel
                if (px > 0) {
                    nchunk = chunk; nindex = voxelIndex - 1;
                } else {
                    nchunk = chunk.left; nindex = voxelIndex + CHUNK_SIZE_MINUS_ONE;
                }
                RemoveTorchLightFromNeighbourVoxel (nchunk, nindex, light, lightAtten);

                // right voxel
                if (px < CHUNK_SIZE_MINUS_ONE) {
                    nchunk = chunk; nindex = voxelIndex + 1;
                } else {
                    nchunk = chunk.right; nindex = voxelIndex - CHUNK_SIZE_MINUS_ONE;
                }
                RemoveTorchLightFromNeighbourVoxel (nchunk, nindex, light, lightAtten);

                // back voxel
                if (pz > 0) {
                    nchunk = chunk; nindex = voxelIndex - ONE_Z_ROW;
                } else {
                    nchunk = chunk.back; nindex = voxelIndex + ONE_Z_ROW * CHUNK_SIZE_MINUS_ONE;
                }
                RemoveTorchLightFromNeighbourVoxel (nchunk, nindex, light, lightAtten);

                // forward voxel
                if (pz < CHUNK_SIZE_MINUS_ONE) {
                    nchunk = chunk; nindex = voxelIndex + ONE_Z_ROW;
                } else {
                    nchunk = chunk.forward; nindex = voxelIndex - ONE_Z_ROW * CHUNK_SIZE_MINUS_ONE;
                }
                RemoveTorchLightFromNeighbourVoxel (nchunk, nindex, light, lightAtten);

                // top voxel
                if (py < CHUNK_SIZE_MINUS_ONE) {
                    nchunk = chunk; nindex = voxelIndex + ONE_Y_ROW;
                } else {
                    nchunk = chunk.top; nindex = voxelIndex - ONE_Y_ROW * CHUNK_SIZE_MINUS_ONE;
                }
                RemoveTorchLightFromNeighbourVoxel (nchunk, nindex, light, lightAtten);
            }
            torchLightmapRemovalQueue.Clear ();
        }



    }



}
