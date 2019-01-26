Shader "Custom/OutLineShader"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }

		//UIのためライティングなどを切っています
		//カリング処理のオフ
		Cull Off
		//ライティング無効
		Lighting Off
		ZTest[unity_GUIZTestMode]
		//アルファを使うための設定
		Blend SrcAlpha OneMinusSrcAlpha

		LOD 100

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		fixed4 _Color;

		struct Input
		{
			float2 uv_MainTex;
		};


		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
