using UnityEngine;

#nullable enable

namespace jwelloneEditor
{
	public class GameViewDrawNormals : GameViewDraw
	{
		private Shader? _shader = null;

		public override void Draw(Camera camera)
		{
			base.Draw(camera);

			if (_shader == null)
			{
				_shader = Shader.Find("jwellone/Editor/GameView/DrawNormals");
			}

			camera.SetReplacementShader(_shader, null);
		}

		public override void Release()
		{
			_shader = null;
		}
	}
}