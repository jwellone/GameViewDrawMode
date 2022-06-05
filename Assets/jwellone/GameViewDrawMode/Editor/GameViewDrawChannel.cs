using UnityEngine;

#nullable enable

namespace jwelloneEditor
{
	public class GameViewDrawChannel : IGameViewDraw
	{
		public enum eMode
		{
			R,
			G,
			B,
			A,
			MAX
		}

		private eMode _mode;
		private Material? _material;

		public GameViewDrawChannel(eMode mode)
		{
			_mode = mode;
		}

		public void Draw(Camera camera)
		{
			if (_material == null)
			{
				_material = new Material(Shader.Find("jwellone/Editor/GameView/DrawChannel"));
			}

			var gameViewCamera = camera.gameObject.GetComponent<GameViewCamera>();
			if (gameViewCamera == null)
			{
				gameViewCamera = camera.gameObject.AddComponent<GameViewCamera>();
				gameViewCamera.Mode = _mode;
				gameViewCamera.material = _material;
			}
		}

		public void Reset(Camera camera)
		{
		}

		public void Release()
		{
			if (_material != null)
			{
				GameObject.DestroyImmediate(_material);
				_material = null;
			}
		}

		[RequireComponent(typeof(Camera))]
		protected class GameViewCamera : MonoBehaviour
		{
			private static eMode _currentMode = eMode.MAX;
			private CameraBackupParameters _backupParameters;

			public eMode Mode
			{
				get;
				set;
			}

			public Material? material
			{
				get;
				set;
			}

			private void OnEnable()
			{
				var camera = gameObject.GetComponent<Camera>();
				_backupParameters.Backup(camera);
				camera.backgroundColor = Color.clear;
				if (camera.tag == "MainCamera")
				{
					camera.clearFlags = CameraClearFlags.SolidColor;
				}
			}

			private void OnDisable()
			{
				var camera = gameObject.GetComponent<Camera>();
				_backupParameters.Apply(camera);

				material = null;
			}

			private void OnRenderImage(RenderTexture source, RenderTexture dest)
			{
				UpdateKeyword();
				Graphics.Blit(source, dest, material);
				DestroyImmediate(this);
			}

			private void UpdateKeyword()
			{
				if (Mode == _currentMode)
				{
					return;
				}

				_currentMode = Mode;

				for (var i = 0; i < (int)eMode.MAX; ++i)
				{
					Shader.DisableKeyword("_CHANNEL_" + ((eMode)i).ToString());
				}

				Shader.EnableKeyword("_CHANNEL_" + Mode.ToString());
			}
		}
	}
}