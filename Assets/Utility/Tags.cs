using UnityEngine;
using System.Collections;


// for the autocomplete!

public class Tags : MonoBehaviour {
    public const string player = "Player";
	public const string block = "Block";
    public const string energyUI = "ComboUI";
    public const string comboProgressUI = "ComboProgressUI";
    public const string popup = "Popup";
    public const string itemSlots = "ItemSlotsUI";
    public const string canvas = "Canvas";
    public const string screenFlash = "ScreenFlash";
    public const string inventory = "BaseInventory";
    public const string tutorial = "Tutorial";
    public const string mouseParent = "MouseParent";
    public const string objective = "Objective";
}

public class Scenes
{
    public const string mainMenu = "MainMenu";
    public const string mainScene = "MainScene";
    public const string tutorial = "TheTutorial";
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
    public const string baseInventory = "baseInventory";
    public const string items = "Items";
    public const string OreX = "OreSeedX";
    public const string OreY = "OreSeedY";
    public const string ColorX = "ColorSeedX";
    public const string ColorY = "ColorSeedY";
    public const string TransX = "TransSeedX";
    public const string TransY = "TransSeedY";
    public const string obstacle = "Obstacle";
    public const string guffin = "Guffin";
    public const string objectives = "Objectives";
    public const string tutorialComplete = "tutorialComplete";
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