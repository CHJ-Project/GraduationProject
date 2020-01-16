using UnityEngine;
using System.Collections;

namespace UnityEngine.UI
{
    public class EmptyRaycast : Graphic
    {

        protected EmptyRaycast()
        {
            useLegacyMeshGeneration = false;
        }
		public override void SetMaterialDirty() { return; }
		public override void SetVerticesDirty() { return; }
		protected override void OnPopulateMesh(VertexHelper toFill)
        {
            toFill.Clear();
        }
    }
}