// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Infinity PBR/BOTD/Conversion1_CutoutBackFace"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.3
		_SubsurfaceIntensity("Subsurface Intensity", Range( 0 , 1)) = 0.7
		_DiffuseColor("Diffuse Color", Color) = (1,1,1,1)
		_Diffuse("Diffuse", 2D) = "white" {}
		_Normal("Normal", 2D) = "bump" {}
		_Maskmap("Maskmap", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#pragma target 4.0
		#pragma surface surf StandardCustom keepalpha addshadow fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
		};

		struct SurfaceOutputStandardCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			half3 Transmission;
		};

		uniform sampler2D WindNoise;
		uniform float4 WindConfiguration;
		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform sampler2D _Diffuse;
		uniform float4 _Diffuse_ST;
		uniform float4 _DiffuseColor;
		uniform sampler2D _Maskmap;
		uniform float4 _Maskmap_ST;
		uniform float _SubsurfaceIntensity;
		uniform float _Cutoff = 0.3;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float temp_output_208_0 = (0.0 + (WindConfiguration.x - 0.0) * (1.2 - 0.0) / (1.0 - 0.0));
			float2 temp_cast_0 = (temp_output_208_0).xx;
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 appendResult197 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 _Vector1 = float2(0.2,0.2);
			float2 panner217 = ( 0.2 * _Time.y * temp_cast_0 + ( appendResult197 * _Vector1 ));
			float3 ase_worldNormal = UnityObjectToWorldNormal( v.normal );
			float3 temp_output_192_0 = abs( mul( unity_WorldToObject, float4( ase_worldNormal , 0.0 ) ).xyz );
			float dotResult195 = dot( temp_output_192_0 , float3(1,1,1) );
			float3 BlendComponents212 = ( temp_output_192_0 / dotResult195 );
			float3 break222 = BlendComponents212;
			float2 temp_cast_3 = (temp_output_208_0).xx;
			float2 appendResult198 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 panner214 = ( 0.2 * _Time.y * temp_cast_3 + ( appendResult198 * _Vector1 ));
			float2 temp_cast_4 = (temp_output_208_0).xx;
			float2 appendResult202 = (float2(ase_worldPos.x , ase_worldPos.y));
			float2 panner225 = ( 0.2 * _Time.y * temp_cast_4 + ( appendResult202 * _Vector1 ));
			float4 lerpResult239 = lerp( ( ( ( tex2Dlod( WindNoise, float4( panner217, 0, 1.0) ) * break222.x ) + ( tex2Dlod( WindNoise, float4( panner214, 0, 1.0) ) * break222.y ) ) + ( tex2Dlod( WindNoise, float4( panner225, 0, 1.0) ) * break222.z ) ) , float4( 0,0,0,0 ) , WindConfiguration.y);
			float4 clampResult242 = clamp( ( lerpResult239 * WindConfiguration.w ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			v.vertex.xyz += clampResult242.rgb;
		}

		inline half4 LightingStandardCustom(SurfaceOutputStandardCustom s, half3 viewDir, UnityGI gi )
		{
			half3 transmission = max(0 , -dot(s.Normal, gi.light.dir)) * gi.light.color * s.Transmission;
			half4 d = half4(s.Albedo * transmission , 0);

			SurfaceOutputStandard r;
			r.Albedo = s.Albedo;
			r.Normal = s.Normal;
			r.Emission = s.Emission;
			r.Metallic = s.Metallic;
			r.Smoothness = s.Smoothness;
			r.Occlusion = s.Occlusion;
			r.Alpha = s.Alpha;
			return LightingStandard (r, viewDir, gi) + d;
		}

		inline void LightingStandardCustom_GI(SurfaceOutputStandardCustom s, UnityGIInput data, inout UnityGI gi )
		{
			#if defined(UNITY_PASS_DEFERRED) && UNITY_ENABLE_REFLECTION_BUFFERS
				gi = UnityGlobalIllumination(data, s.Occlusion, s.Normal);
			#else
				UNITY_GLOSSY_ENV_FROM_SURFACE( g, s, data );
				gi = UnityGlobalIllumination( data, s.Occlusion, s.Normal, g );
			#endif
		}

		void surf( Input i , inout SurfaceOutputStandardCustom o )
		{
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			o.Normal = UnpackNormal( tex2D( _Normal, uv_Normal ) );
			float2 uv_Diffuse = i.uv_texcoord * _Diffuse_ST.xy + _Diffuse_ST.zw;
			float4 tex2DNode69 = tex2D( _Diffuse, uv_Diffuse );
			o.Albedo = ( tex2DNode69 * _DiffuseColor ).rgb;
			float2 uv_Maskmap = i.uv_texcoord * _Maskmap_ST.xy + _Maskmap_ST.zw;
			float4 tex2DNode73 = tex2D( _Maskmap, uv_Maskmap );
			o.Metallic = tex2DNode73.r;
			o.Smoothness = ( tex2DNode73.a / 2.5 );
			o.Occlusion = tex2DNode73.g;
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			o.Transmission = ( ( ase_lightColor.rgb * ( ase_lightColor.a / 6.0 ) ) * _SubsurfaceIntensity );
			o.Alpha = 1;
			clip( ( tex2DNode69.a * _DiffuseColor.a ) - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
1950;66;1153;948;-633.5358;656.4662;1.402612;True;True
Node;AmplifyShaderEditor.CommentaryNode;184;-3113.186,701.0977;Inherit;False;4537.894;1236.413;Wind;12;222;225;217;214;211;200;213;206;205;190;187;183;Wind;0.1981132,0.1981132,0.1981132,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;183;-2974.285,1241.921;Inherit;False;1251.001;478.4487;Object World Normal;8;191;212;203;195;192;188;186;185;Object World Normal;0,0.7490196,1,1;0;0
Node;AmplifyShaderEditor.WorldNormalVector;185;-2899.581,1420.786;Inherit;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldToObjectMatrix;186;-2899.581,1324.787;Inherit;False;0;1;FLOAT4x4;0
Node;AmplifyShaderEditor.CommentaryNode;187;-1673.169,1241.141;Inherit;False;462.4961;491.9891;World Position;4;202;197;198;189;World Position;0,0.7490196,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;188;-2627.581,1388.786;Inherit;False;2;2;0;FLOAT4x4;0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldPosInputsNode;189;-1623.169,1511.414;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;190;-1676.86,766.1543;Inherit;False;560.0123;408.5356;Base Wind Configuration;3;208;196;193;Base Wind Configuration;0,0.7490196,1,1;0;0
Node;AmplifyShaderEditor.AbsOpNode;192;-2467.58,1388.786;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector3Node;191;-2500.699,1569.234;Float;False;Constant;_Vector0;Vector 0;-1;0;Create;True;0;0;False;0;False;1,1,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector2Node;193;-1611.164,1013.69;Float;False;Constant;_Vector1;Vector 1;13;0;Create;True;0;0;False;0;False;0.2,0.2;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector4Node;196;-1626.86,816.1541;Inherit;False;Global;WindConfiguration;WindConfiguration;11;0;Create;True;0;0;False;0;False;0,0,0,0;0.5,0.3,0.2,0.2;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;197;-1370.475,1291.141;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DotProductOpNode;195;-2293.68,1455.183;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;198;-1365.901,1394.308;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;205;-1126.566,1299.336;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0.05,0.05;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;203;-2139.872,1386.714;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TFHCRemapNode;208;-1329.847,832.3606;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;206;-1122.972,1406.583;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0.05,0.05;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;202;-1368.382,1501.171;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;217;-954.638,1252.448;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-1;False;1;FLOAT;0.2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;213;-1123.519,1514.467;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0.05,0.05;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;200;-2102.366,835.635;Inherit;False;302;280;Wind Noise;1;209;Wind Noise;0,0.7490196,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;212;-1985.09,1388.786;Float;True;BlendComponents;1;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;211;-696.9145,888.0839;Inherit;False;2063.97;1009.355;Noise Generation;11;257;239;220;242;238;236;234;228;226;219;218;Noise Generation;0,0.7483807,1,1;0;0
Node;AmplifyShaderEditor.PannerNode;214;-954.2341,1386.99;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,1;False;1;FLOAT;0.2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;219;-623.982,967.7847;Inherit;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;220;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;209;-2052.366,885.6351;Inherit;True;Global;WindNoise;WindNoise;6;0;Create;True;0;0;False;0;False;None;d4967142faab9c940be7140d38625a8b;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.PannerNode;225;-952.197,1513.031;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,1;False;1;FLOAT;0.2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;218;-620.5417,1185.844;Inherit;True;Property;_TextureSample3;Texture Sample 3;1;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;220;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;222;-1511.528,1771.691;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;226;-293.1107,938.0839;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;228;-295.3458,1176.734;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;220;-628.9144,1426.917;Inherit;True;Property;_NoiseRGB;Noise RGB;1;0;Create;True;0;0;False;0;False;-1;None;c1f26bd78e9a02944ae1c46868e74a48;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;234;-14.27825,1058.813;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;236;-299.0847,1405.075;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;84;625.6956,324.4097;Inherit;False;722.3415;330.7169;SSS;5;78;79;77;259;258;SSS;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;238;255.8875,1224.832;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LightColorNode;77;640.6956,381.3555;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.CommentaryNode;80;704.962,-852.2233;Inherit;False;560.9589;466.4702;Albedo;3;69;71;70;Albedo;1,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;239;548.0929,1525.143;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;79;832.5731,462.1265;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;6;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;83;705.9334,-5.180159;Inherit;False;640.0347;298.7335;PBR Mask;2;73;76;PBR Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;81;702.0105,-335.2483;Inherit;False;380.9999;280;Normal;1;72;Normal;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;70;814.7309,-592.7527;Inherit;False;Property;_DiffuseColor;Diffuse Color;2;0;Create;True;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;73;755.9334,44.81984;Inherit;True;Property;_Maskmap;Maskmap;5;0;Create;True;0;0;False;0;False;-1;None;dfeadc79c4715de43a9d7ef0a97053cc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;259;839.3158,566.0637;Inherit;False;Property;_SubsurfaceIntensity;Subsurface Intensity;1;0;Create;True;0;0;False;0;False;0.7;0.7;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;69;754.962,-802.2233;Inherit;True;Property;_Diffuse;Diffuse;3;0;Create;True;0;0;False;0;False;-1;None;524dc6a82608b2a4fafabee732240817;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;257;763.3461,1632.833;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;82;1114.214,-327.9006;Inherit;False;233.4966;199.0942;Cutout;1;74;Cutout;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;78;969.037,374.4097;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;74;1178.798,-272.2901;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;1096.921,-783.694;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;72;752.0105,-285.2483;Inherit;True;Property;_Normal;Normal;4;0;Create;True;0;0;False;0;False;-1;None;c300b9730f45f2940ab04bce17cc2325;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;258;1220.316,392.0638;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;76;1188.968,160.5533;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;2.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;242;1132.829,1619.754;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1789.72,-117.3091;Float;False;True;-1;4;ASEMaterialInspector;0;0;Standard;Infinity PBR/BOTD/Conversion1_CutoutBackFace;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.3;True;True;0;False;TransparentCutout;;AlphaTest;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;188;0;186;0
WireConnection;188;1;185;0
WireConnection;192;0;188;0
WireConnection;197;0;189;1
WireConnection;197;1;189;3
WireConnection;195;0;192;0
WireConnection;195;1;191;0
WireConnection;198;0;189;1
WireConnection;198;1;189;3
WireConnection;205;0;197;0
WireConnection;205;1;193;0
WireConnection;203;0;192;0
WireConnection;203;1;195;0
WireConnection;208;0;196;1
WireConnection;206;0;198;0
WireConnection;206;1;193;0
WireConnection;202;0;189;1
WireConnection;202;1;189;2
WireConnection;217;0;205;0
WireConnection;217;2;208;0
WireConnection;213;0;202;0
WireConnection;213;1;193;0
WireConnection;212;0;203;0
WireConnection;214;0;206;0
WireConnection;214;2;208;0
WireConnection;219;1;217;0
WireConnection;225;0;213;0
WireConnection;225;2;208;0
WireConnection;218;1;214;0
WireConnection;222;0;212;0
WireConnection;226;0;219;0
WireConnection;226;1;222;0
WireConnection;228;0;218;0
WireConnection;228;1;222;1
WireConnection;220;0;209;0
WireConnection;220;1;225;0
WireConnection;234;0;226;0
WireConnection;234;1;228;0
WireConnection;236;0;220;0
WireConnection;236;1;222;2
WireConnection;238;0;234;0
WireConnection;238;1;236;0
WireConnection;239;0;238;0
WireConnection;239;2;196;2
WireConnection;79;0;77;2
WireConnection;257;0;239;0
WireConnection;257;1;196;4
WireConnection;78;0;77;1
WireConnection;78;1;79;0
WireConnection;74;0;69;4
WireConnection;74;1;70;4
WireConnection;71;0;69;0
WireConnection;71;1;70;0
WireConnection;258;0;78;0
WireConnection;258;1;259;0
WireConnection;76;0;73;4
WireConnection;242;0;257;0
WireConnection;0;0;71;0
WireConnection;0;1;72;0
WireConnection;0;3;73;1
WireConnection;0;4;76;0
WireConnection;0;5;73;2
WireConnection;0;6;258;0
WireConnection;0;10;74;0
WireConnection;0;11;242;0
ASEEND*/
//CHKSM=110A0BCC4234C8AB5DE1AE4B5F0E24E6B75FD143