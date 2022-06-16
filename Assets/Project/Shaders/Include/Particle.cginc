struct Particle
{
    int active;
    float scale;
    float3 position;
    float3 targetPosition;
    float3 velocity;
    float4 color;
};

struct ParticleData
{
    uint activateTypes;
    float scale;
    float4 targetPosition;
    float3 velocity;
    float4 color;
};