using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace UnityEngine.Miles.Rendering {
    /// <summary>
    /// 渲染接口 renderer 的抽象，partial，摄影机使用渲染接口实现具体的渲染流程
    /// </summary>
    public partial class ScriptableRenderer : IDisposable {
        public void Dispose() {
        }
    }
}
