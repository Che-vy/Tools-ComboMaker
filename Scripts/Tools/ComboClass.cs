using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboClass
{
    public KeyCode keycode;
    public Ability ability;
    public Animation animation;
    public float timewindow;
    public Common.Accuracy accuracy;
    public float damage;

    public ComboClass() {
        timewindow = 4f;
    }

    public ComboClass(KeyCode keycode, Ability ability, Animation animation, float timewindow, Common.Accuracy accuracy, float damage)
    {
        this.keycode = keycode;
        this.ability = ability;
        this.animation = animation;
        this.timewindow = timewindow;
        this.accuracy = accuracy;
        this.damage = damage;
    }
}
