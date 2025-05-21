Shader "Custom/Rotation"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ScrollSpeed ("Scroll Speed", Float) = 0.05
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

            sampler2D _MainTex;
            float _ScrollSpeed;
            float4 _MainTex_ST;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float2 uvCenter : TEXCOORD1;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uvCenter = v.uv * 2.0 - 1.0; // -1 ~ 1 기준 중심 좌표
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float offset = frac(_Time.y * _ScrollSpeed);
                float2 uv = i.uv;
                uv.x = frac(uv.x + offset); // 반복 스크롤 처리

                // 원형 마스크: 중심으로부터 거리 계산
                float dist = length(i.uvCenter);
                if (dist > 1.0) discard; // 원 바깥은 그리지 않음

                return tex2D(_MainTex, uv);
            }
            ENDCG
        }
    }
}