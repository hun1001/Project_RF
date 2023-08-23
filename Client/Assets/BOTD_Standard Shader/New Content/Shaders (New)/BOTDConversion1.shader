// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Infinity PBR/BOTD/Conversion1"
{
	Properties
	{
		_DiffuseColor("Diffuse Color", Color) = (1,1,1,0)
		_Diffuse("Diffuse", 2D) = "black" {}
		_Normal("Normal", 2D) = "bump" {}
		_Maskmap("Maskmap", 2D) = "black" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 4.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform sampler2D _Diffuse;
		uniform float4 _Diffuse_ST;
		uniform float4 _DiffuseColor;
		uniform sampler2D _Maskmap;
		uniform float4 _Maskmap_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			o.Normal = UnpackNormal( tex2D( _Normal, uv_Normal ) );
			float2 uv_Diffuse = i.uv_texcoord * _Diffuse_ST.xy + _Diffuse_ST.zw;
			o.Albedo = ( tex2D( _Diffuse, uv_Diffuse ) * _DiffuseColor ).rgb;
			float2 uv_Maskmap = i.uv_texcoord * _Maskmap_ST.xy + _Maskmap_ST.zw;
			float4 tex2DNode1 = tex2D( _Maskmap, uv_Maskmap );
			o.Metallic = tex2DNode1.r;
			o.Smoothness = tex2DNode1.a;
			o.Occlusion = tex2DNode1.g;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
1950;66;1153;948;340.9963;371.2296;1.3;True;True
Node;AmplifyShaderEditor.SamplerNode;3;77.00953,-148.1323;Inherit;True;Property;_Diffuse;Diffuse;1;0;Create;True;0;0;False;0;False;-1;None;9ba43e074443d5246a0a979541182409;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;9;157.3817,53.72905;Float;False;Property;_DiffuseColor;Diffuse Color;0;0;Create;True;0;0;False;0;False;1,1,1,0;1,1,1,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;4;86.7028,445.4955;Inherit;True;Property;_Normal;Normal;2;0;Create;True;0;0;False;0;False;-1;None;b10797acdb0c04749be3253ce4c5991c;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;96.75212,250.8683;Inherit;True;Property;_Maskmap;Maskmap;3;0;Create;True;0;0;False;0;False;-1;None;ab5a0db1ea4004249b15689e1b11f433;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;479.8602,51.36263;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;645,25;Float;False;True;-1;4;ASEMaterialInspector;0;0;Standard;Infinity PBR/BOTD/Conversion1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.CommentaryNode;8;-535.4745,-205.9929;Inherit;False;100;100; ;0;;1,1,1,1;0;0
WireConnection;11;0;3;0
WireConnection;11;1;9;0
WireConnection;0;0;11;0
WireConnection;0;1;4;0
WireConnection;0;3;1;1
WireConnection;0;4;1;4
WireConnection;0;5;1;2
ASEEND*/
//CHKSM=B0F6258E489AD278BB54731589F4F19E22CCC379