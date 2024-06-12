Shader"Custom/MoebiusSkyboxShader"
{
    Properties
    {
        _TopColor ("Top Color", Color) = (0.5,0.5,1,1)
        _BottomColor ("Bottom Color", Color) = (1,0.5,0.5,1)
        _GradientPosition ("Gradient Position", Range(0, 1)) = 0.5
        _StarsTex ("Stars Texture", 2D) = "white" {}
        _StarsTint ("Stars Tint", Color) = (1,1,1,1)
        _StarsIntensity ("Stars Intensity", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "Queue" = "Background" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
#include "UnityCG.cginc"

struct appdata_t
{
    float4 vertex : POSITION;
};

struct v2f
{
    float4 pos : SV_POSITION;
    float3 worldPos : TEXCOORD0;
};

sampler2D _StarsTex;
float4 _TopColor;
float4 _BottomColor;
float4 _StarsTint;
float _StarsIntensity;
float _GradientPosition;

v2f vert(appdata_t v)
{
    v2f o;
    o.pos = UnityObjectToClipPos(v.vertex);
    o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
    return o;
}

fixed4 frag(v2f i) : SV_Target
{
    float t = saturate(i.worldPos.y + _GradientPosition - 0.5);
    fixed4 color = lerp(_BottomColor, _TopColor, t);

    fixed4 stars = tex2D(_StarsTex, i.worldPos.xy);
    stars.rgb *= _StarsTint.rgb;
    stars.a *= _StarsIntensity;

    color.rgb += stars.rgb * stars.a;
    return color;
}
            ENDCG
        }
    }
FallBack"Diffuse"
}
