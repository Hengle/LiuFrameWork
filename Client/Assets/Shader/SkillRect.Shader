// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "SkillRange/Rect" {
	Properties {
		_MainTex("texture ", 2D) = "White" {}
		_Color("Color", Color) = (1,1,1,1)
		_Color2("Color2", Color) = (1,1,1,1)
		_Color3("Color3", Color) = (1,1,1,1)
		_ScrollX("Speed", float ) = 1.0 
	}
	SubShader {
		Tags { "Queue" = "Transparent" "IgnoreProjector" = "true" "RenderType"="Transparent"}
		Pass {
			Blend  SrcAlpha OneMinusSrcAlpha
			ZWrite off 
			Lighting off
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			fixed4 _Color;
			fixed4 _Color2;
			fixed4 _Color3;
			sampler2D _MainTex;
			float4 _MainTex_ST;

			struct  a2v{
				float4 vertex :POSITION;
				float4 texcoord: TEXCOORD0; 
			} ;

				struct v2f{
				float4 pos:SV_POSITION;
				float4 Objectpos:TEXCOORD0  ;
				fixed2 uv:TEXCOORD1  ;
			};

			float _ScrollX;

			v2f vert(  a2v  v){
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);	
				o.Objectpos = v.vertex;
				o.uv.xy = v.texcoord.xy ;
				return o;

			}

			fixed4 frag(v2f i):COLOR{
				fixed4 color = _Color;
				color.a = abs(i.Objectpos.x);
				float n = (_ScrollX * _Time.y) % 1.3;
				float n2 = (_ScrollX * _Time.y) % 0.6;
				
				
				if(n > abs(i.Objectpos.z)  && abs(i.Objectpos.z) > n - 0.2)
				{
					color = lerp(color, _Color2, _Color2.a * abs(0.6 -(abs(n - 0.6)))*4);
				}

				if(abs(i.Objectpos.x) >= 0.49){
					color = _Color3;
				}
				
				
				return color;
			}

			ENDCG 
		}
	}

	Fallback "Diffuse"
} 