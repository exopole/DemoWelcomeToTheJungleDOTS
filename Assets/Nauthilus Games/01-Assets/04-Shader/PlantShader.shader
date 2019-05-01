// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "PlantShader"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_Plantcolor01("Plant color 01", Color) = (0,0,0,0)
		_Plantcolor02("Plant color 02", Color) = (0,0,0,0)
		_Flower_color_01("Flower_color_01", Color) = (0,0,0,0)
		_Flower_color_02("Flower_color_02", Color) = (0,0,0,0)
		_Emissivecolor("Emissive color", Color) = (0,0,0,0)
		_EmissivePower("Emissive Power", Range( 0 , 2)) = 0
		[Toggle]_Outline("Outline ", Int) = 1
		_Outlinecolor("Outline color", Color) = (0,0,0,0)
		_Texturemask01("Texture mask 01", 2D) = "white" {}
		_Texturemask02("Texture mask 02", 2D) = "white" {}
		_MaskClipValue( "Mask Clip Value", Float ) = 0.5
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha
		BlendOp Add
		CGPROGRAM
		#pragma target 3.0
		#pragma shader_feature _OUTLINE_ON
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _Plantcolor01;
		uniform float4 _Plantcolor02;
		uniform sampler2D _Texturemask01;
		uniform float4 _Texturemask01_ST;
		uniform float4 _Flower_color_01;
		uniform float4 _Flower_color_02;
		uniform sampler2D _Texturemask02;
		uniform float4 _Texturemask02_ST;
		uniform float4 _Outlinecolor;
		uniform float _EmissivePower;
		uniform float4 _Emissivecolor;
		uniform float _MaskClipValue = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Texturemask01 = i.uv_texcoord * _Texturemask01_ST.xy + _Texturemask01_ST.zw;
			float4 tex2DNode36 = tex2D( _Texturemask01, uv_Texturemask01 );
			float4 lerpResult16 = lerp( _Plantcolor01 , _Plantcolor02 , tex2DNode36.b);
			float4 lerpResult14 = lerp( float4(0,0,0,0) , lerpResult16 , tex2DNode36.r);
			float4 lerpResult32 = lerp( _Flower_color_01 , _Flower_color_02 , tex2DNode36.r);
			float2 uv_Texturemask02 = i.uv_texcoord * _Texturemask02_ST.xy + _Texturemask02_ST.zw;
			float4 tex2DNode37 = tex2D( _Texturemask02, uv_Texturemask02 );
			float4 lerpResult28 = lerp( lerpResult14 , lerpResult32 , tex2DNode37.b);
			float4 lerpResult19 = lerp( lerpResult28 , _Outlinecolor , tex2DNode37.g);
			#ifdef _OUTLINE_ON
			float4 staticSwitch35 = lerpResult19;
			#else
			float4 staticSwitch35 = lerpResult28;
			#endif
			o.Albedo = staticSwitch35.rgb;
			o.Emission = ( ( _EmissivePower * _Emissivecolor ) * tex2DNode37.r ).rgb;
			o.Alpha = 1;
			clip( tex2DNode36.g - _MaskClipValue );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13101
7;29;1906;1004;2540.353;1863.323;2.392238;True;False
Node;AmplifyShaderEditor.SamplerNode;36;-1590.7,-1109.253;Float;True;Property;_Texturemask01;Texture mask 01;8;0;Assets/Materials/PlantPack_mask_01.tga;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;17;-1145.819,-507.7972;Float;False;Property;_Plantcolor02;Plant color 02;1;0;0,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;11;-1144.544,-684.4213;Float;False;Property;_Plantcolor01;Plant color 01;0;0;0,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.LerpOp;16;-746.2632,-481.8316;Float;False;3;0;COLOR;0.0,0,0,0;False;1;COLOR;0.0;False;2;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.ColorNode;2;-827.7828,-888.5297;Float;False;Constant;_ColorRoot;Color Root;0;0;0,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;30;-1174.953,-1755.651;Float;False;Property;_Flower_color_01;Flower_color_01;2;0;0,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;31;-1173.208,-1577.172;Float;False;Property;_Flower_color_02;Flower_color_02;3;0;0,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;37;-1650.138,-819.5281;Float;True;Property;_Texturemask02;Texture mask 02;9;0;Assets/Materials/PlantPack_mask_02.tga;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.LerpOp;32;-730.0182,-1609.885;Float;False;3;0;COLOR;0.0;False;1;COLOR;0.0,0,0,0;False;2;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.LerpOp;14;-249.8077,-546.3232;Float;False;3;0;COLOR;0.0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.LerpOp;28;-134.2745,-1239.643;Float;False;3;0;COLOR;0.0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;26;-778.1285,52.4193;Float;False;Property;_EmissivePower;Emissive Power;5;0;0;0;2;0;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;20;479.1504,-1757.507;Float;False;Property;_Outlinecolor;Outline color;7;0;0,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;23;-684.2311,300.9401;Float;False;Property;_Emissivecolor;Emissive color;4;0;0,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.LerpOp;19;808.2234,-1445.472;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0.0;False;2;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-374.0675,117.6664;Float;False;2;2;0;FLOAT;0.0;False;1;COLOR;0.0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-35.09951,396.2192;Float;False;2;2;0;COLOR;0.0;False;1;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.StaticSwitch;35;838.0955,-1076.511;Float;False;Property;_Outline;Outline ;6;0;0;True;2;0;COLOR;0.0,0,0,0;False;1;COLOR;0.0;False;1;COLOR
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;780.6375,-340.6196;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;PlantShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;0;False;0;0;Custom;0.5;True;True;0;True;Transparent;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;2;SrcAlpha;OneMinusSrcAlpha;0;One;One;Add;Add;0;False;1;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;10;-1;-1;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;16;0;11;0
WireConnection;16;1;17;0
WireConnection;16;2;36;3
WireConnection;32;0;30;0
WireConnection;32;1;31;0
WireConnection;32;2;36;1
WireConnection;14;0;2;0
WireConnection;14;1;16;0
WireConnection;14;2;36;1
WireConnection;28;0;14;0
WireConnection;28;1;32;0
WireConnection;28;2;37;3
WireConnection;19;0;28;0
WireConnection;19;1;20;0
WireConnection;19;2;37;2
WireConnection;25;0;26;0
WireConnection;25;1;23;0
WireConnection;24;0;25;0
WireConnection;24;1;37;1
WireConnection;35;0;19;0
WireConnection;35;1;28;0
WireConnection;0;0;35;0
WireConnection;0;2;24;0
WireConnection;0;10;36;2
ASEEND*/
//CHKSM=B659F936369CE2414347E6E802F590D372856528