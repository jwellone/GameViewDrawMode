using UnityEngine;

#nullable enable

namespace jwelloneEditor
{
	public class GameViewDrawShadowMap : IGameViewDraw
	{
		private Material? _material;

		public void Draw(Camera camera)
		{
			if (_material == null)
			{
				_material = new Material(Shader.Find("jwellone/Editor/GameView/DrawShadowMap"));
			}

			var gameViewCamera = camera.gameObject.GetComponent<GameViewCamera>();
			if (gameViewCamera == null)
			{
				var view = camera.gameObject.AddComponent<GameViewCamera>();
				view.material = _material;
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
		public class GameViewCamera : MonoBehaviour
		{
			private DepthTextureMode _depthTextureMode;

			public Material? material
			{
				get;
				set;
			}

			private void OnEnable()
			{
				var camera = GetComponent<Camera>();
				if (camera == null)
				{
					return;
				}

				_depthTextureMode = camera.depthTextureMode;
				camera.depthTextureMode |= DepthTextureMode.DepthNormals;
			}

			private void OnDisable()
			{
				var camera = GetComponent<Camera>();
				if (camera != null)
				{
					camera.depthTextureMode = _depthTextureMode;
				}

				material = null;
			}

			private void OnRenderImage(RenderTexture source, RenderTexture dest)
			{
				Graphics.Blit(source, dest, material);
				DestroyImmediate(this);
			}
		}
	}
}