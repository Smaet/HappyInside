using System;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelPlay {

	public class VoxelPlaceholder : MonoBehaviour {

		[NonSerialized]
		public int resistancePointsLeft;

		[NonSerialized]
		public Renderer damageIndicator;

		[NonSerialized]
		public VoxelChunk chunk;

		[NonSerialized]
		public int voxelIndex;

		[NonSerialized]
		public GameObject modelTemplate;

		[NonSerialized]
		public GameObject modelInstance;

		[NonSerialized]
		public Bounds bounds;

		[NonSerialized]
		public MeshFilter modelMeshFilter;

        [NonSerialized]
        public Color32[] originalMeshColors32;

        public MeshRenderer modelMeshRenderer {
            get {
                if (modelMeshRenderers == null) return null;
                return modelMeshRenderers[0];
            }

        }

        [NonSerialized]
        public MeshRenderer[] modelMeshRenderers;

		[NonSerialized]
		public Rigidbody rb;

        [NonSerialized]
        public Color32 lastMivTintColor = new Color32 (255, 255, 255, 15); // last computed tint color when rendered a miv in this position

		public Material damageIndicatorMaterial {
			get {
				if (_damageIndicatorMaterial == null && damageIndicator != null) {
					_damageIndicatorMaterial = Instantiate<Material> (damageIndicator.sharedMaterial);
					damageIndicator.sharedMaterial = _damageIndicatorMaterial;
				}
				return _damageIndicatorMaterial;
			}
		}


		float recoveryTime;
		Material _damageIndicatorMaterial;


        private void OnDestroy()
        {
			CancelInvoke(nameof(Recover));
			StopAllCoroutines();
        }

        public void StartHealthRecovery (VoxelChunk chunk, int voxelIndex, float damageDuration) {
			this.chunk = chunk;
			this.voxelIndex = voxelIndex;
			recoveryTime = Time.time + damageDuration;
			CancelInvoke (nameof(Recover));
			Invoke (nameof(Recover), damageDuration + 0.1f);
		}

		void Recover () {
			float time = Time.time;
			if (time >= recoveryTime) {
				if (chunk != null && chunk.voxels [voxelIndex].typeIndex != 0) {
					resistancePointsLeft = chunk.voxels [voxelIndex].type.resistancePoints;
				}
				if (damageIndicator != null) {
					damageIndicator.enabled = false;
				}
			}
		}


		public void SetCancelDynamic (float delay) {
			Invoke (nameof(CancelDynamic), delay + UnityEngine.Random.value);
		}

		public void CancelDynamic () {
			if (this != null && isActiveAndEnabled) {
				StartCoroutine (Consolidate ());
			}
		}

		IEnumerator Consolidate () {
			WaitForSeconds w = new WaitForSeconds (1f);
			VoxelChunk targetChunk;
			VoxelPlayEnvironment env = VoxelPlayEnvironment.instance;
			if (env == null || gameObject.Equals(null))
				yield break;
			if (env.GetChunk (transform.position, out targetChunk, false)) {
				const float maxDist = 100*100;
				if (env == null || gameObject.Equals(null) || env.cameraMain == null)
					yield break;
				while (FastVector.SqrDistanceByValue ((Vector3)targetChunk.position, env.cameraMain.transform.position) < maxDist && env.ChunkIsInFrustum(targetChunk)) {
					yield return w;
				}
				env.VoxelCancelDynamic (this);
			}
		}

        [MethodImpl(256)] // equals to MethodImplOptions.AggressiveInlining
        public void ToggleRenderers(bool enabled) {
            if (modelMeshRenderers == null) return;
            for (int j = 0; j < modelMeshRenderers.Length; j++) {
                if (modelMeshRenderers[j] != null) {
                    modelMeshRenderers[j].enabled = enabled;
                }
            }
        }


    }
}