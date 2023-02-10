using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Rendering;

namespace UnityEngine.Funny.Rendering {
    /// <summary>
    /// 用于管理渲染目标的双缓冲, 不要混淆这个双缓冲与 backbuffer/frontbuffer 的概念
    /// </summary>
    internal sealed class RenderTargetBufferSystem {
        struct SwapBuffer {
            public RTHandle rtHandle_MSAA;
            public RTHandle rtHandle_Resolve;
            public string name;
            public int msaa;
        }

        SwapBuffer m_ABuffer;
        SwapBuffer m_BBuffer;

        static bool m_ABufferIsBackBuffer = true;


        /// <summary>
        /// 构造时为双 buffer 命名
        public RenderTargetBufferSystem(string name) {
            m_ABuffer.name = name + "A";
            m_BBuffer.name = name + "B";
        }

    }
}
