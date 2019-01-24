using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using System;

public unsafe class VFAnimationTest : TestBase
{
    private ComputeBuffer objBuffer;
    private ComputeBuffer verticesBuffer;
    private ComputeBuffer bindBuffer;
    private int bindPoseCount;
    private List<float> animStates = new List<float>();

    public Texture2D animTex;
    public Mesh targetMesh;
    public int framePerSecond = 30;
    public Material targetMaterial;

    void Awake()
    {
        objBuffer = new ComputeBuffer(characterPoints.Length, sizeof(float));
        Matrix4x4[] bindPosesArray = targetMesh.bindposes;
        bindPoseCount = bindPosesArray.Length;
        bindBuffer = new ComputeBuffer(bindPoseCount, sizeof(Matrix3x4));
        NativeArray<Matrix3x4> bindNative = new NativeArray<Matrix3x4>(bindPoseCount, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
        Matrix3x4* bindPtr = bindNative.Ptr();
        for (int i = 0; i < bindPoseCount; ++i)
        {
            *bindPtr = new Matrix3x4(ref bindPosesArray[i]);
            bindPtr++;
        }
        bindBuffer.SetData(bindNative);
        bindNative.Dispose();
        int[] triangles = targetMesh.triangles;
        Vector3[] vertices = targetMesh.vertices;
        //Vector3[] normals = targetMesh.normals;
        //Vector4[] tangents = targetMesh.tangents;
        BoneWeight[] weights = targetMesh.boneWeights;
        Vector2[] uv = targetMesh.uv;
        NativeArray<SkinPoint> allSkinPoints = new NativeArray<SkinPoint>(vertices.Length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
        SkinPoint* pointsPtr = allSkinPoints.Ptr();
        for (int i = 0; i < vertices.Length; ++i)
        {
            SkinPoint* currentPtr = pointsPtr + i;
            int index = i;
            currentPtr->position = vertices[index];
            Vector3 a = currentPtr->position;
            //currentPtr->tangent = tangents[index];
            //currentPtr->normal = normals[index];
            ref BoneWeight bone = ref weights[index];
            currentPtr->boneWeight = new Vector4(bone.weight0, bone.weight1, bone.weight2, bone.weight3);
            currentPtr->boneIndex = new Vector4Int(bone.boneIndex0, bone.boneIndex1, bone.boneIndex2, bone.boneIndex3);
            currentPtr->uv = uv[index];
        }
        verticesBuffer = new ComputeBuffer(allSkinPoints.Length, sizeof(SkinPoint));
        verticesBuffer.SetData(allSkinPoints);
        allSkinPoints.Dispose();

        for (int i = 0; i < characterPoints.Length; i++)
        {
            animStates.Add(UnityEngine.Random.Range(0, animTex.width - 1e-4f));
        }
        objBuffer.SetData(animStates.ToArray());
    }

    public void Update()
    {
        targetMaterial.SetVector(ShaderIDs._ModelBones, new Vector2(bindPoseCount, verticesBuffer.count));
        targetMaterial.SetVector(ShaderIDs._TimeVar, new Vector4(framePerSecond, animTex.width - 1e-4f));
        targetMaterial.SetBuffer(ShaderIDs.objBuffer, objBuffer);
        targetMaterial.SetTexture(ShaderIDs._AnimTex, animTex);
        targetMaterial.SetBuffer(ShaderIDs.bindBuffer, bindBuffer);
        targetMaterial.SetBuffer(ShaderIDs.verticesBuffer, verticesBuffer);
    }

    public void OnGUI()
    {
        GUI.Label(new Rect((Screen.width - 100) / 2, 50, 200, 100), "VFAnimation");
    }
}