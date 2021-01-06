using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ChickenTerrainMaterialGUI : ShaderGUI {

    MaterialEditor materialEditor;
    MaterialProperty[] materialProperties;
    static GUIContent staticLabel = new GUIContent();

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties) {
        //base.OnGUI(materialEditor, properties);
        this.materialEditor = materialEditor;
        this.materialProperties = properties;
        DoMainBlock();
    }

    void DoMainBlock() {
        GUILayout.Label("Chicken Terrain V2", EditorStyles.boldLabel);

        GUILayout.Label("Splat Maps: ");
        GUILayout.Space(5);
        MaterialProperty splatMap01 = GetPropFromMat("_Control01");
        GUIContent splatMap01_GUI = new GUIContent(splatMap01.displayName, "Input for splatmap0.");

        MaterialProperty splatMap02 = GetPropFromMat("_Control02");
        GUIContent splatMap02_GUI = new GUIContent(splatMap02.displayName, "Input for splatmap1.");

        GUILayout.BeginHorizontal();
        materialEditor.TexturePropertySingleLine(splatMap01_GUI, splatMap01);
        materialEditor.TexturePropertySingleLine(splatMap02_GUI, splatMap02);
        GUILayout.EndHorizontal();

        GUILayout.Space(5);
        GUILayout.Label("Edge Blend: ");
        GUILayout.Space(5);
/*        MaterialProperty _weight = GetPropFromMat("_Weight");
        materialEditor.ShaderProperty(_weight, MakeGUILabel(_weight));*/
        DoSliderBlock("_Weight01");
        //DoSliderBlock("_Weight02");
        GUILayout.Space(5);


        GUILayout.Label("Terrain Diffuse: ");
        GUILayout.Space(5);
        DoLayerBlock("_TexLayer01", "_Color01");
        DoLayerBlock("_TexLayer02", "_Color02");
        DoSliderBlock("_Edge02");
        DoLayerBlock("_TexLayer03", "_Color03");
        DoSliderBlock("_Edge03");
        DoLayerBlock("_TexLayer04", "_Color04");
        DoSliderBlock("_Edge04");
        DoLayerBlock("_TexLayer05", "_Color05");
        DoSliderBlock("_Edge05");
        DoLayerBlock("_TexLayer06", "_Color06");
        DoSliderBlock("_Edge06");
        DoLayerBlock("_TexLayer07", "_Color07");
        DoSliderBlock("_Edge07");
        DoLayerBlock("_TexLayer08", "_Color08");
        DoSliderBlock("_Edge08");
        DoSliderBlock("_SpatLod");
        DoSliderBlock("_LayerLod");

    }

    void DoSliderBlock(string propertyName) {
        MaterialProperty prop = GetPropFromMat(propertyName);
        GUIContent pro_GUI = new GUIContent(prop.displayName);
        materialEditor.ShaderProperty(prop, pro_GUI);
        
    }

    void DoLayerBlock(string layerName, string colorName) {
        MaterialProperty diffuse = GetPropFromMat(layerName);
        GUIContent diffuse0_GUI = new GUIContent(diffuse.displayName);
        materialEditor.TexturePropertySingleLine(diffuse0_GUI, diffuse, GetPropFromMat(colorName));
        materialEditor.TextureScaleOffsetProperty(diffuse);
    }

    MaterialProperty GetPropFromMat(string propertyName) {
        return FindProperty(propertyName, this.materialProperties);
    }

    static GUIContent MakeGUILabel(MaterialProperty materialProperty, string tooltip = null) {
        staticLabel.text = materialProperty.displayName;
        staticLabel.tooltip = tooltip;
        return staticLabel;
    }
}
