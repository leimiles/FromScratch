using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Funny.Rendering {
    public enum ShaderPathID {
        Unlit
    }
    public static class ShaderUtils {

        // shader 对象的完整名称
        static readonly string[] s_ShaderPaths = {
            "Funny Render Pipeline/Unlit"
        };

        /// <summary>
        /// 通过 id 获取 shader 所在的路径
        /// </summary>
        public static string GetShaderPath(ShaderPathID id) {
            int index = (int)id;
            int arrayLength = s_ShaderPaths.Length;
            if (arrayLength > 0 && index >= 0 && index < arrayLength) {
                return s_ShaderPaths[index];
            } else {
                Debug.LogError("无法获取 shader 路径: (" + id + ": " + index + ")");
                return "";
            }
        }

#if UNITY_EDITOR
        static readonly string[] s_ShaderGUIDs = {
            // unlit
            "8fe503cd47431d041a46d26ccf1432a5",
        };

        /// <summary>
        /// 通过 path id 获得 shader 的 guid，注意 guid 是写死的
        /// </summary>
        public static string GetShaderGUID(ShaderPathID id) {
            int index = (int)id;
            int arrayLength = s_ShaderGUIDs.Length;
            if (arrayLength > 0 && index >= 0 && index < arrayLength) {
                return s_ShaderGUIDs[index];
            } else {
                Debug.LogError("无法获取 shader guid: (" + id + ": " + index + ")");
                return "";
            }
        }
#endif
    }
}
