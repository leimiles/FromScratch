using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEngine.Funny.Rendering {
    public class DrawSkyboxPass : ScriptableRenderPass {
        public DrawSkyboxPass(RenderPassEvent renderPassEvent) {
            this.renderPassEvent = renderPassEvent;
        }
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) {
            ref CameraData cameraData = ref renderingData.cameraData;
            Camera camera = cameraData.camera;

            context.DrawSkybox(camera);
        }
    }
}


