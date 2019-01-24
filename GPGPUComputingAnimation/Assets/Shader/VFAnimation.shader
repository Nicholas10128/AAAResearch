// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/VFAnimation"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100
		Pass
		{
			CGPROGRAM
			// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it uses non-square matrices
			#pragma exclude_renderers gles
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 5.0
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"

			struct SkinPoint
			{
				float3 position;
				//float3 normal;
				//float4 tangent;
				float2 uv;
				float4 boneWeight;
				uint4 boneIndex;
			};

			struct Point {
				float3 vertex;
				//float4 tangent;
				//float3 normal;
				float2 texcoord;
				//uint objIndex;
			};

			float2 _ModelBones;
			float2 _TimeVar;
			Texture2D<float4> _AnimTex;
			StructuredBuffer<float3x4> bindBuffer;
			StructuredBuffer<float> objBuffer;
			StructuredBuffer<SkinPoint> verticesBuffer;

			sampler2D _MainTex;
			float4 _MainTex_ST;

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			float4x4 BoneUpdate(uint boneIndex, uint currentObjectIndex)
			{
				float frame = fmod(_Time.y * _TimeVar.x + objBuffer[currentObjectIndex], _TimeVar.y);
				float4x4 bonesMatrix0 = float4x4(_AnimTex[uint2((uint)frame, boneIndex * 3)]
					, _AnimTex[uint2((uint)frame, boneIndex * 3 + 1)]
					, _AnimTex[uint2((uint)frame, boneIndex * 3 + 2)]
					, float4(0, 0, 0, 1));
				uint nextValue = (uint)min(frame + 1, _TimeVar.y);
				float4x4 bonesMatrix1 = float4x4(_AnimTex[uint2(nextValue, boneIndex * 3)]
					, _AnimTex[uint2(nextValue, boneIndex * 3 + 1)]
					, _AnimTex[uint2(nextValue, boneIndex * 3 + 2)]
					, float4(0, 0, 0, 1));
				bonesMatrix0 = lerp(bonesMatrix0, bonesMatrix1, frac(frame));

				float4x4 bindMatrix = float4x4(bindBuffer[boneIndex], float4(0, 0, 0, 1));

				return mul(bonesMatrix0, bindMatrix);
			}

			Point GPUSkinning(uint vertexID, uint instanceID)
			{
				Point pt;
				uint offset = _ModelBones.x * instanceID;
				SkinPoint skinPt = verticesBuffer[vertexID];
				float3x4 combineTex = mul(BoneUpdate(skinPt.boneIndex.x, instanceID), skinPt.boneWeight.x) +
					mul(BoneUpdate(skinPt.boneIndex.y, instanceID), skinPt.boneWeight.y) +
					mul(BoneUpdate(skinPt.boneIndex.z, instanceID), skinPt.boneWeight.z) +
					mul(BoneUpdate(skinPt.boneIndex.w, instanceID), skinPt.boneWeight.w);
				pt.vertex = mul(combineTex, float4(skinPt.position, 1));
				//pt.normal = mul(combineTex, float4(skinPt.normal, 0));
				//pt.tangent.xyz = mul(combineTex, float4(skinPt.tangent.xyz, 0));
				//pt.tangent.w = skinPt.tangent.w;
				pt.texcoord = skinPt.uv;
				//pt.objIndex = 0;
				return pt;
			}

			struct appdata
			{
				uint vertexID : SV_VERTEXID;
				uint instanceID : SV_INSTANCEID;
				float2 uv : TEXCOORD0;
			};

			v2f vert(appdata v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				Point result = GPUSkinning(v.vertexID, v.instanceID);
				o.vertex = UnityObjectToClipPos(float4(result.vertex, 1));
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				return tex2D(_MainTex, i.uv);
			}
			ENDCG
		}
	}
}
