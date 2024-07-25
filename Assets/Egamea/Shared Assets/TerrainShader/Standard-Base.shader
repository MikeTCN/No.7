// 4-channel Colormapped Splatmap terrain Shader Copyright (c) 2018 Paul Blower

Shader "Hidden/TerrainEngine/Colormapped/Standard-Base" {
    Properties {
        _ColorMap ("Color Map (RGB)", 2D) = "white" {}
        //_MetallicTex ("Metallic (R)", 2D) = "white" {}

        // used in fallback on old cards
        _Color ("Main Color", Color) = (1,1,1,1)
    }

    SubShader {
        Tags {
            "RenderType" = "Opaque"
            "Queue" = "Geometry-100"
        }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0
        // needs more than 8 texcoords
        #pragma exclude_renderers gles
        #include "UnityPBSLighting.cginc"

        sampler2D _ColorMap;
        //sampler2D _MetallicTex;

        struct Input {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o) {
            half4 c = tex2D (_ColorMap, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = 1;
            o.Smoothness = fixed4(0,0,0,0);
            o.Metallic = fixed4(0,0,0,0); //tex2D (_MetallicTex, IN.uv_MainTex).r;
        }

        ENDCG
    }

    FallBack "Diffuse"
}
