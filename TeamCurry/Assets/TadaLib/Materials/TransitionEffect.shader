Shader "Unlit/TransitionEffect"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) = (0,0,0,1)
        _Progress("Progress", Range(0, 1)) = 0
    }

        SubShader
        {
            Tags
            {
                "Queue" = "Transparent"
                "IgnoreProjector" = "True"
                "RenderType" = "Transparent"
                "PreviewType" = "Plane"
                "CanUseSpriteAtlas" = "True"
            }

            Cull Off
            Lighting Off
            ZWrite Off
            ZTest[unity_GUIZTestMode]
            Fog{ Mode Off }
            Blend SrcAlpha OneMinusSrcAlpha

            Pass
            {
                CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag
    #include "UnityCG.cginc"

                struct appdata_t
                {
                    float4 vertex   : POSITION;
                    float2 texcoord : TEXCOORD0;
                };

                struct v2f
                {
                    float4 vertex   : SV_POSITION;
                    half2 texcoord  : TEXCOORD0;
                };

                fixed4 _Color;
                fixed _Progress;
                sampler2D _MainTex;

                // 頂点シェーダーの基本
                v2f vert(appdata_t IN)
                {
                    v2f OUT;
                    OUT.vertex = UnityObjectToClipPos(IN.vertex);
                    OUT.texcoord = IN.texcoord;
    #ifdef UNITY_HALF_TEXEL_OFFSET
                    OUT.vertex.xy += (_ScreenParams.zw - 1.0) * float2(-1,1);
    #endif
                    return OUT;
                }

                // 通常のフラグメントシェーダー
                fixed4 frag(v2f IN) : SV_Target
                {
                    //fixed4 col = tex2D(_MainTex, IN.texcoord);
                    //col.a = saturate(col.a + (_Progress * 2 - 1));
                    //return col;

                    half alpha = tex2D(_MainTex, IN.texcoord).a;
                    alpha = saturate(alpha + (_Progress * 2 - 1));
                    return fixed4(_Color.r, _Color.g, _Color.b, alpha);
                }
                ENDCG
            }
        }

            FallBack "UI/Default"
}
