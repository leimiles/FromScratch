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

        /// 预定义的 4 个主要的渲染块 render block，这是渲染管线的主流程框架
        static class RenderPassBlock {

            // 这个 pass block 不依赖摄影机矩阵
            public static readonly int BeforeRendering = 0;
            // 不透明与透明 block
            public static readonly int MainRenderingOpaque = 1;
            public static readonly int MainRenderingTransparent = 2;
            // post processing 之后
            public static readonly int AfterRendering = 3;
        }

        // 共四个 render pass block，before rendering, opaque, transparent, after rendering
        const int k_RenderPassBlockCount = 4;

        /// <summary>
        /// 渲染流程会将 passes 按照索引分成一个个 block，按照 block 为单位执行该 block 中的所有 passes
        /// </summary>
        internal struct RenderBlocks : IDisposable {

            // 用于保存 render pass event 的集合，在初始化时，这个集合的长度被设为固定常量 4
            private NativeArray<RenderPassEvent> m_BlockEventLimits;

            // 用于保存每个 render block 需要渲染的 pass 个数，这些 pass 都会按照其 render event 被安放在对应的 block 范围内
            private NativeArray<int> m_BlockRanges;

            // 用于保存每个 block 中包含多少个 passes 的个数
            private NativeArray<int> m_BlockRangeLengths;

            public RenderBlocks(List<ScriptableRenderPass> activeRenderPassQueue) {

                // 默认长度为 4，每个元素都是一个 render pass event 代表的整数值，在渲染时，所有的 pass 自身的 pass event 数值需要和当前所在的 block event limit 进行对比，确保他们按照正确的序列渲染
                m_BlockEventLimits = new NativeArray<RenderPassEvent>(k_RenderPassBlockCount, Allocator.Temp);

                // 默认长度为 5，其中第一个元素数值始终为 0，其他元素中，每一个保存的是当前需要渲染的 passes 数量
                m_BlockRanges = new NativeArray<int>(m_BlockEventLimits.Length + 1, Allocator.Temp);

                // 默认长度为 5，每个元素代表当前 block range 之间的 pass 数量的差
                m_BlockRangeLengths = new NativeArray<int>(m_BlockRanges.Length, Allocator.Temp);

                // 基于 render pass event，划分 4 个主要的渲染 block
                m_BlockEventLimits[RenderPassBlock.BeforeRendering] = RenderPassEvent.BeforeRenderingPrePasses;     // 150
                m_BlockEventLimits[RenderPassBlock.MainRenderingOpaque] = RenderPassEvent.AfterRenderingOpaques;    // 300
                m_BlockEventLimits[RenderPassBlock.MainRenderingTransparent] = RenderPassEvent.AfterRenderingPostProcessing;    // 600
                m_BlockEventLimits[RenderPassBlock.AfterRendering] = (RenderPassEvent)Int32.MaxValue;       // 无限大

                FillBlockRanges(activeRenderPassQueue);
                m_BlockEventLimits.Dispose();

                // 4 次遍历，通过 block range 之间 pass 的数量差来保存
                for (int i = 0; i < m_BlockRanges.Length - 1; i++) {
                    m_BlockRangeLengths[i] = m_BlockRanges[i + 1] - m_BlockRanges[i];
                }
            }


            /// <summary>
            /// 设置每个 render block 的 pass 的数量，总共有 4 个 render block 主块
            /// </summary>
            void FillBlockRanges(List<ScriptableRenderPass> activeRenderPassQueue) {
                int currentRangeIndex = 0;
                int currentRenderPass = 0;

                // 将 m_BlockRanges[0] 设为 0，然后让 currentRangeIndex += 1
                m_BlockRanges[currentRangeIndex++] = 0;

                // 根据定义的 render event 数量来遍历 4 个渲染主步骤, 3 次循环
                for (int i = 0; i < m_BlockEventLimits.Length - 1; ++i) {
                    // 如果 activeRenderPass 中有内容，同时正在被渲染的 renderpass 的 render pass event 代表的值不超过主 block 流程的范围
                    while (currentRenderPass < activeRenderPassQueue.Count && activeRenderPassQueue[currentRenderPass].renderPassEvent < m_BlockEventLimits[i]) {
                        // 究竟有多少个 pass 在指定的 block limits 范围内，把这个数量加起来
                        currentRenderPass++;
                    }

                    m_BlockRanges[currentRangeIndex++] = currentRenderPass;
                }
                // miles ask, 为什么要在最后的 block range 中保存所有 active render pass 的数量
                m_BlockRanges[currentRangeIndex] = activeRenderPassQueue.Count;
            }


            /// <summary>
            /// 通过输入的 render pass block 阶段，0, 1, 2, 3, 返回该阶段存在的 render pass 数量
            /// </summary>
            public int GetLength(int index) {
                return m_BlockRangeLengths[index];
            }

            public void Dispose() {
                m_BlockRangeLengths.Dispose();
                m_BlockRanges.Dispose();
            }

            /// <summary>
            /// 获得一个对象来表示当前渲染处在 block 中的哪个 pass
            /// </summary>
            public BlockRange GetRange(int index) {
                return new BlockRange(m_BlockRanges[index], m_BlockRanges[index + 1]);

            }

            /// <summary>
            /// 用于标记 block range 中的开始 pass 和结束 pass 的索引
            /// </summary>
            public struct BlockRange : IDisposable {
                int m_Current;
                int m_End;
                public int Current {
                    get {
                        return m_Current;
                    }
                }

                public BlockRange(int beginAt, int EndAt) {
                    Assertions.Assert.IsTrue(beginAt <= EndAt);
                    m_Current = beginAt < EndAt ? beginAt : EndAt;
                    m_End = EndAt > beginAt ? EndAt : beginAt;
                    m_Current -= 1;
                }

                public BlockRange GetEnumerator() {
                    return this;
                }

                public bool MoveNext() {
                    return ++m_Current < m_End;
                }

                public void Dispose() {
                }
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

            var renderblocks = new RenderBlocks(m_ActiveRenderPassQueue);
            if (renderblocks.GetLength(RenderPassBlock.MainRenderingTransparent) > 0) {
                ExecuteBlock(RenderPassBlock.MainRenderingTransparent, in renderblocks, renderContext, ref renderingData);
            }

            internalFinishRendering(renderContext, false, renderingData);
        }

        /// <summary>
        /// 执行 render block，此方法中 submit 到 GPU 的步骤时可选的，默认情况下不会在此处进行 submit
        /// </summary>
        void ExecuteBlock(int renderPassBlockIndex, in RenderBlocks renderBlocks, ScriptableRenderContext renderContext, ref RenderingData renderingData, bool sumbit = false) {
            if (sumbit) {
                renderContext.Submit();
            }
        }

        /// <summary>
        /// 执行每个 render pass
        /// </summary>
        void ExecuteRenderPass(ScriptableRenderContext renderContext, ScriptableRenderPass renderPass, ref RenderingData renderingData) {

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
