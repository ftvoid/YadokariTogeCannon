Shader "UI/TextureMove"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_UVMoveX("MoveX",Range(0,1)) = 0.0
		_UVMoveY("MoveY",Range(0,1)) = 0.0
	}

	SubShader
	{
		// No culling or depth
		Cull Off
		Lighting Off
		//ZWrite Off
		ZTest[unity_GUIZTestMode]
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
			float _UVMoveX;
			float _UVMoveY;

			fixed4 frag (v2f i) : SV_Target
			{
				float uvX = i.uv.x + _UVMoveX;
				float uvY = i.uv.y + _UVMoveY;
				if (uvX > 1)
				{
					uvX = uvX - 1;
				}
				if (uvY > 1)
				{
					uvY = uvY - 1;
				}
				float2 uv = float2(uvX, uvY);

				fixed4 col = tex2D(_MainTex,uv);
				// just invert the colors
				col.rgb = col.rgb;
				return col;
			}
			ENDCG
		}
	}
}
