using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	public static IntVector3 operator *(IntVector3 a, IntVector3 b) {
		return new IntVector3 (a.x * b.x, a.y * b.y, a.z * b.z);
	}


	float Pythagorean(float x, float y) {
		return Mathf.Sqrt ((x * x) + (y * y));
	}

	public float Length() {
		return Pythagorean (Pythagorean (Mathf.Abs(x), Mathf.Abs(y)), Mathf.Abs(z));
	}

    public static IntVector3 Left = new IntVector3(-1, 0, 0);
    public static IntVector3 Right = new IntVector3(1, 0, 0);
    public static IntVector3 Down = new IntVector3(0, -1, 0);
    public static IntVector3 Up = new IntVector3(0, 1, 0);
    public static IntVector3 Back = new IntVector3(0, 0, -1);
    public static IntVector3 Forward = new IntVector3(0, 0, 1);
}