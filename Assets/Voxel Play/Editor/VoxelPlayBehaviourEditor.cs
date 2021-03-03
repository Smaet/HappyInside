using UnityEngine;
using System;
using System.Collections;
using UnityEditor;

namespace VoxelPlay {
				
	[CustomEditor (typeof(VoxelPlayBehaviour))]
	public class VoxelPlayBehaviourEditor : UnityEditor.Editor {

		SerializedProperty enableVoxelLight, useVoxelPlayMaterials, forceUnstuck;
		SerializedProperty checkNearChunks, chunkExtents, renderChunks;
		SerializedProperty useOriginShift;

		void OnEnable () {
			enableVoxelLight = serializedObject.FindProperty ("enableVoxelLight");
			useVoxelPlayMaterials = serializedObject.FindProperty("useVoxelPlayMaterials");
			forceUnstuck = serializedObject.FindProperty ("forceUnstuck");
			checkNearChunks = serializedObject.FindProperty ("checkNearChunks");
			chunkExtents = serializedObject.FindProperty ("chunkExtents");
			renderChunks = serializedObject.FindProperty ("renderChunks");
			useOriginShift = serializedObject.FindProperty ("useOriginShift");
		}


		public override void OnInspectorGUI () {
			serializedObject.Update ();
			EditorGUILayout.Separator ();
			EditorGUI.BeginChangeCheck ();
			EditorGUILayout.PropertyField (enableVoxelLight, new GUIContent("Enable Voxel Light", "Enable this property to adjust material lighting based on voxel global illumination"));
			EditorGUILayout.PropertyField(useVoxelPlayMaterials, new GUIContent("Use Voxel Play Materials", "Replace materials of this gameobject by optimized Voxel Play materials."));
			EditorGUILayout.PropertyField (forceUnstuck, new GUIContent("Force Unstuck", "Moves this gameobject to the surface of the terrain if it falls below or crosses a solid voxel"));
			EditorGUILayout.PropertyField (checkNearChunks, new GUIContent("Chunk Area", "Ensures all nearby chunks are generated"));
			if (checkNearChunks.boolValue) {
				EditorGUILayout.PropertyField (chunkExtents, new GUIContent("   Extents", "Distance in chunks around the transform position (1 chunk = 16 world units by default)"));
				EditorGUILayout.PropertyField (renderChunks, new GUIContent("   Render Chunks", "If this option is enabled, chunks within area will also be rendered. If this option is disabled, chunks will only be generated but no mesh/collider/navmesh will be generated."));
			}
			EditorGUILayout.PropertyField (useOriginShift);
			serializedObject.ApplyModifiedProperties ();
			if (EditorGUI.EndChangeCheck ()) {
				VoxelPlayBehaviour b = (VoxelPlayBehaviour)target;
				b.Refresh ();
			}
		}
	}

}
