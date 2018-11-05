using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Common {

    public static int MAX_CHARACTERS = 10;
    /// <summary>
    /// The default color during dev.
    /// </summary>
    public static Color PINK = new Color(255, 46, 255);

    public enum Accuracy { WEAK, GOOD, PERFECT }
    public enum Character { BOGOTIN = ~0, MORINO =0, EBESIR }//to complete
    public enum DamageType { FAIL, FIRE, EARTH, WATER, LIGHTNING, HOLY, DARK, SPEED }//to complete
    public enum KeyType { LP, HP, LK, HK, JUMP, DOWN } //to complete
    
}
