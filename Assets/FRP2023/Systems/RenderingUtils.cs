using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEngine.Funny.Rendering {
    /// <summary>
    /// 渲染操作的辅助工具
    /// </summary>
    public class RenderingUtils {

        /// <summary>
        /// 根据 rt 的引用设置，判断是否需要重新分配内存
        /// </summary>
        public static bool ReAllocateIfNeeded(
            ref RTHandle rtHandle,
            in RenderTextureDescriptor descriptor,
            FilterMode filterMode = FilterMode.Point,
            TextureWrapMode wrapMode = TextureWrapMode.Clamp,
            bool isShadowMap = false,
            int anisoLevel = 1,
            float mipMapBias = 0,
            string name = ""
            ) {
            if (RTHandleNeedsReAlloc(rtHandle, descriptor, filterMode, wrapMode, isShadowMap, anisoLevel, mipMapBias, name, false)) {
                rtHandle?.Release();
                rtHandle = RTHandles.Alloc(descriptor, filterMode, wrapMode, isShadowMap, anisoLevel, mipMapBias, name);
                return true;
            }
            return false;

        }

        /// <summary>
        /// 根据 shadertagid，创建并返回 drawingSettings
        /// </summary>
        static public DrawingSettings CreateDrawingSettings(ShaderTagId shaderTagId, ref RenderingData renderingData, SortingCriteria sortingCriteria) {
            SortingSettings sortingSettings = new SortingSettings(renderingData.cameraData.camera) { criteria = sortingCriteria };
            DrawingSettings drawingSettings = new DrawingSettings(shaderTagId, sortingSettings) {
                enableInstancing = renderingData.cameraData.cameraType == CameraType.Preview ? false : true
            };
            return drawingSettings;
        }

        /// <summary>
        /// 根据 shadertagid 列表，创建并返回 drawingSettings
        /// </summary>
        static public DrawingSettings CreateDrawingSettings(List<ShaderTagId> shaderTagIds,
            ref RenderingData renderingData, SortingCriteria sortingCriteria) {
            if (shaderTagIds == null || shaderTagIds.Count == 0) {
                // if no shaderTagIds, create default
                return CreateDrawingSettings(new ShaderTagId("SRPDefaultUnlit"), ref renderingData, sortingCriteria);
            }

            DrawingSettings drawingSettings = CreateDrawingSettings(shaderTagIds[0], ref renderingData, sortingCriteria);
            for (int i = 1; i < shaderTagIds.Count; ++i) {
                drawingSettings.SetShaderPassName(i, shaderTagIds[i]);
            }
            return drawingSettings;
        }

        /// <summary>
        /// 根据条件判断是否需要重新分配内存
        /// </summary>
        internal static bool RTHandleNeedsReAlloc(
            RTHandle rtHandle,
            in RenderTextureDescriptor descriptor,
            FilterMode filterMode,
            TextureWrapMode wrapMode,
            bool isShadowMap,
            int anisoLevel,
            float mipMapBias,
            string name,
            bool useScaling
            ) {
            // rt 未初始化，需要重新分配
            if (rtHandle == null || rtHandle.renderTexture == null) {
                return true;
            }
            // rt 的 scaling 状态不同，需要重新分配
            if (rtHandle.useScaling != useScaling) {
                return true;
            }
            // rt 没有使用 scaling，同时 rt 的宽度与高度不同，需要重新分配
            if (!useScaling && (rtHandle.renderTexture.width != descriptor.width || rtHandle.renderTexture.height != descriptor.height)) {
                return true;
            }

            return
                // 下列条件只要有一个成立，就需要重新分配
                (rtHandle.renderTexture.descriptor.depthBufferBits == (int)DepthBits.None && !isShadowMap && rtHandle.renderTexture.descriptor.graphicsFormat != descriptor.graphicsFormat) ||
                rtHandle.renderTexture.descriptor.depthBufferBits != descriptor.depthBufferBits ||
                rtHandle.renderTexture.descriptor.dimension != descriptor.dimension ||
                rtHandle.renderTexture.descriptor.enableRandomWrite != descriptor.enableRandomWrite ||
                rtHandle.renderTexture.descriptor.useMipMap != descriptor.useMipMap ||
                rtHandle.renderTexture.descriptor.autoGenerateMips != descriptor.autoGenerateMips ||
                rtHandle.renderTexture.descriptor.msaaSamples != descriptor.msaaSamples ||
                rtHandle.renderTexture.descriptor.bindMS != descriptor.bindMS ||
                rtHandle.renderTexture.descriptor.useDynamicScale != descriptor.useDynamicScale ||
                rtHandle.renderTexture.descriptor.memoryless != descriptor.memoryless ||
                rtHandle.renderTexture.filterMode != filterMode ||
                rtHandle.renderTexture.wrapMode != wrapMode ||
                rtHandle.renderTexture.anisoLevel != anisoLevel ||
                rtHandle.renderTexture.mipMapBias != mipMapBias ||
                rtHandle.name != name
                ;

        }
    }
}