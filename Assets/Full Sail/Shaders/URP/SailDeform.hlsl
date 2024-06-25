		static const float pi = 3.141592653589793238462;
        static const float pi2 = 6.283185307179586476924;
		static const float hpi = 3.141592653589793238462 * 0.5;

		//float4		_ImpactPoints[64];

#if false
		float2 Hash(float2 p)
		{
			p = float2(dot(p, float2(127.1, 311.7)), dot(p, float2(269.5, 183.3)));
			return frac(sin(p) * 43758.5453);
		} 
			
		float voronoi(float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
		{
			float2 n = floor(v);
			float2 f = frac(v);
			float F1 = 8.0;
			float F2 = 8.0; float2 mg = 0;
			for ( int j = -1; j <= 1; j++ )
			{
				for ( int i = -1; i <= 1; i++ )
				{
					float2 g = float2(i, j);
					float2 o = Hash(n + g);
					o = (sin(time + o * pi2) * 0.5 + 0.5);
					float2 r = f - g - o;
					float d = 0.5 * dot(r, r);
					float h = smoothstep(0.0, 1.0, 0.5 + 0.5 * (F1 - d) / smoothness);
					F1 = lerp(F1, d, h) - smoothness * h * (1.0 - h);
					mg = g;
					mr = r;
					id = o;
				}
			}
			return F1;
		}
#endif
		inline float noiseRandomValue(float2 uv)
		{
			return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
		}

		inline float noiseInterpolate(float a, float b, float t)
		{
			return (1.0 - t) * a + (t * b);
		}

		inline float valueNoise(float2 uv)
		{
			float2 i = floor(uv);
			float2 f = frac(uv);
			f = f * f * (3.0 - 2.0 * f);

			uv = abs(frac(uv) - 0.5);
			float2 c0 = i + float2(0.0, 0.0);
			float2 c1 = i + float2(1.0, 0.0);
			float2 c2 = i + float2(0.0, 1.0);
			float2 c3 = i + float2(1.0, 1.0);
			float r0 = noiseRandomValue(c0);
			float r1 = noiseRandomValue(c1);
			float r2 = noiseRandomValue(c2);
			float r3 = noiseRandomValue(c3);

			float bottomOfGrid = noiseInterpolate(r0, r1, f.x);
			float topOfGrid = noiseInterpolate(r2, r3, f.x);
			float t = noiseInterpolate(bottomOfGrid, topOfGrid, f.y);
			return t;
		}

		float SimpleNoise(float2 UV, float Scale)
		{
			float t = 0.0;

			float freq = pow(2.0, float(0));
			float amp = pow(0.5, float(3-0));
			t += valueNoise(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

			freq = pow(2.0, float(1));
			amp = pow(0.5, float(3-1));
			t += valueNoise(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

			freq = pow(2.0, float(2));
			amp = pow(0.5, float(3-2));
			t += valueNoise(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

			return t - 0.5;
		}

		//float wave(float2 position, float2 origin, float time)
		//{
		//	float d = length(position - origin);
		//	float t = time - (d * 0.5);
		//	return ((tex2Dlod(_ImpactRipple, float4(t, 0, 0, 0)).r - 0.5) * 2) * clamp(1 - d, 0, 1);
		//}

		float wave(float2 position, float2 origin, float time)
		{
			float d = length(position - origin);
			float t = time - (d * 0.5);
			return ((SAMPLE_TEXTURE2D_LOD(_ImpactRipple, sampler_ImpactRipple, float4(t, 0, 0, 0), 0).r - 0.5) * 2) * clamp(1 - d, 0, 1);
		}

		float SimpleNoise1(float2 UV, float Scale)
		{
			float v = SAMPLE_TEXTURE2D_LOD(_RippleNoise, sampler_RippleNoise, float4(UV.x * Scale * 0.01, UV.y * Scale * 0.01, 0, 0), 0).r;
			return v - 0.5;
		}

		float3 Deform(float2 uvin, float time1)
		{
			float3 locdir = _WindDir;	//mul(unity_WorldToObject, _FlapDir);
			//float3 ldir = mul(UNITY_MATRIX_I_M, _WindDir);
			float3x3 tm = (float3x3) UNITY_MATRIX_I_M;
			float3 ldir = mul(tm, _WindDir); // 	mul(UNITY_MATRIX_I_M, _WindDir); // * dotamt);

			locdir = ldir;

			float _sailWindLevel = (_SailWind);	// * _FillPercent);
			float dotamt = dot(ldir, _SailForward);
			float rev = 1.0;
			float mask; 

			if ( dotamt < 0 )
			{
				float swa = _sailWindLevel * _SailReverse * abs(dotamt);
				if ( swa > _sailWindLevel * _SailReverse )
					_sailWindLevel = _sailWindLevel * _SailReverse;
				else
					_sailWindLevel = swa;

				mask = SAMPLE_TEXTURE2D_LOD(_MaskMapRev, sampler_MaskMapRev, float4(uvin, 0, 0), 0).r;
			}
			else
			{
				mask = SAMPLE_TEXTURE2D_LOD(_MaskMap, sampler_MaskMap, float4(uvin, 0, 0), 0).r;
				_sailWindLevel *= dotamt;
			}

			float3 movedir = (lerp(_SailForward * dotamt, locdir, _SailSideways));
			float vs = _VorSeed;

			//float voroi = SimpleNoise(uvin + ((time1 + _VorSeed)  * _VorSpeed), _VorScale);
			float voroi = SimpleNoise1(uvin + ((time1 + _VorSeed)  * _VorSpeed), _VorScale);

			float3 val = (_sailWindLevel * movedir * _Speed) * mask * rev;
			//float3 rip = (mask * locdir * (rev * voroi * _VorStrength * _FillPercent * (1.0 - (_sailWindLevel * _FullRipple))));
			float3 rip = (mask * _SailForward * (rev * voroi * _VorStrength * _FillPercent * (1.0 - (_sailWindLevel * _FullRipple))));

			val += rip;
			//float3 val = (((_sailWindLevel * locdir * _Speed) * mask * dotamt) + (mask * locdir * (voroi * _VorStrength * _FillPercent * (1.0 - (_sailWindLevel * _FullRipple)))));
			//float3 val = (((_sailWindLevel * locdir) * mask * dotamt) + (mask * locdir * (voroi * _VorStrength * (1.0 - (_sailWindLevel * _FullRipple)))));

			float swls = _sailWindLevel * _Speed;
			float la = swls * (1.0 - uvin.y);
			val.y += la * _SailLift;

			// Side Arch
			float sa = abs((uvin.y * 2.0) - 1.0);
			float lsa = swls * _SailLiftSideArch;
			if ( uvin.x < 0.5 )
				val.x += (_SailSideArch + lsa) * cos(sa * hpi) * (1.0 - (uvin.x / 0.5));
			else
				val.x -= (_SailSideArch + lsa) * cos(sa * hpi) * (1.0 - ((1.0 - uvin.x) / 0.5));

			// Arch
			// bot arch
			float xa = 1.0 - abs((uvin.x * 2.0) - 1.0);
			float ya = 1.0 - (uvin.y / _SailArchStart);
			float sinx = sin(xa * hpi);
			if ( ya < 0.0 )
				ya = 0.0;
			if ( uvin.y < _SailArchStart )
			{
				val.y += (_SailArch + (la * _SailLiftArch)) * sinx * ya;
			}

			// top arch
			val.y += (_SailTopArch * sinx * (1 - ya));

			// Impact ripple
			val.z += wave(uvin.xy, _ImpactLocation.xy, (_Time.y - _ImpactLocation.z)) * _ImpactLocation.w * mask;

			return val;
		}

		void SailDeform_float(float3 vertex, float2 texcoord1, float time, out float3 pos)
		{
			float3 defamt = Deform(texcoord1.xy, time);
			float ft = 1.0;
			float2 uv_Furl = texcoord1.xy * _FurlMap_ST.xy + _FurlMap_ST.zw;
			
			float f = SAMPLE_TEXTURE2D_LOD(_FurlMap, sampler_FurlMap, float4(uv_Furl, 0, 0), 0).r;

			float rf = _Furl;
			float fa = 0;
			float xo = 1.0 - texcoord1.y;
			float tilt = lerp(_SailTiltTop, _SailTilt, xo);

			if ( f < rf )
			{
				ft = clamp(1.0 - (rf - f), 0, 1);

				if ( rf > 1 )
					fa = rf - 1;

				float3 furledvert;
				float frad = _FurledRadius * SAMPLE_TEXTURE2D_LOD(_FurledMap, sampler_FurledMap, float4(uv_Furl.x * _FurledMap_ST.x + _FurledMap_ST.z, _FurledMap_ST.y * _FurledMap_ST.y + _FurledMap_ST.w, 0, 0), 0).r;

				furledvert.z = (sin(pi2 - (pi2 * texcoord1.y)) * frad);
				furledvert.y = (cos(pi2 - (pi2 * texcoord1.y)) * frad) - frad;
				furledvert.x = vertex.x;
				furledvert.z *= _YScale;

				furledvert.y += _SailTiltTop * ((texcoord1.x * 2) - 1);

				float3 vert = vertex.xyz + (defamt * (ft * ft));

				vert.y += tilt * ((texcoord1.x * 2) - 1);
				vert.y = lerp(furledvert.y, vert.y, ft);
				vert.x *= 1.0 + (_SailTaper * xo) * ft;
				vert.x += xo * ft * _SailShear;

				vertex.xyz = lerp(vert, furledvert, fa);
			}
			else
			{
				float3 vert = vertex.xyz + (defamt * (ft * ft));

				vert.y += tilt * ((texcoord1.x * 2) - 1);
				vert.x += xo * ft * _SailShear;
				vert.x *= 1.0 + (_SailTaper * xo) * ft;

				vertex.xyz = vert.xyz;
			}

			pos = vertex;
		}
