// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Button" {
	Properties {
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
	    _Color("Tint", Color) = (1,1,1,1)

		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255

		_ColorMask("Color Mask", Float) = 15
	}
	SubShader {
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}
		LOD 200

		Stencil
		{
			Ref[_Stencil]
			Comp[_StencilComp]
			Pass[_StencilOp]
			ReadMask[_StencilReadMask]
			WriteMask[_StencilWriteMask]
		}
		
			Cull Off
			Lighting Off
			ZWrite Off
			ZTest[unity_GUIZTestMode]
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask[_ColorMask]

		Pass
		{

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		//#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		//#pragma target 3.0

        #pragma vertex vert  
        #pragma fragment frag  
        #include "UnityCG.cginc"  

		struct appdata_t
		{
			float4 vertex   : POSITION;
			float4 color    : COLOR;
			float2 texcoord : TEXCOORD0;
		};

		struct v2f
		{
			float4 vertex   : SV_POSITION;
			fixed4 color : COLOR;
			half2 texcoord  : TEXCOORD0;
		};

		int _type;
		fixed4 _Color;

		v2f vert(appdata_t IN)
		{
			v2f OUT;
			OUT.vertex = UnityObjectToClipPos(IN.vertex);
			OUT.texcoord = IN.texcoord;
#ifdef UNITY_HALF_TEXEL_OFFSET  
			OUT.vertex.xy += (_ScreenParams.zw - 1.0)*float2(-1, 1);
#endif  
			OUT.color = IN.color * _Color;
			return OUT;
		}

		sampler2D _MainTex;

		fixed4 frag(v2f IN) : SV_Target
		{
			half4 color = tex2D(_MainTex, IN.texcoord) * IN.color;
			clip(color.a - 0.01);

			if (_type == 1)
			{
				color = color * 1.6f;
			}
			else if (_type == 2)
			{
				color = color * 0.8f;
			}
			else if (_type == 3)
			{
				float grey = dot(color.rgb, fixed3(0.22, 0.707, 0.071));
				color = half4(grey,grey,grey,color.a);
			}
			return color;
		}

		struct Input {
			float2 uv_MainTex;
		};

		/*half _Glossiness;
		half _Metallic;
		fixed4 _Color;*/

		
		//UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		//UNITY_INSTANCING_CBUFFER_END

		//void surf (Input IN, inout SurfaceOutputStandard o) {
		//	// Albedo comes from a texture tinted by color
		//	fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
		//	o.Albedo = c.rgb;
		//	// Metallic and smoothness come from slider variables
		//	o.Metallic = _Metallic;
		//	o.Smoothness = _Glossiness;
		//	o.Alpha = c.a;
		//}
		ENDCG
		}
	}
	FallBack "Diffuse"
}
