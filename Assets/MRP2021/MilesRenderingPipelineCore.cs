using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System;

namespace MilesRenderingPipeline {
    public sealed partial class MilesRenderingPipeline {
        public static MilesRenderingPipelineAsset asset {
            get => GraphicsSettings.currentRenderPipeline as MilesRenderingPipelineAsset;
        }
        Comparison<Camera> cameraComparision = (camera1, camera2) => { return (int)camera1.depth - (int)camera2.depth; };
        void SortCameras(List<Camera> cameras) {
            if (cameras.Count > 1) {
                cameras.Sort(cameraComparision);
            }

        }

    }
    public struct CameraData {
        public Camera camera;
        public CameraRenderType renderType;
        public CameraType cameraType;
        public bool postProcessEnabled;
        public ScriptableRenderer renderer;
        public RenderTextureDescriptor cameraTargetDescriptor;

        public bool isSceneViewCamera => cameraType == CameraType.SceneView;
    }

    public struct RenderingData {
        public CullingResults cullingResults;
        public CameraData cameraData;
    }

}
