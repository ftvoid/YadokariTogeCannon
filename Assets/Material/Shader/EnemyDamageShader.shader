Shader "Custom/EnemyDamageShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows alphatest:_Cutoff

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input 
		{
			float2 uv_MainTex;
		};

		float _Metallic;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			// Albedo comes from a texture tinted by color
			fixed4 c = _Color;
			o.Albedo = c.rgb;

			fixed4 col = tex2D(_MainTex, IN.uv_MainTex);
			float colorInc = col.r + col.g + col.b;
			colorInc += 1.0f;
			//フェードの進行度によって透過度を変更するIF文
			if (_Metallic <= colorInc)
			{
				//現在の色からフェード具合を引いた値を透過度に				
				o.Alpha = colorInc - _Metallic;
			}
			else
			{
				o.Alpha = 0.0f;
			}
		}
		ENDCG
	}
	FallBack "Diffuse"
}
