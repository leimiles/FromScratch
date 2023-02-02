using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEngine.Miles.Rendering {
    // 用于支持渲染管线类的功能部分
    public sealed partial class MilesRenderPipeline {

        // 用于判断是否是 game 窗口
        public static bool IsGameCamera(Camera camera) {
            if (camera == null) {
                throw new ArgumentNullException("camera null error");
            } else {
                return camera.cameraType == CameraType.Game;
            }
        }

    }
}