using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace UnityEngine.Miles.Rendering {
    // 渲染接口的抽象，摄影机使用渲染接口实现具体的渲染流程
    public class ScriptableRenderer : IDisposable {
        public void Dispose() {
        }
    }
}
