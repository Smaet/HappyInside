using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VoxelPlay {
				
	public static class TextureTools {


		public static void Scale (Texture2D tex, int width, int height, FilterMode mode = FilterMode.Trilinear) {

            if (tex.width == width && tex.height == height) return;

			RenderTexture currentActiveRT = RenderTexture.active;

			Rect texR = new Rect (0, 0, width, height);

            //We need the source texture in VRAM because we render with it
            tex.filterMode = mode;
            tex.Apply(true);

            //Using RTT for best quality and performance. Thanks, Unity 5
            RenderTexture rtt = RenderTexture.GetTemporary(width, height, 0);

            //Set the RTT in order to render to it
            Graphics.SetRenderTarget(rtt);
            Graphics.Blit(tex, rtt);

            // Update new texture
            tex.Resize (width, height, TextureFormat.ARGB32, false);
			tex.ReadPixels (texR, 0, 0, true);
			tex.Apply (true);

			RenderTexture.active = currentActiveRT;
            RenderTexture.ReleaseTemporary(rtt);
		}

		public static void EnsureTextureReadable (Texture2D tex) {
			#if UNITY_EDITOR
			string path = AssetDatabase.GetAssetPath (tex);
			if (string.IsNullOrEmpty (path))
				return;
			TextureImporter imp = AssetImporter.GetAtPath (path) as TextureImporter;
			if (imp != null && !imp.isReadable) {
				imp.isReadable = true;
				imp.SaveAndReimport ();
			}
			#endif
		}


		public static Texture2D GetSolidTexture (Texture2D tex) {
			if (tex == null)
				return tex;
			EnsureTextureReadable (tex);
			Texture2D tex2 = new Texture2D (tex.width, tex.height, TextureFormat.ARGB32, false);
			tex2.name = tex.name;
			Color32[] colors = tex.GetPixels32 ();
			for (int k = 0; k < colors.Length; k++) {
				colors [k].a = 255;
			}
			tex2.SetPixels32 (colors);
			tex2.Apply ();
			return tex2;
		}

		public static void Smooth (Texture2D tex, float smoothAmount) {
			int w = tex.width;
			int h = tex.height;
			int ws = Mathf.Clamp ((int)(w * (1f - smoothAmount)), 1, w);
			int hs = Mathf.Clamp ((int)(h * (1f - smoothAmount)), 1, h);
			Scale (tex, ws, hs);
			Scale (tex, w, h);
		}
	}

}