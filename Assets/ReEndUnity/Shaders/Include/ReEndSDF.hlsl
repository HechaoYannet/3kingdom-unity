#ifndef REEND_SDF_INCLUDED
#define REEND_SDF_INCLUDED

// ── Signed Distance Field primitives for UI shaders ──
// 约定：正值 = 内部（可见），负值 = 外部（裁剪）
// aspectRatio = width / height，用于将 UV 缩放到正方形空间，使对角线为 45°

/// 定向切角矩形 SDF — 仅切右上 (RT) 和 左下 (LB) 两个角
/// uv: [0,1] 范围
/// cornerSize: 切角大小（以短边为基准的 UV 空间比例）
/// aspectRatio: 矩形宽高比 (width / height)
float sdCutCornerRT_LB(float2 uv, float cornerSize, float aspectRatio)
{
    // 缩放到正方形空间：宽矩形拉伸 X，高矩形拉伸 Y
    float2 scale = aspectRatio >= 1.0
        ? float2(aspectRatio, 1.0)
        : float2(1.0, 1.0 / aspectRatio);

    float2 p = (uv - 0.5) * scale;
    float2 halfSize = 0.5 * scale;

    // 基础矩形距离
    float2 q = abs(p);
    float rectDist = min(halfSize.x - q.x, halfSize.y - q.y);

    // 右上角 (RT): 对角线在 p.x + p.y = halfSize.x + halfSize.y - cornerSize
    float rtDist = 1.0e10;
    if (p.x > 0.0 && p.y > 0.0)
        rtDist = (halfSize.x + halfSize.y - cornerSize) - (p.x + p.y);

    // 左下角 (LB): 对角线在 p.x + p.y = -(halfSize.x + halfSize.y - cornerSize)
    float lbDist = 1.0e10;
    if (p.x < 0.0 && p.y < 0.0)
        lbDist = (p.x + p.y) + (halfSize.x + halfSize.y - cornerSize);

    return min(min(rectDist, rtDist), lbDist);
}

/// 非对称切角 — 仅切右上 (RT)，用于 OperatorCard 头像裁剪
float sdCutCornerRT(float2 uv, float cornerSize, float aspectRatio)
{
    float2 scale = aspectRatio >= 1.0
        ? float2(aspectRatio, 1.0)
        : float2(1.0, 1.0 / aspectRatio);

    float2 p = (uv - 0.5) * scale;
    float2 halfSize = 0.5 * scale;

    float2 q = abs(p);
    float rectDist = min(halfSize.x - q.x, halfSize.y - q.y);

    float rtDist = 1.0e10;
    if (p.x > 0.0 && p.y > 0.0)
        rtDist = (halfSize.x + halfSize.y - cornerSize) - (p.x + p.y);

    return min(rectDist, rtDist);
}

/// 菱形 SDF（45度旋转正方形）。
/// ⚠ 例外约定：负值 = 内部（标准 SDF 约定），与本文件其他函数相反。
/// 使用者需用 smoothstep(softness, 0.0, d) 而非 smoothstep(0.0, softness, d)。
/// uv: [0,1] 空间中的采样点
/// center: [0,1] 空间中菱形的中心
/// radius: 菱形的半对角线长度（UV 空间）
float sdDiamond(float2 uv, float2 center, float radius)
{
    float2 p = uv - center;
    return (abs(p.x) + abs(p.y)) - radius;
}

#endif
