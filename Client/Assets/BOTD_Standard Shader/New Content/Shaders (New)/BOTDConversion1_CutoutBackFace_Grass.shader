// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Infinity PBR/BOTD/Conversion1_CutoutBackFace_Grass"
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
			float temp_output_163_0 = (0.0 + (WindConfiguration.x - 0.0) * (1.2 - 0.0) / (1.0 - 0.0));
			float2 temp_cast_0 = (temp_output_163_0).xx;
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 appendResult151 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 _Vector2 = float2(0.2,0.2);
			float2 panner170 = ( 0.2 * _Time.y * temp_cast_0 + ( appendResult151 * _Vector2 ));
			float3 ase_worldNormal = UnityObjectToWorldNormal( v.normal );
			float3 temp_output_143_0 = abs( mul( unity_WorldToObject, float4( ase_worldNormal , 0.0 ) ).xyz );
			float dotResult144 = dot( temp_output_143_0 , float3(1,1,1) );
			float3 BlendComponents209 = ( temp_output_143_0 / dotResult144 );
			float3 break147 = BlendComponents209;
			float2 temp_cast_3 = (temp_output_163_0).xx;
			float2 appendResult157 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 panner174 = ( 0.2 * _Time.y * temp_cast_3 + ( appendResult157 * _Vector2 ));
			float2 temp_cast_4 = (temp_output_163_0).xx;
			float2 appendResult168 = (float2(ase_worldPos.x , ase_worldPos.y));
			float2 panner183 = ( 0.2 * _Time.y * temp_cast_4 + ( appendResult168 * _Vector2 ));
			float4 lerpResult208 = lerp( ( ( ( tex2Dlod( WindNoise, float4( panner170, 0, 1.0) ) * break147.x ) + ( tex2Dlod( WindNoise, float4( panner174, 0, 1.0) ) * break147.y ) ) + ( tex2Dlod( WindNoise, float4( panner183, 0, 1.0) ) * break147.z ) ) , float4( 0,0,0,0 ) , WindConfiguration.y);
			float4 clampResult207 = clamp( ( ( lerpResult208 * WindConfiguration.w ) / float4( 2.670157,2.670157,2.670157,0 ) ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			v.vertex.xyz += clampResult207.rgb;
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
			o.Transmission = ( ( ase_lightColor.rgb * ( ase_lightColor.a / 8.0 ) ) * _SubsurfaceIntensity );
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
1950;66;1153;948;1560.734;-598.7261;2.40422;True;True
Node;AmplifyShaderEditor.CommentaryNode;219;-2125.971,753.2068;Inherit;False;4476.65;1308.113;Wind;12;147;183;218;174;170;175;215;161;167;216;217;214;Wind;0.1981132,0.1981132,0.1981132,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;214;-2017.18,1276.814;Inherit;False;1251.001;478.4487;Object World Normal;8;209;145;144;143;142;141;140;139;Object World Normal;0,0.7490196,1,1;0;0
Node;AmplifyShaderEditor.WorldNormalVector;139;-1967.181,1422.812;Inherit;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldToObjectMatrix;140;-1967.181,1326.813;Inherit;False;0;1;FLOAT4x4;0
Node;AmplifyShaderEditor.CommentaryNode;217;-685.955,1293.25;Inherit;False;456.8153;488.1536;World Position;4;168;151;157;149;World Position;0,0.7490196,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;141;-1695.181,1390.812;Inherit;False;2;2;0;FLOAT4x4;0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector3Node;142;-1568.299,1571.261;Float;False;Constant;_Vector1;Vector 0;-1;0;Create;True;0;0;False;0;False;1,1,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.AbsOpNode;143;-1535.18,1390.812;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;216;-689.6452,818.2635;Inherit;False;560.0123;408.5356;Base Wind Configuration;3;210;163;159;Base Wind Configuration;0,0.7490196,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;149;-635.955,1563.523;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;151;-383.2607,1343.25;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;210;-639.6452,868.2635;Inherit;False;Global;WindConfiguration;WindConfiguration;11;0;Create;True;0;0;False;0;False;0,0,0,0;0.5,0.3,0.2,0.2;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;144;-1361.28,1457.209;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;157;-378.6867,1446.418;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;159;-623.95,1065.799;Float;False;Constant;_Vector2;Vector 1;13;0;Create;True;0;0;False;0;False;0.2,0.2;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleDivideOpNode;145;-1199.18,1390.812;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;161;-139.3518,1351.445;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0.05,0.05;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;168;-381.1681,1553.28;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TFHCRemapNode;163;-342.6328,884.4696;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;167;-135.7572,1458.692;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0.05,0.05;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;218;225.7244,964.4733;Inherit;False;2052.128;1004.178;Noise Generation;12;208;182;207;213;202;199;196;194;188;193;185;184;Noise Generation;0,0.7483807,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;215;-1099.354,857.9405;Inherit;False;302;280;Wind Noise;1;211;Wind Noise;0,0.7490196,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;175;-136.3047,1566.576;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0.05,0.05;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;170;32.57611,1304.558;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-1;False;1;FLOAT;0.2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;174;32.98074,1439.1;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,1;False;1;FLOAT;0.2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;209;-1038.18,1390.812;Float;True;BlendComponents;1;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BreakToComponentsNode;147;-555.02,1829.911;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.TexturePropertyNode;211;-1049.354,907.9404;Inherit;True;Global;WindNoise;WindNoise;6;0;Create;True;0;0;False;0;False;None;d4967142faab9c940be7140d38625a8b;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;185;302.0972,1262.233;Inherit;True;Property;_TextureSample4;Texture Sample 3;1;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;182;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;184;298.6567,1044.174;Inherit;True;Property;_TextureSample2;Texture Sample 1;1;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;182;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;183;35.01729,1565.14;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,1;False;1;FLOAT;0.2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;182;284.1778,1507.424;Inherit;True;Property;_NoiseRGB;Noise RGB;1;0;Create;True;0;0;False;0;False;-1;None;c1f26bd78e9a02944ae1c46868e74a48;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;188;629.5275,1014.473;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;193;627.2925,1253.123;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;194;623.5535,1481.464;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;196;908.3602,1135.202;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;84;685.6956,334.4097;Inherit;False;669.3415;340.7169;SSS;5;78;79;77;221;222;SSS;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;199;1178.526,1301.221;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LightColorNode;77;735.6956,406.3555;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.LerpOp;208;1411.257,1565.17;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;79;912.5731,455.1265;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;8;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;80;706.962,-854.2233;Inherit;False;622.3622;468.8318;Albedo;3;69;70;223;Albedo;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;83;707.9334,-7.180159;Inherit;False;640.0347;298.7335;PBR Mask;2;73;76;PBR Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;202;1667.84,1709.925;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;70;816.7309,-594.7527;Inherit;False;Property;_DiffuseColor;Diffuse Color;2;0;Create;True;0;0;False;0;False;1,1,1,1;0.7843137,0.7843137,0.7843137,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;69;756.962,-804.2233;Inherit;True;Property;_Diffuse;Diffuse;3;0;Create;True;0;0;False;0;False;-1;None;9bf68c6e5059e7549b60e91ca11987a6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;82;1116.214,-329.9006;Inherit;False;250.551;192.1595;Cutout;1;74;Cutout;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;213;1835.531,1721.174;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;2.670157,2.670157,2.670157,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;78;1052.037,377.4097;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;81;704.0105,-337.2483;Inherit;False;380.9999;280;Normal;1;72;Normal;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;73;757.9334,42.81984;Inherit;True;Property;_Maskmap;Maskmap;5;0;Create;True;0;0;False;0;False;-1;None;e9a1c1b060b9b2c4989233f1037f9d5d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;221;868.1677,573.7278;Inherit;False;Property;_SubsurfaceIntensity;Subsurface Intensity;1;0;Create;True;0;0;False;0;False;0.7;0.7;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;207;2013.582,1696.143;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;74;1195.879,-276.9006;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;76;1190.968,158.5533;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;2.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;72;754.0105,-287.2483;Inherit;True;Property;_Normal;Normal;4;0;Create;True;0;0;False;0;False;-1;None;9a1568052101f2f48ba76e7342adc3f3;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;222;1214.168,459.7278;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;223;1129.737,-668.1431;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2606.742,-122.9804;Float;False;True;-1;4;ASEMaterialInspector;0;0;Standard;Infinity PBR/BOTD/Conversion1_CutoutBackFace_Grass;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.3;True;True;0;False;TransparentCutout;;AlphaTest;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;141;0;140;0
WireConnection;141;1;139;0
WireConnection;143;0;141;0
WireConnection;151;0;149;1
WireConnection;151;1;149;3
WireConnection;144;0;143;0
WireConnection;144;1;142;0
WireConnection;157;0;149;1
WireConnection;157;1;149;3
WireConnection;145;0;143;0
WireConnection;145;1;144;0
WireConnection;161;0;151;0
WireConnection;161;1;159;0
WireConnection;168;0;149;1
WireConnection;168;1;149;2
WireConnection;163;0;210;1
WireConnection;167;0;157;0
WireConnection;167;1;159;0
WireConnection;175;0;168;0
WireConnection;175;1;159;0
WireConnection;170;0;161;0
WireConnection;170;2;163;0
WireConnection;174;0;167;0
WireConnection;174;2;163;0
WireConnection;209;0;145;0
WireConnection;147;0;209;0
WireConnection;185;1;174;0
WireConnection;184;1;170;0
WireConnection;183;0;175;0
WireConnection;183;2;163;0
WireConnection;182;0;211;0
WireConnection;182;1;183;0
WireConnection;188;0;184;0
WireConnection;188;1;147;0
WireConnection;193;0;185;0
WireConnection;193;1;147;1
WireConnection;194;0;182;0
WireConnection;194;1;147;2
WireConnection;196;0;188;0
WireConnection;196;1;193;0
WireConnection;199;0;196;0
WireConnection;199;1;194;0
WireConnection;208;0;199;0
WireConnection;208;2;210;2
WireConnection;79;0;77;2
WireConnection;202;0;208;0
WireConnection;202;1;210;4
WireConnection;213;0;202;0
WireConnection;78;0;77;1
WireConnection;78;1;79;0
WireConnection;207;0;213;0
WireConnection;74;0;69;4
WireConnection;74;1;70;4
WireConnection;76;0;73;4
WireConnection;222;0;78;0
WireConnection;222;1;221;0
WireConnection;223;0;69;0
WireConnection;223;1;70;0
WireConnection;0;0;223;0
WireConnection;0;1;72;0
WireConnection;0;3;73;1
WireConnection;0;4;76;0
WireConnection;0;5;73;2
WireConnection;0;6;222;0
WireConnection;0;10;74;0
WireConnection;0;11;207;0
ASEEND*/
//CHKSM=218451A3469B4EAD71C7E035DFB6EEB1A41AF3E3