using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System;

namespace MilesRenderingPipeline {
    public sealed partial class MilesRenderingPipeline {
        private static class Profiling {
            public static class Pipeline {
                // nameof 
                public static readonly ProfilingSampler beginContextRendering = new ProfilingSampler($"{nameof(RenderPipeline)}.{nameof(BeginContextRendering)}");

            }

        }
        Comparison<Camera> cameraComparision = (camera1, camera2) => { return (int)camera1.depth - (int)camera2.depth; };
        void SortCameras(List<Camera> cameras) {
            if (cameras.Count > 1) {
                cameras.Sort(cameraComparision);
            }

        }

    }
    internal enum MilesRenderingPipelineProfileID {
        MilesRenderTotal
    }
}
