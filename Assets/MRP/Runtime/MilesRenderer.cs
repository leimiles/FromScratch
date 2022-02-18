using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace mrp {
    public class MilesRenderer {
        ScriptableRenderContext context;
        Camera camera;
        public void Init(ScriptableRenderContext context, Camera camera) {
            this.context = context;
            this.camera = camera;
        }
        public void Setup() {
            context.SetupCameraProperties(camera);
        }
        public void DrawSky() {
            context.DrawSkybox(camera);
        }
        public void Submit() {
            context.Submit();
        }
    }
}
