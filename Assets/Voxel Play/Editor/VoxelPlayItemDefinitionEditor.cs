using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

namespace VoxelPlay {
				
	[CustomEditor (typeof(ItemDefinition))]
	public class VoxelPlayItemDefinitionEditor : UnityEditor.Editor {

		SerializedProperty title, category, icon;
		SerializedProperty voxelType, model;
		SerializedProperty prefab, useSound, canBePicked, pickMode, pickupSound, lightIntensity;
		SerializedProperty properties;

		Color titleColor;
		static GUIStyle titleLabelStyle;


		void OnEnable () {
			titleColor = EditorGUIUtility.isProSkin ? new Color (0.52f, 0.66f, 0.9f) : new Color (0.12f, 0.16f, 0.4f);

			title = serializedObject.FindProperty ("title");
			category = serializedObject.FindProperty ("category");
			icon = serializedObject.FindProperty ("icon");
			voxelType = serializedObject.FindProperty ("voxelType");
			model = serializedObject.FindProperty ("model");
			prefab = serializedObject.FindProperty ("prefab");
			useSound = serializedObject.FindProperty ("useSound");
            canBePicked = serializedObject.FindProperty ("canBePicked");
            pickMode = serializedObject.FindProperty ("pickMode");
            pickupSound = serializedObject.FindProperty ("pickupSound");
            lightIntensity = serializedObject.FindProperty ("lightIntensity");
            properties =  serializedObject.FindProperty ("properties");
		}


		public override void OnInspectorGUI () {
			serializedObject.UpdateIfRequiredOrScript ();
			if (titleLabelStyle == null) {
				titleLabelStyle = new GUIStyle (EditorStyles.label);
			}
			titleLabelStyle.normal.textColor = titleColor;
			titleLabelStyle.fontStyle = FontStyle.Bold;
			EditorGUIUtility.labelWidth = 130;

			EditorGUILayout.Separator ();
			GUILayout.Label ("Item Properties", titleLabelStyle);
			EditorGUILayout.PropertyField (category);
			switch (category.intValue) {
			case (int)ItemCategory.Torch:
				EditorGUILayout.HelpBox ("A special item that represents a light source. Can be attached on the sides of other voxels.", MessageType.Info);
				break;
			case (int)ItemCategory.Voxel:
				EditorGUILayout.HelpBox ("An item representing a voxel. All voxels referrenced in the world definition are available as items in build mode.", MessageType.Info);
				break;
			case (int)ItemCategory.Model:
				EditorGUILayout.HelpBox ("An item representing a structure (model definition). All model definitions referrenced in the world definition are available as items in build mode.", MessageType.Info);
				break;
			case (int)ItemCategory.General:
				EditorGUILayout.HelpBox ("Any other item like weapons, crafting tools or wereables.", MessageType.Info);
				break;
			}

			EditorGUILayout.PropertyField (title);
			EditorGUILayout.PropertyField (icon);
			EditorGUILayout.PropertyField (useSound);
			if (category.intValue != (int)ItemCategory.Model) {
				EditorGUILayout.PropertyField (canBePicked);
				if (canBePicked.boolValue) {
					EditorGUI.indentLevel++;
					EditorGUILayout.PropertyField (pickMode);
					EditorGUILayout.PropertyField (pickupSound);
					EditorGUI.indentLevel--;
				}
			}

			switch (category.intValue) {
			case (int)ItemCategory.Torch:
				EditorGUILayout.PropertyField (prefab);
                EditorGUILayout.PropertyField (lightIntensity);
                break;
			case (int)ItemCategory.Voxel:
				EditorGUILayout.PropertyField (voxelType);
				break;
			case (int)ItemCategory.Model:
				EditorGUILayout.PropertyField (model);
				break;
			case (int)ItemCategory.General:
				EditorGUILayout.PropertyField (prefab);
				EditorGUILayout.PropertyField (properties, true);
				break;
			}

			serializedObject.ApplyModifiedProperties ();

		}

	}

}
