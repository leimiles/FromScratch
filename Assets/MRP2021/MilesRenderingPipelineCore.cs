using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System;

namespace MilesRenderingPipeline {
    public sealed partial class MilesRenderingPipeline {
        Comparison<Camera> cameraComparision = (camera1, camera2) => { return (int)camera1.depth - (int)camera2.depth; };
        void SortCameras(List<Camera> cameras) {
            if (cameras.Count > 1) {
                cameras.Sort(cameraComparision);
            }

        }

    }
    public struct CameraData {
        public bool postProcessEnabled;
        public MilesScriptableRenderer renderer;
        public Camera camera;
    }

}
