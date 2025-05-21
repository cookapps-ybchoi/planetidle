Shader "Custom/CircleOutline"
{
    Properties
    {
        _Radius("Radius", Float) = 0.4
        _Thickness("Thickness", Float) = 0.02
        _Color("Color", Color) = (0, 1, 1, 1)
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float _Radius;
            float _Thickness;
            float4 _Color;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                // UV 영역을 더 넓게 잡음
                float scaleFactor = 2.0; // 예: 4.0이면 -2~2까지 영역 커짐
                o.uv = (v.uv - 0.5) * scaleFactor;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float dist = length(i.uv);
                float border = 0.005;

                float outer = _Radius;
                float inner = _Radius - _Thickness;

                // 선 경계에서만 그리기
                float edgeOuter = smoothstep(outer + border, outer - border, dist);
                float edgeInner = smoothstep(inner + border, inner - border, dist);
                float alpha = edgeOuter * (1.0 - edgeInner);

                return fixed4(_Color.rgb, _Color.a * alpha);
            }
            ENDCG
        }
    }
}
