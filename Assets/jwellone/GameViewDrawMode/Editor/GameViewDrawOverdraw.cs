using UnityEngine;
using UnityEditor;

#nullable enable

namespace jwelloneEditor
{
	public class GameViewDrawOverdraw : GameViewDraw
	{
		private Shader? _shader;

		protected override bool canMainCameraClearFlagsSolid => true;

		public override void Draw(Camera camera)
		{
			base.Draw(camera);

			if (_shader == null)
			{
				_shader = EditorGUIUtility.LoadRequired("SceneView/SceneViewShowOverdraw.shader") as Shader;
			}

			camera.SetReplacementShader(_shader, "RenderType");
		}

		public override void Release()
		{
			_shader = null;
		}
	}
}