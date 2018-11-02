using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboClass
{
    public KeyCode keycode;
    public Ability ability;
    public float timewindow;
    public Common.Accuracy accuracy;
    public float damage;

    public ComboClass() {
        timewindow = 4f;
    }

    public ComboClass(KeyCode keycode, Ability ability, float timewindow, Common.Accuracy accuracy, float damage)
    {
        this.keycode = keycode;
        this.ability = ability;
        this.timewindow = timewindow;
        this.accuracy = accuracy;
        this.damage = damage;
    }
}
