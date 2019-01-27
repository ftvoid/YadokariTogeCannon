using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* 制作日：2018/07/03
*/

/// <summary>
/// string型でInputを管理すると誤字によりバグが出るのでEnumと紐づけしています
/// </summary>
public enum InputEnum
{
    //joystick button 0
    GAMEPAD_A,
    //joystick button 1
    GAMEPAD_B,
    //joystick button 3
    GAMEPAD_Y,
    //joystick button 2
    GAMEPAD_X,
    //joystick button 7
    GAMEPAD_START,
    //joystick button 6
    GAMEPAD_BACK,
    //joystick button 4
    GAMEPAD_LB,
    //joystick button 5
    GAMEPAD_RB,

    //************ここから下がAxis(-1~+1)***************

    //3rd axis(Lが‐1、Rが+1)
    GAMEPAD_LR_TRIGGER,
    //X axis(左が‐1、右が+1)
    GAMEPAD_STICK_LEFT_HORIZONTAL,
    //Y axis(上が‐1、下が+1)
    GAMEPAD_STICK_LEFT_VERTICAL,
    //4th axis(左が‐1、右が+1)
    GAMEPAD_STICK_RIGTH_HORIZONTAL,
    //5th axis(上が‐1、下が+1)
    GAMEPAD_STICK_RIGTH_VERTICAL,
    //6th axis(左が‐1、右が+1)
    GAMEPAD_DIRECTION_HORIZONTAL,
    //7th axis(上が‐1、下が+1)
    GAMEPAD_DIRECTION_VERTICAL

}
