#ifndef REEND_SDF_INCLUDED
#define REEND_SDF_INCLUDED

// ── Signed Distance Field primitives for UI shaders ──
// 约定：正值 = 内部（可见），负值 = 外部（裁剪）

/// 切角矩形 SDF（八边形）。正值 = 内部，负值 = 外部。
/// uv: [0,1] 范围
/// halfSize: 矩形的半宽半高（与 uv 同空间）
/// cornerSize: 切角大小（与 halfSize 同空间）
float sdCutCorner(float2 uv, float2 halfSize, float cornerSize)
{
    // 将 uv 从 [0,1] 映射到 [-halfSize, +halfSize]
    float2 p = (uv - 0.5) * 2.0 * halfSize;
    float2 q = abs(p);

    // 矩形距离：到最近边的距离（正值 = 内部）
    float rectDist = min(halfSize.x - q.x, halfSize.y - q.y);

    // 切角距离：45度斜面的距离（正值 = 切角内侧）
    // 切角线方程：q.x + q.y = halfSize.x + halfSize.y - cornerSize
    float chamferDist = (halfSize.x + halfSize.y - cornerSize) - (q.x + q.y);

    // 两者取最小值 = 切角矩形的交集
    return min(rectDist, chamferDist);
}

/// 标准矩形 SDF。正值 = 内部，负值 = 外部。
float sdRect(float2 uv, float2 halfSize)
{
    float2 p = (uv - 0.5) * 2.0 * halfSize;
    float2 q = abs(p);
    return min(halfSize.x - q.x, halfSize.y - q.y);
}

/// 菱形 SDF（45度旋转正方形）。正值 = 内部，负值 = 外部。
float sdDiamond(float2 uv, float size)
{
    float2 p = (uv - 0.5) * size;
    return (size * 0.5 - (abs(p.x) + abs(p.y))) * 0.7071;
}

#endif
