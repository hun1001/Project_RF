Shader "Custom/ToonShader_Basic"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
		_DivideLevel("Divide Level", Int) = 5
    }
    SubShader
	{
		Pass
		{
			Tags { "RenderType" = "Opaque" "LightMode" = "ForwardBase"}
			LOD 200
			//Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

			sampler2D _MainTex;
			int _DivideLevel;
			fixed4 _Color;

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 worldNormal : NORMAL;
				SHADOW_COORDS(1)
			};

			v2f vert(appdata_base v)
			{
				v2f o;

				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				o.worldNormal = UnityObjectToWorldNormal(v.normal);

				TRANSFER_SHADOW(o);

				return o;
			}


			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);

				half3 ambient = ShadeSH9(half4(i.worldNormal, 1));
				half diffuse = max(0, dot(i.worldNormal, _WorldSpaceLightPos0.xyz));

				fixed shadow = SHADOW_ATTENUATION(i);

				float3 lighting = diffuse * shadow;
				lighting = ceil(lighting * _DivideLevel) / _DivideLevel;
				lighting *= _LightColor0.rgb;
				lighting += ambient;

				col.rgb *= _Color * lighting;
				return col;
			}
			ENDCG
		}
	}
    FallBack "Diffuse"
}