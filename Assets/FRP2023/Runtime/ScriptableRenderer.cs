using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.Collections;

namespace UnityEngine.Funny.Rendering {
    /// <summary>
    /// 渲染接口 renderer 的抽象，partial，摄影机使用渲染接口实现具体的渲染流程
    /// </summary>
    public abstract partial class ScriptableRenderer : IDisposable {
        List<ScriptableRenderPass> m_ActiveRenderPassQueue = new List<ScriptableRenderPass>(32);

        // 共四个 render pass block，before rendering, opaque, transparent, after rendering
        const int k_RenderPassBlockCount = 4;

        /// <summary>
        /// 渲染流程会将 passes 按照索引分成一个个 block，按照 block 为单位执行该 block 中的所有 passes
        /// </summary>
        internal struct RenderBlocks : IDisposable {

            // 用于保存 render pass event 的集合
            private NativeArray<RenderPassEvent> m_BlockEventLimits;

            // 用于保存当前需要被渲染的 render passes 的 index
            private NativeArray<int> m_BlockRanges;

            public RenderBlocks(List<ScriptableRenderPass> activeRenderPassQueue) {
                m_BlockEventLimits = new NativeArray<RenderPassEvent>(k_RenderPassBlockCount, Allocator.Temp);
                m_BlockRanges = new NativeArray<int>(m_BlockEventLimits.Length + 1, Allocator.Temp);
            }

            void FillBlockRanges(List<ScriptableRenderPass> activeRenderPassQueue) {


            }

            public void Dispose() {
            }
        }

        public ScriptableRenderer(ScriptableRendererData scriptableRendererData) {
            // 在 renderer 逻辑开始前清空所有 passes
            m_ActiveRenderPassQueue.Clear();

        }

        /// <summary>
        /// 基于 renderingdata 设置场景中所需的所有 passes，需要有子类 renderer 设计实现
        /// </summary>
        public abstract void Setup(ScriptableRenderContext renderContext, ref RenderingData renderingData);

        /// <summary>
        /// 插入需要渲染的 pass
        /// </summary>
        public void EnqueuePass(ScriptableRenderPass scriptableRenderPass) {
            m_ActiveRenderPassQueue.Add(scriptableRenderPass);
        }

        /// <summary>
        /// 执行场景渲染，即按照插入的 pass 顺序执行，不需要子类 renderer 设计实现
        /// </summary>
        public void Execute(ScriptableRenderContext renderContext, ref RenderingData renderingData) {
            // todo, need use renderblock for rendering passes


            internalFinishRendering(renderContext, false, renderingData);
        }

        /// <summary>
        /// 用于将新增的 render features 添加到渲染流程当中
        /// </summary>
        internal void AddRenderPasses(ref RenderingData renderingData) {
        }


        /// <summary>
        /// 渲染结束后清空 passes 队列
        /// </summary>
        void internalFinishRendering(bool resolveFinalTarget, RenderingData renderingData) {
            m_ActiveRenderPassQueue.Clear();

        }

        /// <summary>
        /// 渲染后清空 passes 队列，重置 native renderPass 数据，执行 rendering data 中的 command buffer，然后清空
        /// </summary>
        void internalFinishRendering(ScriptableRenderContext renderContext, bool resolveFinalTarget, RenderingData renderingData) {
            internalFinishRendering(resolveFinalTarget, renderingData);
        }

        public void Dispose() {
        }
    }
}
