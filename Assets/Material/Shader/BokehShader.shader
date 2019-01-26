Shader "UI/BokehShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_SceneChangeTex("SceneTexture", 2D) = "white" {}
	}
	SubShader
	{
		//カリング処理のオフ
		Cull Off
		//ライティング無効
		Lighting Off
		ZWrite Off
		ZTest[unity_GUIZTestMode]
		//アルファを使うための設定
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
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _SceneChangeTex;
			float _alpha;

			fixed4 frag (v2f i) : SV_Target
			{
				float4 depth = float4(0,0,0,0);
				float2 uv = float2(0, 0);
				float bokeh_wait = 0.08f;
				for (int j = -3; j <= 3; j++)
				{
					for (int k = -3; k <= 3; k++)
					{
						//float x = (_PixelSizeX * j) + i.uv.x;
						float x = 0.006f*j + i.uv.x;
						float y = 0.006f*k + i.uv.y;
						uv = float2(x, y);
						float absJ = j;
						float absK = k;
						if (absJ < 0)
						{
							absJ *= -1;
						}
						if (absK < 0)
						{
							absK *= -1;
						}
						if ((absK + absJ) == 0)
						{
							depth += tex2D(_SceneChangeTex, uv)*bokeh_wait;
						}
						else
						{
							depth += tex2D(_SceneChangeTex, uv)*(bokeh_wait / (absK + absJ));
						}
					}
				}
				depth.a = depth.a * _alpha;
				return depth;
			}
			ENDCG
		}
	}
}
