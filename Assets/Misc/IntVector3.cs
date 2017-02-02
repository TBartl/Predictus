using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using System.Collections;

[System.Serializable]
public struct IntVector2 {
    public int x, y, z;

    public IntVector2(int x, int y, int z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }


    public override bool Equals(object obj) {
        return (x == ((IntVector2)obj).x && (y == ((IntVector2)obj).y) && (z == ((IntVector2)obj).z));
    }
    public override int GetHashCode() {
        return 0;
    }

    static public explicit operator Vector3(IntVector2 intVec2) {
        return new Vector3(intVec2.x, intVec2.y, intVec2.z);
    }
    static public explicit operator Vector2(IntVector2 intVec2) {
        return new Vector2(intVec2.x, intVec2.y);
    }

    public static IntVector2 operator +(IntVector2 a, IntVector2 b) {
        return new IntVector2(a.x + b.x, a.y + b.y, a.z + b.z);
    }
    public static IntVector2 operator -(IntVector2 a, IntVector2 b) {
        return new IntVector2(a.x - b.x, a.y - b.y, a.z - b.z);
    }
    public static bool operator ==(IntVector2 a, IntVector2 b) {
        return (a.x == b.x && a.y == b.y && a.z == b.z);
    }
    public static bool operator !=(IntVector2 a, IntVector2 b) {
        return !(a.x == b.x && a.y == b.y && a.z == b.z);
    }
}