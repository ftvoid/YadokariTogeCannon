using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* 制作日：2018/07/03
*/

/// <summary>
/// Inputの名前を変数として管理するクラスです
/// Enumにも対応させてInputの設定を変更した場合でもこのクラスを変更するだけでよくなりました
/// </summary>
public static class InputName
{
    //joystick button 0
    public static readonly string GAMEPAD_A = "Pad_A";
    //joystick button 1
    public static readonly string GAMEPAD_B = "Pad_B";
    //joystick button 3
    public static readonly string GAMEPAD_Y = "Pad_Y";
    //joystick button 2
    public static readonly string GAMEPAD_X = "Pad_X";
    //joystick button 7
    public static readonly string GAMEPAD_START = "";
    //joystick button 6
    public static readonly string GAMEPAD_BACK = "";
    //joystick button 4
    public static readonly string GAMEPAD_LB = "";
    //joystick button 5
    public static readonly string GAMEPAD_RB = "Pad_RB";

    //************ここから下がAxis(-1~+1)***************

    //3rd axis(Lが‐1、Rが+1)
    public static readonly string GAMEPAD_LR_TRIGGER = "LR_Trigger";
    //X axis(左が‐1、右が+1)
    public static readonly string GAMEPAD_STICK_LEFT_HORIZONTAL = "Horizontal";
    //Y axis(上が‐1、下が+1)
    public static readonly string GAMEPAD_STICK_LEFT_VERTICAL = "Vertical";
    //4th axis(左が‐1、右が+1)
    public static readonly string GAMEPAD_STICK_RIGTH_HORIZONTAL = "Camera_X";
    //5th axis(上が‐1、下が+1)
    public static readonly string GAMEPAD_STICK_RIGTH_VERTICAL = "Camera_Y";
    //6th axis(左が‐1、右が+1)
    public static readonly string GAMEPAD_DIRECTION_HORIZONTAL = "";
    //7th axis(上が‐1、下が+1)
    public static readonly string GAMEPAD_DIRECTION_VERTICAL = "";

    //Inputの名前をすべてEnumで管理しています
    public static readonly Dictionary<InputEnum, string> INPUT_NAMES = new Dictionary<InputEnum, string>()
    {
        { InputEnum.GAMEPAD_A,GAMEPAD_A},
        { InputEnum.GAMEPAD_B,GAMEPAD_B},
        { InputEnum.GAMEPAD_BACK,GAMEPAD_BACK},
        { InputEnum.GAMEPAD_DIRECTION_HORIZONTAL,GAMEPAD_DIRECTION_HORIZONTAL},
        { InputEnum.GAMEPAD_DIRECTION_VERTICAL,GAMEPAD_DIRECTION_VERTICAL},
        { InputEnum.GAMEPAD_LB,GAMEPAD_LB},
        { InputEnum.GAMEPAD_LR_TRIGGER,GAMEPAD_LR_TRIGGER},
        { InputEnum.GAMEPAD_RB,GAMEPAD_RB},
        { InputEnum.GAMEPAD_START,GAMEPAD_START},
        { InputEnum.GAMEPAD_STICK_LEFT_HORIZONTAL,GAMEPAD_STICK_LEFT_HORIZONTAL},
        { InputEnum.GAMEPAD_STICK_LEFT_VERTICAL,GAMEPAD_STICK_LEFT_VERTICAL},
        { InputEnum.GAMEPAD_STICK_RIGTH_HORIZONTAL,GAMEPAD_STICK_RIGTH_HORIZONTAL},
        { InputEnum.GAMEPAD_STICK_RIGTH_VERTICAL,GAMEPAD_STICK_RIGTH_VERTICAL},
        { InputEnum.GAMEPAD_X,GAMEPAD_X},
        { InputEnum.GAMEPAD_Y,GAMEPAD_Y},
    };
}
