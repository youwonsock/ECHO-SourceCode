Shader "Unlit/NumberShader"
{
  Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _PlayerEchoColor ("Player Echo Color", Color) = (1,1,1,1)
        _EnemyEchoColor ("Enemy Echo Color", Color) = (1,1,1,1)
        _AfterImageDistance ("AfterImageDistance", float) = 1.0
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
            float _AfterImageDistance;

            float _EchoRadius[25];
            float4 _EchoPosition[25];
                
            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };



            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = _Color;

                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                float shortestDis = 9999;

                [unroll(25)]
                for(int i = 0; i < 25; i++)
				{
                    float dis = distance(_EchoPosition[i], worldPos);

                    if(shortestDis > dis && dis < _EchoRadius[i])
                    {
                        shortestDis = dis;
                        float ratio = (dis - (_EchoRadius[i] - _AfterImageDistance)) / _AfterImageDistance * 0.5;


                        if(_EchoPosition[i].w == 0)
						{
							o.color = _EnemyEchoColor * ratio;
						}
						else
						{
							o.color = _PlayerEchoColor * ratio;
						}
				    }
				}

                return o;
            }

            fixed4 frag (v2f info) : SV_Target
            {
                return info.color;
            }
            ENDCG
        }
    }
}

