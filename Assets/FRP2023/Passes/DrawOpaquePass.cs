using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEngine.Funny.Rendering {
    /// <summary>
    /// 用于正常渲染不透明序列的对象
    /// </summary>
    public class DrawOpaquePass : ScriptableRenderPass {
        public DrawOpaquePass() { }
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) {
        }
    }
}
