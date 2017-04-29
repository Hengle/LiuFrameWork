// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Shader/Diffuse_Test"{
	Properties{
		_MDiffuseColor("DiffuseColor", Color) = (1,1,1,1)
	}
	SubShader{
		Pass {
			Tags {  }
			CGPROGRAM
			
			
			#pragma vertex vert
			#pragma fragment frag
			#include "Lighting.cginc" 

			struct a2v{
				float4 vertex : POSITION; 
				float4 normal : NORMAL;
			} ;

			struct  v2f{
				float4 pos :SV_POSITION;
				fixed4 color : COLOR;
			} ;

			fixed4 _MDiffuseColor;
			v2f vert(a2v v){
				v2f o;
				//世界坐标
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex );

				//光照方向
				fixed3  lightdir = normalize(_WorldSpaceLightPos0.xyz);

				//世界法线
				fixed3  WorldNormal = normalize(mul(v.normal, (float3x3)unity_WorldToObject));

				o.color = _MDiffuseColor * _LightColor0 * saturate(dot(WorldNormal,lightdir)) + UNITY_LIGHTMODEL_AMBIENT;
				
				return o;
			}


			fixed4 frag(v2f v): SV_TARGET{
				return v.color;
			}
			ENDCG
		}
		
	}
	FallBack  "Diffuse"
}