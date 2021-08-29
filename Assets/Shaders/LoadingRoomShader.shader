Shader "Unlit/LoadingRoomShader"
{
    Properties
    {
        _Color ("Color", Color) = (0,0,0,1)
        _Opacity ("Opacity", Range (0.0, 1.0)) = 1
    }
    SubShader
    {
        Tags {"Queue"="AlphaTest" "IgnoreProjector"="True"}
        Cull Front
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            float4 _Color;
            float _Opacity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 color = _Color;
                color.w = _Opacity;
                return color;
            }
            ENDCG
        }
    }
}
