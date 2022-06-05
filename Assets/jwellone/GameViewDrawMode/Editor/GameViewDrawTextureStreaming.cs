using UnityEngine;
using UnityEditor;

#nullable enable

namespace jwelloneEditor
{
	public class GameViewDrawTextureStreaming : GameViewDraw
	{
		private Shader? _shader;

		public override void Draw(Camera camera)
		{
			base.Draw(camera);

			if (_shader == null)
			{
				_shader = EditorGUIUtility.LoadRequired("SceneView/SceneViewShowTextureStreaming.shader") as Shader;
			}

			if (!_shader?.isSupported ?? false)
			{
				return;
			}

			Texture.SetStreamingTextureMaterialDebugProperties();

			camera.SetReplacementShader(_shader, "RenderType");
		}

		public override void Release()
		{
			_shader = null;
		}
	}
}