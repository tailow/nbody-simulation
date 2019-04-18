Shader "Custom/StarShader"
{
 	SubShader
	{
		Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 100
 
		Cull Off
		ZWrite Off
		Blend One One
 
		Pass
		{
			CGPROGRAM
			#pragma target 5.0
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
 
			uniform StructuredBuffer<float3> position : register(t1);
 
			uniform int offset;
			uniform float4 color = float4(1, 1, 1, 1);

			struct appdata
			{
				float4 vertex : POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
 
			struct v2f {
				float4 pos : SV_POSITION;
				float4 vertex : VERTEX;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
 
			v2f vert(appdata v)
			{
				v2f o;

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
 
				o.pos = mul(UNITY_MATRIX_P, mul(UNITY_MATRIX_MV, float4(0.0, 0.0, 0.0, 1.0)) + float4(v.vertex.x, v.vertex.y, 0.0, 0.0));

				#ifdef UNITY_INSTANCING_ENABLED
				o.pos += float4(position[unity_InstanceID + offset], 0.0f);
				#endif

				o.vertex = v.vertex;
 
				return o;
			}
 
			fixed4 frag(v2f i) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);

				return color;
			}
			ENDCG
		}
	}
}
