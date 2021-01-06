using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ManyColors : MonoBehaviour {
    static int baseColorID = Shader.PropertyToID("_BaseColor");
    static MaterialPropertyBlock materialPropertyBlock;
    [SerializeField]
    Color baseColor = Color.white;
    private void Awake() {
        OnValidate();
    }

    private void OnValidate() {
      
        if(materialPropertyBlock == null) {
            materialPropertyBlock = new MaterialPropertyBlock();
        }
        materialPropertyBlock.SetColor(baseColorID, baseColor);
        GetComponent<Renderer>().SetPropertyBlock(materialPropertyBlock);
    }
}
