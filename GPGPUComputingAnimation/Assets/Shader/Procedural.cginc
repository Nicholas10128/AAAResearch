#ifndef PROCEDURAL
#define PROCEDURAL

#define CLUSTERCLIPCOUNT 256
#define CLUSTERVERTEXCOUNT 384
#define PLANECOUNT 6
    struct PropertyValue
    {
        float _SpecularIntensity;
        float _MetallicIntensity;
        float4 _EmissionColor;
        float _Occlusion;
        float _Glossiness;
        float4 _Color;
        int3 textureIndex;
    };
struct Point{
    float3 vertex;
    //float4 tangent;
    //float3 normal;
    float2 texcoord;
	//uint objIndex;
};
#ifndef COMPUTESHADER		//Below is Not for compute shader
StructuredBuffer<Point> verticesBuffer;
StructuredBuffer<uint> resultBuffer;
static const uint IndexArray[6] = 
{
	0,	1,	2,
	1,	3,	2
};
inline Point getVertex(uint vertexID, uint instanceID)
{
    instanceID = resultBuffer[instanceID];
	uint vertID = instanceID * CLUSTERCLIPCOUNT;
	uint triangleCount = IndexArray[vertexID % 6];
	vertID += vertexID / 6 * 4 + triangleCount;
	return verticesBuffer[vertID];
}
#endif
#endif