using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Rendering;

namespace UnityEngine.Funny.Rendering {
    /// <summary>
    /// 用于管理渲染目标的双缓冲，默认情况下 a buffer 会关联 back buffer
    /// </summary>
    internal sealed class RenderTargetBufferSystem {
        struct SwapBuffer {
            // MSAA 开启时
            public RTHandle rtHandle_MSAA;
            // MSAA 关闭时
            public RTHandle rtHandle_Resolve;
            public string name;
            // MSAA 的数量
            public int msaaSamples;
        }

        SwapBuffer m_ABuffer;
        SwapBuffer m_BBuffer;


        // a buffer 默认关联 back buffer
        static bool m_ABufferIsBackBuffer = true;
        static RenderTextureDescriptor m_Descriptor;
        FilterMode m_FilterMode;
        bool m_AllowMSAA = true;

        /// <summary>
        /// 构造时为双 buffer 命名
        public RenderTargetBufferSystem(string name) {
            m_ABuffer.name = name + "A";
            m_BBuffer.name = name + "B";
        }

        public void Dispose() {
            m_ABuffer.rtHandle_MSAA?.Release();
            m_BBuffer.rtHandle_MSAA?.Release();
            m_ABuffer.rtHandle_Resolve?.Release();
            m_ABuffer.rtHandle_Resolve?.Release();
        }

        /// <summary>
        /// 返回 back buffer，如果 a buffer 不关联 back buffer，back buffer 就是 b buffer
        /// </summary>
        ref SwapBuffer backBuffer {
            get {
                // 不希望结构体被复制，使用索引返回
                return ref m_ABufferIsBackBuffer ? ref m_ABuffer : ref m_BBuffer;
            }
        }

        /// <summary>
        /// 返回 front buffer，如果 a buffer 不关联 back buffer，front buffer 就是 a buffer
        /// </summary>
        ref SwapBuffer frontBuffer {
            get {
                return ref m_ABufferIsBackBuffer ? ref m_BBuffer : ref m_ABuffer;
            }
        }

        // 获取 backbuffer 中的内容
        public RTHandle PeekBackBuffer() {
            if (m_AllowMSAA && backBuffer.msaaSamples > 1) {
                // 如果开启 msaa
                return backBuffer.rtHandle_MSAA;
            } else {
                // 如果关闭 msaa
                return backBuffer.rtHandle_Resolve;
            }
        }


        public RTHandle GetBackBuffer(CommandBuffer commandBuffer) {
            ReAllocate(commandBuffer);
            return PeekBackBuffer();
        }

        public void SetCameraSettings(RenderTextureDescriptor descriptor, FilterMode filterMode) {
            // 关闭 depth buffer，关联 framebuffer 时，depth 不需要了
            descriptor.depthBufferBits = 0;
            m_Descriptor = descriptor;
            m_FilterMode = filterMode;

            m_ABuffer.msaaSamples = m_Descriptor.msaaSamples;
            m_BBuffer.msaaSamples = m_Descriptor.msaaSamples;

            if (m_Descriptor.msaaSamples > 1) {
                EnableMSAA(true);
            }

        }

        public void EnableMSAA(bool enabled) {
            m_AllowMSAA = false;
            if (enabled) {
                m_ABuffer.msaaSamples = m_Descriptor.msaaSamples;
                m_BBuffer.msaaSamples = m_Descriptor.msaaSamples;
            }
        }


        // 分配时同时对 a buffer 和 b buffer 进行分配
        void ReAllocate(CommandBuffer commandBuffer) {
            var descriptor = m_Descriptor;

            // a buffer msaa rt 分配
            descriptor.msaaSamples = m_ABuffer.msaaSamples;
            if (descriptor.msaaSamples > 1) {
                RenderingUtils.ReAllocateIfNeeded(ref m_ABuffer.rtHandle_MSAA, descriptor, m_FilterMode, TextureWrapMode.Clamp, name: m_ABuffer.name);
            }

            // b buffer msaa rt 分配
            descriptor.msaaSamples = m_BBuffer.msaaSamples;
            if (descriptor.msaaSamples > 1) {
                RenderingUtils.ReAllocateIfNeeded(ref m_BBuffer.rtHandle_MSAA, descriptor, m_FilterMode, TextureWrapMode.Clamp, name: m_BBuffer.name);
            }

            // a buffer 非 msaa rt 分配
            descriptor.msaaSamples = 1;
            RenderingUtils.ReAllocateIfNeeded(ref m_ABuffer.rtHandle_Resolve, descriptor, m_FilterMode, TextureWrapMode.Clamp, name: m_ABuffer.name);
            // b buffer 非 msaa rt 分配
            RenderingUtils.ReAllocateIfNeeded(ref m_BBuffer.rtHandle_Resolve, descriptor, m_FilterMode, TextureWrapMode.Clamp, name: m_BBuffer.name);

            commandBuffer.SetGlobalTexture(m_ABuffer.name, m_ABuffer.rtHandle_Resolve);
            commandBuffer.SetGlobalTexture(m_BBuffer.name, m_BBuffer.rtHandle_Resolve);

        }

    }
}
