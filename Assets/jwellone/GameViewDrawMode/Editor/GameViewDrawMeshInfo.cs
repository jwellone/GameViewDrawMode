using UnityEngine;

#nullable enable

namespace jwelloneEditor
{
	public class GameViewDrawMeshInfo : GameViewDraw
	{
		public enum Mode
		{
			UV0,
			UV1,
			VERTEXCOLOR,
			NORMALS,
			TANGENTS,
			BITANGENTS,
			MAX
		}

		private static Mode _currentMode = Mode.MAX;
		private Mode _mode;

		public GameViewDrawMeshInfo(Mode mode)
		{
			_mode = mode;
		}

		public override void Draw(Camera camera)
		{
			base.Draw(camera);

			UpdateKeyword();
			camera.SetReplacementShader(Shader.Find("jwellone/Editor/GameView/DrawMeshInfo"), null);
		}

		private void UpdateKeyword()
		{
			if (_mode == _currentMode)
			{
				return;
			}

			_currentMode = _mode;

			for (var i = 0; i < (int)Mode.MAX; ++i)
			{
				Shader.DisableKeyword("_MESHINFO_" + ((Mode)i).ToString());
			}

			Shader.EnableKeyword("_MESHINFO_" + _mode.ToString());
		}
	}
}