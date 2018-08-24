// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/PaththoughEffect" {
	Properties {
		_Diffuse("Diffuse", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		Pass
	    {
		    Tags{ "LightMode" = "ForwardBase" }        // 只有设置了正确的LightMode，才能访问一些Unity提供的内置变量，如：_LightColor0

		    CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

		    struct appdata
	        {
		        float4 vertex : POSITION;
		        float3 normal : NORMAL;
	        };

	        struct v2f
	        {
		        float4 vertex : SV_POSITION;
		        float3 worldNormal : NORMAL;
	        };

	        float4 _Diffuse;

	        v2f vert(appdata v)
	        {
		        v2f o;
		        o.vertex = UnityObjectToClipPos(v.vertex);
		        float3 wn = mul(v.normal, (float3x3)unity_WorldToObject);
		        o.worldNormal = normalize(wn);
		        return o;
	        }

	        fixed4 frag(v2f i) : SV_Target
	        {
		        float3 lightDir = normalize(_WorldSpaceLightPos0);
		        float4 diffuseColor = _Diffuse * _LightColor0 * max(0, dot(lightDir, i.worldNormal));
		        return UNITY_LIGHTMODEL_AMBIENT + diffuseColor;
	        }

		    ENDCG
	    }
	
	}
	FallBack "Diffuse"
}
