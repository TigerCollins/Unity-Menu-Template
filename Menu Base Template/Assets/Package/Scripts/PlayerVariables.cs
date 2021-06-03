using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TextureResolutionFactor { low, Medium, High }
public enum AntiAliasingFactor { None, MSAAx2, MSAAx4, MSAAx8, MSAAx16 }
public enum AmbientOcclusionFactor { None, low, Medium, High }

[CreateAssetMenu(fileName = "new playerprofile", menuName = "Player Profile")]
[System.Serializable]


public class PlayerVariables : ScriptableObject
{
    [Header("Player Options - Audio")]
    //Volume
    public float masterVolume;
    public float masterVolumeCore;
    public float gameVolume;
    public float gameVolumeCore;
    public float menuVolume;
    public float menuVolumeCore;
    public float musicVolume;
    public float musicVolumeCore;
    [Header("Player Options - Visual")]
    //Visual
    public bool chromaticAberration;
    public bool filmGrain;
    public bool vignette;
    public bool dynamicResolution;
    public bool anistropicFiltering;
    public bool VSync;
    public TextureResolutionFactor textureResolution;
    public AntiAliasingFactor antiAliasing;
    public bool ambientOcclusion;
    [Header("Player Options - Resolution")]
    public bool fullScreen;
    public int resolutionWidth;
    public int resolutionHeight;


}
