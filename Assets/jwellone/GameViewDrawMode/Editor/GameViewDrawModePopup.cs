using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#nullable enable

namespace jwelloneEditor
{
	[Serializable]
	public class GameViewDrawModePopup : Toolbar.ToolbarUI
	{
		public enum Mode
		{
			Default = 0,
			Overdraw,
			Mips,
			TextureStreming,
			Wireframe,
			Depth,
			Normals,
			ShadowMap,
			RChannel,
			GChannel,
			BChannel,
			AlphaChannel,
			MeshUV0,
			MeshUV1,
			MeshVertexColor,
			MeshNormals,
			MeshTangents,
			MeshBiTangents,
		}

		[SerializeField] Mode _mode;

		public override string name => "GameViewDrawMode";
		readonly string[] _modeLabels;

		public GameViewDrawModePopup() : base(Area.Right)
		{
			var modeArray = Enum.GetValues(typeof(Mode));
			_modeLabels = new string[(int)modeArray.Length];
			for (var i = 0; i < modeArray.Length; ++i)
			{
				_modeLabels[i] = modeArray.GetValue(i).ToString();
			}
		}

		public override void OnGUI()
		{
			_mode = (Mode)EditorGUILayout.Popup((int)_mode, _modeLabels, new[] { GUILayout.Width(128) });
			Renderer.selectMode = _mode;
		}

		[InitializeOnLoad]
		static class Renderer
		{
			static readonly List<IGameViewDraw> _drawList;
			static readonly List<int> _targetInstanceIDs = new List<int>(16);

			static IGameViewDraw current => _drawList[(int)selectMode];

			static public Mode selectMode;

			static Renderer()
			{
				EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
				EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

				_drawList = new List<IGameViewDraw>(new IGameViewDraw[]
				{
					new GameViewDrawNothing(),
					new GameViewDrawOverdraw(),
					new GameViewDrawMips(),
					new GameViewDrawTextureStreaming(),
					new GameViewDrawWireframe(),
					new GameViewDrawDepth(),
					new GameViewDrawNormals(),
					new GameViewDrawShadowMap(),
					new GameViewDrawChannel( GameViewDrawChannel.eMode.R ),
					new GameViewDrawChannel( GameViewDrawChannel.eMode.G ),
					new GameViewDrawChannel( GameViewDrawChannel.eMode.B ),
					new GameViewDrawChannel( GameViewDrawChannel.eMode.A ),
					new GameViewDrawMeshInfo( GameViewDrawMeshInfo.Mode.UV0 ),
					new GameViewDrawMeshInfo( GameViewDrawMeshInfo.Mode.UV1 ),
					new GameViewDrawMeshInfo( GameViewDrawMeshInfo.Mode.VERTEXCOLOR ),
					new GameViewDrawMeshInfo( GameViewDrawMeshInfo.Mode.NORMALS ),
					new GameViewDrawMeshInfo( GameViewDrawMeshInfo.Mode.TANGENTS ),
					new GameViewDrawMeshInfo( GameViewDrawMeshInfo.Mode.BITANGENTS )
				});
			}

			static void OnPlayModeStateChanged(PlayModeStateChange state)
			{
				switch (state)
				{
					case PlayModeStateChange.EnteredPlayMode:
						{
							Camera.onPreCull += OnPreCull;
							Camera.onPostRender += OnPostRender;
						}
						break;

					case PlayModeStateChange.ExitingPlayMode:
						{
							Camera.onPreCull -= OnPreCull;
							Camera.onPostRender -= OnPostRender;

							foreach (var draw in _drawList)
							{
								draw.Release();
							}

							foreach (var camera in Camera.allCameras)
							{
								camera.ResetReplacementShader();
							}

							_targetInstanceIDs.Clear();
						}
						break;
				}
			}

			static void OnPreCull(Camera camera)
			{
				if (_targetInstanceIDs.Contains(camera.GetInstanceID()))
				{
					return;
				}

				_targetInstanceIDs.Add(camera.GetInstanceID());
				current.Draw(camera);
			}

			static void OnPostRender(Camera camera)
			{
				if (_targetInstanceIDs.Contains(camera.GetInstanceID()))
				{
					current.Reset(camera);
					_targetInstanceIDs.Remove(camera.GetInstanceID());
				}
			}
		}
	}
}