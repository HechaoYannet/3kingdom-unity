#ifndef REEND_SDF_INCLUDED
#define REEND_SDF_INCLUDED

// ── Signed Distance Field primitives for UI shaders ──

/// Returns signed distance to a cut-corner octagon rectangle.
/// positive = inside, negative = outside
float sdCutCorner(float2 uv, float2 halfSize, float cornerSize)
{
    // Normalize uv from [0,1] to [-halfSize, +halfSize]
    float2 p = (uv - 0.5) * 2.0 * halfSize;

    // Reflect into first quadrant
    float2 q = abs(p) - halfSize + cornerSize;

    // Distance to the octagon = max(distance to inner rect, 0) + min(max(q.x, q.y), 0)
    // Actually use the standard rounded-rect SDF with zero radius for the straight parts,
    // and the corner cut creates a 45-degree chamfer.

    // Chamfer distance: distance from point to the 45-degree cut line at each corner
    float2 cornerPos = halfSize - cornerSize;
    float d;

    if (p.x > cornerPos.x && p.y > cornerPos.y)
    {
        // Inside top-right corner region: distance to the 45-degree cut
        float2 c = p - cornerPos;
        d = (c.x + c.y - cornerSize) * 0.7071; // 1/sqrt(2)
    }
    else if (p.x < -cornerPos.x && p.y > cornerPos.y)
    {
        float2 c = float2(-p.x - cornerSize, p.y - cornerSize) * float2(-1, 1);
        d = (c.x + c.y - cornerSize) * 0.7071;
    }
    else if (p.x > cornerPos.x && p.y < -cornerPos.y)
    {
        float2 c = float2(p.x - cornerSize, -p.y - cornerSize) * float2(1, -1);
        d = (c.x + c.y - cornerSize) * 0.7071;
    }
    else if (p.x < -cornerPos.x && p.y < -cornerPos.y)
    {
        float2 c = float2(-p.x - cornerSize, -p.y - cornerSize);
        d = (c.x + c.y - cornerSize) * 0.7071;
    }
    else
    {
        // Inside the axis-aligned region: standard rect SDF
        d = -min(min(halfSize.x - abs(p.x), halfSize.y - abs(p.y)), 0.0);
    }

    return d;
}

/// Standard rect SDF
float sdRect(float2 uv, float2 halfSize)
{
    float2 d = abs((uv - 0.5) * 2.0 * halfSize) - halfSize;
    return -min(max(d.x, d.y), 0.0) + length(max(d, 0.0));
}

/// Diamond SDF (45-degree rotated square)
float sdDiamond(float2 uv, float size)
{
    float2 p = (uv - 0.5) * size;
    return (abs(p.x) + abs(p.y) - size * 0.5) * 0.7071;
}

#endif
