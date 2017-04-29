// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Shader/Diffuse_Frag_Test"{
	Properties{
		_mDiffuseColor("DiffuseColor", Color) = (1,1,1,1)
	}
	SubShader {
		Pass {
			Tags { "LightMode" = "ForwardBase" }
			CGPROGRAM
			#pragma vertex vert;
			#pragma fragment frag;
			##include "Lighting.cginc" 
			float _mDiffuseColor;
			struct a2v{
				float4 vertex: POSITION ;
				float3 normal:NORMAL;
			} ;

			struct v2f{
				float4 pos : SV_POSITION ;
				float3 worldNormal: NOMAL;

			} ;

			v2f vert(a2v v){
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
				o.worldNormal = normalize(mul(v.normal, (float3x3)unity_WorldToObject));
				return 0;
			}

			fixed4 frag(v2f v){

				//获取光照方向
				fixed3 lightdir = normalize(WorldSpaceLightDir.xyz)

				fixed4 color = _LightColor0 * _mDiffuseColor * saturate(dot(v.worldNormal , lightdir));
				color = color + UNITY_LIGHTMODEL_AMBIENT; 

				return color;
				// c = (clihgt * mdiffuse * max(0, n * v))
			}


			ENDCG
		}
	}
}