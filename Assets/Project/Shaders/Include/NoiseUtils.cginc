#define PI 3.141592653589793

float rand(float x)
{
    return frac(sin(x) * 43758.5453);
}

float rand(float2 co)
{
    return frac(sin(dot(co.xy, float2(12.9898, 78.233))) * 43758.5453);
}

float rand(float3 co)
{
    return frac(sin(dot(co.xyz, float3(12.9898, 78.233, 56.787))) * 43758.5453);
}

float noise(float3 pos)
{
    float3 ip = floor(pos);
    float3 fp = smoothstep(0, 1, frac(pos));
    float4 a = float4(
        rand(ip + float3(0, 0, 0)),
        rand(ip + float3(1, 0, 0)),
        rand(ip + float3(0, 1, 0)),
        rand(ip + float3(1, 1, 0)));
    float4 b = float4(
        rand(ip + float3(0, 0, 1)),
        rand(ip + float3(1, 0, 1)),
        rand(ip + float3(0, 1, 1)),
        rand(ip + float3(1, 1, 1)));

    a = lerp(a, b, fp.z);
    a.xy = lerp(a.xy, a.zw, fp.y);
    return lerp(a.x, a.y, fp.x);
}

float perlin(float3 pos)
{
    return
        (noise(pos) * 32 +
         noise(pos * 2) * 16 +
         noise(pos * 4) * 8 +
         noise(pos * 8) * 4 +
         noise(pos * 16) * 2 +
         noise(pos * 32)) / 63;
}

float3 Pnoise(float3 vec)
{
    float x = perlin(vec);

    float y = perlin(float3(
        vec.y + 31.416,
        vec.z - 47.853,
        vec.x + 12.793
    ));

    float z = perlin(float3(
        vec.z - 233.145,
        vec.x - 113.408,
        vec.y - 185.31
    ));

    return float3(x, y, z);
}

