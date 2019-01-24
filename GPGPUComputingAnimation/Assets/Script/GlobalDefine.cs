using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Vector4Int
{
    public int x;
    public int y;
    public int z;
    public int w;
    public Vector4Int(int x, int y, int z, int w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }
}

[System.Serializable]
public unsafe struct Matrix3x4
{
    public float m00;
    public float m10;
    public float m20;
    public float m01;
    public float m11;
    public float m21;
    public float m02;
    public float m12;
    public float m22;
    public float m03;
    public float m13;
    public float m23;
    public const int SIZE = 48;
    public Matrix3x4(Matrix4x4 target)
    {
        m00 = target.m00;
        m01 = target.m01;
        m02 = target.m02;
        m03 = target.m03;
        m10 = target.m10;
        m11 = target.m11;
        m12 = target.m12;
        m13 = target.m13;
        m20 = target.m20;
        m21 = target.m21;
        m22 = target.m22;
        m23 = target.m23;
    }
    public Matrix3x4(Matrix4x4* target)
    {
        m00 = target->m00;
        m01 = target->m01;
        m02 = target->m02;
        m03 = target->m03;
        m10 = target->m10;
        m11 = target->m11;
        m12 = target->m12;
        m13 = target->m13;
        m20 = target->m20;
        m21 = target->m21;
        m22 = target->m22;
        m23 = target->m23;
    }
    public Matrix3x4(ref Matrix4x4 target)
    {
        m00 = target.m00;
        m01 = target.m01;
        m02 = target.m02;
        m03 = target.m03;
        m10 = target.m10;
        m11 = target.m11;
        m12 = target.m12;
        m13 = target.m13;
        m20 = target.m20;
        m21 = target.m21;
        m22 = target.m22;
        m23 = target.m23;
    }
}

public struct AnimState
{
    public Matrix3x4 localToWorldMatrix;
    public float frame;
};

public struct SkinPoint
{
    public Vector3 position;
    //public Vector3 normal;
    //public Vector4 tangent;
    public Vector2 uv;
    public Vector4 boneWeight;
    public Vector4Int boneIndex;
};

struct Point
{
    Vector3 vertex;
    //Vector4 tangent;
    //Vector3 normal;
    Vector2 texcoord;
    //uint objIndex;
};
