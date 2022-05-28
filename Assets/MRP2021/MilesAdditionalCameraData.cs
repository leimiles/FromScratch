using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[DisallowMultipleComponent]
[RequireComponent(typeof(Camera))]
[ImageEffectAllowedInSceneView]
public class MilesAdditionalCameraData : MonoBehaviour, ISerializationCallbackReceiver, IAdditionalData {
    public void OnAfterDeserialize() {

    }

    public void OnBeforeSerialize() {

    }
}