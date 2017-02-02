using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using System.Collections;

[System.Serializable]
public struct IntVector3 {
    public int x, y, z;

    public IntVector3(int x, int y, int z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }


    public override bool Equals(object obj) {
        return (x == ((IntVector3)obj).x && (y == ((IntVector3)obj).y) && (z == ((IntVector3)obj).z));
    }
    public override int GetHashCode() {
        return 0;
    }

    static public explicit operator Vector3(IntVector3 intVec2) {
        return new Vector3(intVec2.x, intVec2.y, intVec2.z);
    }
    static public explicit operator Vector2(IntVector3 intVec2) {
        return new Vector2(intVec2.x, intVec2.y);
    }

    public static IntVector3 operator +(IntVector3 a, IntVector3 b) {
        return new IntVector3(a.x + b.x, a.y + b.y, a.z + b.z);
    }
    public static IntVector3 operator -(IntVector3 a, IntVector3 b) {
        return new IntVector3(a.x - b.x, a.y - b.y, a.z - b.z);
    }
    public static bool operator ==(IntVector3 a, IntVector3 b) {
        return (a.x == b.x && a.y == b.y && a.z == b.z);
    }
    public static bool operator !=(IntVector3 a, IntVector3 b) {
        return !(a.x == b.x && a.y == b.y && a.z == b.z);
    }
}