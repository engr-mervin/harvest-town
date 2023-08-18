using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character",menuName = "Player Character")]
public class Character : ScriptableObject
{
    public AnimationClip idleRight;
    public AnimationClip idleLeft;
    public AnimationClip idleBack;
    public AnimationClip idleFront;


    public AnimationClip runRight;
    public AnimationClip runLeft;
    public AnimationClip runBack;
    public AnimationClip runFront;

    public AnimationClip standRight;
    public AnimationClip standLeft;
    public AnimationClip standBack;
    public AnimationClip standFront;

    public AnimationClip phoneOn;
    public AnimationClip phoneOff;
    public AnimationClip sitRight;
    public AnimationClip sitLeft;

}
