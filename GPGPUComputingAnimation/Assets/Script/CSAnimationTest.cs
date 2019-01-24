using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public unsafe class CSAnimationTest : TestBase
{
    const int AnimationUpdateKernel = 0;
    const int BoneUpdateKernel = 1;
    const int SkinUpdateKernel = 2;

    private ComputeBuffer objBuffer;
    private ComputeBuffer bonesBuffer;
    private ComputeBuffer verticesBuffer;
    private ComputeBuffer resultBuffer;
    private ComputeBuffer bindBuffer;
    private int[] _ModelBones = new int[2];
    private int bindPoseCount;

    public Texture2D animTex;
    public Mesh targetMesh;
    public int framePerSecond = 30;
    public ComputeShader gpuSkinShader;
    public Material targetMaterial;

    void Awake()
    {
        objBuffer = new ComputeBuffer(characterPoints.Length, sizeof(float));
        Matrix4x4[] bindPosesArray = targetMesh.bindposes;
        bindPoseCount = bindPosesArray.Length;
        bonesBuffer = new ComputeBuffer(bindPoseCount * characterPoints.Length, sizeof(Matrix3x4));
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

       // Debug.LogError("triangles   " + triangles.Length);
       // Debug.LogError("vertices   " + vertices.Length);
        //Debug.LogError("normals   " + normals.Length);
        //Debug.LogError("tangents   " + tangents.Length);
      //  Debug.LogError("weights   " + weights.Length);
      //  Debug.LogError("uv   " + uv.Length);

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
        resultBuffer = new ComputeBuffer(allSkinPoints.Length * characterPoints.Length, sizeof(Point));
        allSkinPoints.Dispose();

        List<float> allAnimState = new List<float>();
        for (int i = 0; i < characterPoints.Length; ++i)
        {
            allAnimState.Add(Random.Range(0, animTex.width - 1e-4f));
        }
        objBuffer.SetData(allAnimState.ToArray());
        allAnimState = null;
    }

    public void Update()
    {
        ComputeShader shader = gpuSkinShader;
        int* pointer = stackalloc int[] { bindPoseCount, verticesBuffer.count };
        _ModelBones.CopyFrom(pointer, 2);
        shader.SetInts(ShaderIDs._ModelBones, _ModelBones);
        shader.SetVector(ShaderIDs._TimeVar, new Vector4(Time.deltaTime * framePerSecond, animTex.width - 1e-4f));
        shader.SetBuffer(AnimationUpdateKernel, ShaderIDs.objBuffer, objBuffer);
        shader.SetBuffer(BoneUpdateKernel, ShaderIDs.objBuffer, objBuffer);
        shader.SetBuffer(BoneUpdateKernel, ShaderIDs.bonesBuffer, bonesBuffer);
        shader.SetTexture(BoneUpdateKernel, ShaderIDs._AnimTex, animTex);
        shader.SetBuffer(BoneUpdateKernel, ShaderIDs.bindBuffer, bindBuffer);
        shader.SetBuffer(SkinUpdateKernel, ShaderIDs.bonesBuffer, bonesBuffer);
        shader.SetBuffer(SkinUpdateKernel, ShaderIDs.verticesBuffer, verticesBuffer);
        shader.SetBuffer(SkinUpdateKernel, ShaderIDs.resultBuffer, resultBuffer);
        shader.SetBuffer(SkinUpdateKernel, ShaderIDs.objBuffer, objBuffer); //Debug
        const int THREAD = 256;

        ComputeShaderUtility.Dispatch(shader, AnimationUpdateKernel, characterPoints.Length, THREAD);
        ComputeShaderUtility.Dispatch(shader, BoneUpdateKernel, bonesBuffer.count, THREAD);
        ComputeShaderUtility.Dispatch(shader, SkinUpdateKernel, resultBuffer.count, THREAD);

        targetMaterial.SetBuffer(ShaderIDs.resultBufferA, resultBuffer);
        Vector2 mb = new Vector2(bindPoseCount, verticesBuffer.count);
        targetMaterial.SetVector(ShaderIDs.modelBonesA, mb);
    }

    public void OnGUI()
    {
        GUI.Label(new Rect((Screen.width - 100) / 2, 50, 200, 100), "CSAnimation");
    }
}