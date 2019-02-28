using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosDirLength {
    public Vector2 position;
    public int direction;
    public float length;
    public PosDirLength(Vector2 pos, int dir, float len)
    {
        position = pos;
        direction = dir;
        length = len;
    }
}
