using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinChanger : MonoBehaviour {
    public Texture[] texturesDefault;
    public Texture[] texturesA;
    public Texture[] texturesB;
    public Texture[] texturesC;

    Dictionary<string, Texture[]> textures;

    public int activeSkin = 0;

    Material[] meshMaterials;

    void LoadTexturesIntoDictionary(Texture[] listOfTextures) {
        foreach (Texture tex in listOfTextures) {
            List<Texture> texList = new List<Texture>();
            texList.Add(tex);
            if (!textures.ContainsKey(tex.name)) {
                textures.Add(tex.name, texList.ToArray());
            } else {
                List<Texture> list = new List<Texture>();
                list.AddRange(textures[tex.name]);
                list.AddRange(texList);
                textures[tex.name] = list.ToArray();
            }
        }
    }

    void Awake() {
        meshMaterials = GetComponentInChildren<Renderer>().materials;
        textures = new Dictionary<string, Texture[]>();

        LoadTexturesIntoDictionary(texturesDefault);
        LoadTexturesIntoDictionary(texturesA);
        LoadTexturesIntoDictionary(texturesB);
        LoadTexturesIntoDictionary(texturesC);
    }

    void ReplaceTexturesWithSkin() {
        foreach(Material material in meshMaterials) {
            string name = material.name.Replace(" (Instance)", "");
            material.mainTexture = textures[name][activeSkin];
        }
    }

    void PreviousSkin() {
        if (activeSkin == 0) {
            activeSkin = 3;
        } else {
            activeSkin = activeSkin - 1;
        }
    }

    void NextSkin() {
        if (activeSkin == 3) {
            activeSkin = 0;
        } else {
            activeSkin = activeSkin + 1;
        }
    }

    void Update() {
        if (InputManager.instance.fire4) {
            PreviousSkin();
            ReplaceTexturesWithSkin();
        }
        if (InputManager.instance.fire5) {
            NextSkin();
            ReplaceTexturesWithSkin();
        }
    }
}
