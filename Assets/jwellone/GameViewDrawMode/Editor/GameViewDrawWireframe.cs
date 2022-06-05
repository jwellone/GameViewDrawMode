using UnityEngine;

#nullable enable

namespace jwelloneEditor
{
	public class GameViewDrawWireframe : GameViewDraw
	{
		protected override bool canMainCameraClearFlagsSolid => true;

		public override void Draw(Camera camera)
		{
			base.Draw(camera);
			GL.wireframe = true;
		}

		public override void Reset(Camera camera)
		{
			ResetParamater(camera);
			GL.wireframe = false;
		}
	}
}