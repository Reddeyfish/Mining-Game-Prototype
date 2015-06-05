using UnityEngine;
using System.Collections;


// for the autocomplete!

public class Tags : MonoBehaviour {
    public const string player = "Player";
	public const string block = "Block";
    public const string comboUI = "ComboUI";
    public const string comboProgressUI = "ComboProgressUI";
    public const string popup = "Popup";
}

public class Axis
{
    public const string horizontal = "Horizontal";
    public const string vertical = "Vertical";
}

public class Layers
{
    public const string blocks = "Blocks";
    public const string transBlocks = "TransparentBlocks";
    public const string player = "Player";
}

public class AnimatorParams
{
    public static int dig = Animator.StringToHash("Digging");
}

public class PlayerPrefKeys
{
    public const string map = "Map";
    public const string inventory = "Inventory";
    public const string OreX = "OreSeedX";
    public const string OreY = "OreSeedY";
    public const string ColorX = "ColorSeedX";
    public const string ColorY = "ColorSeedY";
    public const string TransX = "TransSeedX";
    public const string TransY = "TransSeedY";
    public const string obstacle = "Obstacle";
}

public class Options
{
    public const string SoundLevel = "SoundLevel";
    public const string MusicLevel = "MusicLevel";
}

public class ShaderParams
{
    public static int color = Shader.PropertyToID("_Color");
    public static int emission = Shader.PropertyToID("_EmissionColor");
}