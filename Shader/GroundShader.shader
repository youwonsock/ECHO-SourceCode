Shader "Unlit/GroundShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _PlayerEchoColor ("Player Echo Color", Color) = (1,1,1,1)
        _EnemyEchoColor ("Enemy Echo Color", Color) = (1,1,1,1)
        _EchoWidth ("Echo Width", float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM


            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float4 _Color;
            float4 _PlayerEchoColor;
            float4 _EnemyEchoColor;
            float _EchoWidth;

            float _EchoRadius[25];
            float4 _EchoPosition[25];
                
            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 worldPos : POS;

            };



            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);

                return o;
            }

            fixed4 frag (v2f info) : SV_Target
            {
                [unroll(25)]
                for(int i = 0; i < 25; i++)
				{
                    float dis = distance(_EchoPosition[i], info.worldPos);

                    if(dis > (_EchoRadius[i] - _EchoWidth) && dis < _EchoRadius[i])
                    {
                        float ratio = ((dis - (_EchoRadius[i] - _EchoWidth)) / _EchoWidth) * 0.5;

                        if(_EchoPosition[i].w == 0)
						{
							return _EnemyEchoColor * ratio;
						}
						else
						{
							return _PlayerEchoColor * ratio;
						}
				    }
				}
                
                return _Color;
            }
            ENDCG
        }
    }
}
