Shader "Colorz/Alpha"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_OccTex ("Occ (RGB)", 2D) = "white" {}
		_OccPower("Occ Power", Range(0.0,1.0)) = 0.5
		_RimPower("Rim Power", Range(0.0,1.0)) = 0.5
		_RimCP("Rim Contrast Point", Range(0.0,0.05)) = 0.05
		_RimCL("Rim Contrast Level", Range(0.0,1.0)) = 0.5
		_RimPowerEdge("Rim Power Edge", Range(0.0,1.0)) = 0.05
		_RimSizeEdge("Rim Size Edge", Range(0.0,2.0)) = 1.0
		_AlphaTex ("Alpha (RGB)", 2D) = "white" {}
		_Cutoff ("Base Alpha cutoff", Range (0,.9)) = .5
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		Cull Back
		
		CGPROGRAM
			#pragma surface surf WrapLambert
			#pragma exclude_renderers flash
			
			sampler2D _MainTex, _OccTex, _AlphaTex;
			float _OccPower, _RimPower, _RimCP, _RimCL, _RimPowerEdge, _RimSizeEdge, _Cutoff;
			
			half4 LightingWrapLambert (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
			{
				half diff = min (0.32, dot(s.Normal, lightDir)+0.32);

				half4 c;
				c.rgb = s.Albedo * _LightColor0.rgb * (diff * atten);
				c.a = s.Alpha;
				return c;
			}

			struct Input
			{
				float2 uv_MainTex;
				float3 worldNormal;
				float3 viewDir;
				INTERNAL_DATA
			};

			void surf (Input IN, inout SurfaceOutput o)
			{
				half4 alpha = tex2D(_AlphaTex, IN.uv_MainTex);
				if(((alpha.r + alpha.g + alpha.b)/3) < _Cutoff)
				{
					discard;
				}

				half4 tex = tex2D(_MainTex, IN.uv_MainTex);
				half4 occ = (1.0-_OccPower) + _OccPower * tex2D(_OccTex, IN.uv_MainTex);

				o.Albedo = tex.rgb;
				
				half3 rimNormal = pow((((saturate(dot(normalize(IN.viewDir), o.Normal))) - _RimCP) * _RimCL) + _RimCP, 1-_RimPower);
				// Cull BACK
				half rimEdge = pow((1.0f - saturate(dot(normalize(IN.viewDir), o.Normal))), 2/_RimSizeEdge);
				o.Emission = occ.rgb * (tex.rgb * rimNormal + ((tex.rgb*(1-_RimPowerEdge) + occ.rgb*_RimPowerEdge) * rimEdge));
			}
		ENDCG
	}
	Fallback "Mobile/Diffuse"
}