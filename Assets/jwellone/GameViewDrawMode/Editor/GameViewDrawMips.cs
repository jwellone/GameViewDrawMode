using UnityEngine;
using UnityEditor;

#nullable enable

namespace jwelloneEditor
{
	public class GameViewDrawMips : GameViewDraw
	{
		private Shader? _shader;
		private Texture2D? _texture;

		public override void Draw(Camera camera)
		{
			base.Draw(camera);

			if (_shader == null)
			{
				_shader = EditorGUIUtility.LoadRequired("SceneView/SceneViewShowMips.shader") as Shader;
			}

			camera.SetReplacementShader(_shader, "RenderType");

			if (_texture != null)
			{
				return;
			}

			_texture = new Texture2D(32, 32, TextureFormat.RGBA32, true);

			var colorTables = new Color[]
			{
				new Color( 0.0f, 0.0f, 1.0f, 0.8f ),
				new Color( 0.0f, 0.5f, 1.0f, 0.4f ),
				new Color( 1.0f, 1.0f, 1.0f, 0.0f ),
				new Color( 1.0f, 0.7f, 0.0f, 0.2f ),
				new Color( 1.0f, 0.3f, 0.0f, 0.6f ),
				new Color( 1.0f, 0.0f, 0.0f, 0.8f )
			};

			var count = Mathf.Min(colorTables.Length, _texture.mipmapCount);
			for (var mipLevel = 0; mipLevel < count; ++mipLevel)
			{
				var w = Mathf.Max(_texture.width >> mipLevel, 1);
				var h = Mathf.Max(_texture.height >> mipLevel, 1);
				var colors = new Color[w * h];

				for (var i = 0; i < colors.Length; ++i)
				{
					colors[i] = colorTables[mipLevel];
				}

				_texture.SetPixels(colors, mipLevel);
			}

			_texture.filterMode = FilterMode.Trilinear;
			_texture.Apply(false);

			Shader.SetGlobalTexture("_SceneViewMipcolorsTexture", _texture);
		}

		public override void Release()
		{
			_shader = null;
			if (_texture != null)
			{
				Texture2D.Destroy(_texture);
				_texture = null;
			}
		}
	}
}