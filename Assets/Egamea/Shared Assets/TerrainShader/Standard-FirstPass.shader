// 4-channel Colormapped Splatmap terrain Shader Copyright (c) 2018 Paul Blower

Shader "Nature/Terrain/Colormapped4Splat" {
    Properties {

         _SplatMap ("Splat Map (RGBA)", 2D) = "red" {}
		 _ColorMap ("Color Map (RGBA)", 2D) = "white" {}

		 _FadeDistance ("Fade Distance", Range(0.0, 1000.0)) = 100.0

		 [HideInInspector]
		 _Splat0 ("Splat Layer 1 (R)", 2D) = "white" {}
		 [HideInInspector]
		 _Splat1 ("Splat Layer 2 (G)", 2D) = "white" {}
		 [HideInInspector]
		 _Splat2 ("Splat Layer 3 (B)", 2D) = "white" {}
		 [HideInInspector]
         _Splat3 ("Splat Layer 4 (A)", 2D) = "white" {}

		 [HideInInspector]
         _Normal0 ("Normal Layer 1 (R)", 2D) = "bump" {}
		 [HideInInspector]
		 _Normal1 ("Normal Layer 2 (G)", 2D) = "bump" {}
		 [HideInInspector]
		 _Normal2 ("Normal Layer 3 (B)", 2D) = "bump" {}
		 [HideInInspector]
         _Normal3 ("Normal Layer 4 (A)", 2D) = "bump" {}

		 [HideInInspector]
         [Gamma] _Metallic0 ("Metallic 1", Range(0.0, 1.0)) = 0.0
		 [HideInInspector]
         [Gamma] _Metallic1 ("Metallic 2", Range(0.0, 1.0)) = 0.0
		 [HideInInspector]
         [Gamma] _Metallic2 ("Metallic 3", Range(0.0, 1.0)) = 0.0
		 [HideInInspector]
         [Gamma] _Metallic3 ("Metallic 4", Range(0.0, 1.0)) = 0.0

		 //[HideInInspector]
         _Smoothness0 ("Smoothness 1", Range(0.0, 1.0)) = 1.0
		 //[HideInInspector]
         _Smoothness1 ("Smoothness 2", Range(0.0, 1.0)) = 1.0
		 //[HideInInspector]
         _Smoothness2 ("Smoothness 3", Range(0.0, 1.0)) = 1.0
		 //[HideInInspector]
         _Smoothness3 ("Smoothness 4", Range(0.0, 1.0)) = 1.0

         // used in fallback on old cards & base map
         //_MainTex ("BaseMap (RGB)", 2D) = "white" {}
         //_Color ("Main Color", Color) = (1,1,1,1)
    }

    SubShader {
        Tags {
            "Queue" = "Geometry-100"
            "RenderType" = "Opaque"
        }

        CGPROGRAM

		//#define TERRAIN_USE_FOG	// Uncomment for fog support, but it will switch to SM 4.0

        #pragma surface surf Standard vertex:SplatmapVert finalcolor:SplatmapFinalColor finalgbuffer:SplatmapFinalGBuffer fullforwardshadows noinstancing
        #pragma multi_compile_fog

		#ifdef TERRAIN_USE_FOG
			#pragma target 4.0
		#else
			#pragma target 3.0
		#endif

        // needs more than 8 texcoords
        #pragma exclude_renderers gles psp2
        #include "UnityPBSLighting.cginc"

        #pragma multi_compile __ _TERRAIN_NORMAL_MAP

        #define TERRAIN_STANDARD_SHADER
        #define TERRAIN_SURFACE_OUTPUT SurfaceOutputStandard
        #include "TerrainSplatmapCustom.cginc"

		half _FadeDistance;

        half _Metallic0;
        half _Metallic1;
        half _Metallic2;
        half _Metallic3;

        half _Smoothness0;
        half _Smoothness1;
        half _Smoothness2;
        half _Smoothness3;

        void surf (Input IN, inout SurfaceOutputStandard o) {
            half4 splat_control;
            half4 color_control;
            half weight;
            fixed4 mixedDiffuse;
            half4 defaultSmoothness = half4(_Smoothness0, _Smoothness1, _Smoothness2, _Smoothness3);

            SplatmapMix(IN, defaultSmoothness, splat_control, weight, mixedDiffuse, o.Normal, color_control);

			float fadeout =  1.0 - saturate(  IN.viewDistance  / (_FadeDistance * _FadeDistance) );
            //1.0 - saturate(  IN.viewDistance  / (_FadeDistance * _FadeDistance) );

            o.Albedo = (mixedDiffuse.rgb * fadeout) + (color_control * (1-fadeout));			// base color
            o.Alpha = 1.0;//weight;																	// brightness?
            o.Smoothness = mixedDiffuse.a; 														// specular
            o.Metallic = dot(splat_control, half4(_Metallic0, _Metallic1, _Metallic2, _Metallic3));

			o.Normal = lerp(o.Normal, fixed4(0,0,1,0), 1-fadeout);
        }
        ENDCG
    }

    Dependency "AddPassShader" = "Hidden/TerrainEngine/Colormapped/Standard-AddPass"
    Dependency "BaseMapShader" = "Hidden/TerrainEngine/Colormapped/Standard-Base"

    Fallback "Nature/Terrain/ColorMappedDiffuse"
}

// TODO:
// Add checkbox to disable color map
// Add normal support onto color map channel
// Fog support for PC
