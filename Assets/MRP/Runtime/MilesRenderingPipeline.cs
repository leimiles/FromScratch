using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace mrp {
    public class MilesRenderingPipeline : RenderPipeline {
        MilesRenderer milesRenderer = new MilesRenderer();
        protected override void Render(ScriptableRenderContext context, Camera[] cameras) {
            foreach (Camera camera in cameras) {
                milesRenderer.Init(context, camera);
                milesRenderer.Setup();
                milesRenderer.DrawSky();
                milesRenderer.Submit();
            }
        }
    }
}
