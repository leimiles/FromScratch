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

        static RenderTextureDescriptor CreateRenderTextureDescriptor(Camera camera) {
            RenderTextureDescriptor desc;
            if (camera.targetTexture == null) {
                desc = new RenderTextureDescriptor(camera.pixelWidth, camera.pixelHeight);
            } else {
                desc = camera.targetTexture.descriptor;
                desc.width = camera.pixelWidth;
                desc.height = camera.pixelHeight;
            }
            desc.width = Mathf.Max(1, desc.width);
            desc.height = Mathf.Max(1, desc.height);
            return desc;
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
