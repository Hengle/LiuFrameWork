Shader "Shader/Specular_Vertex"{
	Properties{
		_mSpecularColor("SpecularColor", Color) = (1,1,1,1)
		_mGloss("Gloss", float ) = 1
	}
	SubShader {
		Pass {
			Tags { "LightMode" = "ForwardBase" }
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "Lighting.cginc"

			struct a2v{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			}  ;

			struct v2f{
				float4 pos : SV_POSITION;
				fixed3 color : COLOR;
			} 

			v2f vert(a2v v){
				//高光反射模型
				//C = Clight . Mspecular * max(0,v . r) mgloss
			}

			fixed4 frag(v2f v){

			}
			ENDCG
		}
	}
	
	FallBack  "Specular"
}