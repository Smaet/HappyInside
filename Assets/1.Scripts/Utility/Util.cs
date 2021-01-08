using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Polo
{
	// 자주쓰는 유틸리티 펑션들을 모아둔 클래스 
	public class Util
	{
		static public float EPSILON = 0.001f;
		
		// 차일드 오브젝트에서 PartOfName의 문자열을 일부 포함하는 오브젝트를 검색한다.
		// 리턴값은 단일 오브젝트이며 하나만 찾으면 탐색이 종료된다.
		static public Transform FindChild(Transform beginTransform, string partOfName)
		{
			for(int i=0; i<beginTransform.childCount; i++ )
			{
				Transform t = beginTransform.GetChild(i);
				if( t.name.Contains(partOfName) == true)
				{
                    //Debug.Log("correct t=" + t.name + " vs " + partOfName);
                    return t;
				}
                else {
                    //Debug.Log("incorrect t=(" + t.name + ") vs (" + partOfName+") "+(t.name==partOfName)+","+(t.name.ToLower()==partOfName.ToLower()));
                }

                if (t.childCount > 0 )
				{
					t = FindChild(t, partOfName);
					if( t != null )
					{
						return t;
					}
				}
			}

			return null;
		}
		
		// 차일드 오브젝트에서 PartOfName의 문자열을 일부 포함하는 오브젝트를 검색한다.
		// 리턴값은 멀티 오브젝트이며 재귀호출하며 모든 노드를 탐색한다.
		static public Transform[] FindChilds(Transform beginTransform, string partOfName)
		{
			List<Transform> ret = new List<Transform>();
			
			for(int i=0; i<beginTransform.childCount; i++ )
			{
				Transform t = beginTransform.GetChild(i);
				if( t.name.Contains(partOfName) == true )
				{
					ret.Add (t);
				}
				
				if(t.childCount > 0 )
				{
					Transform[] ts = FindChilds(t, partOfName);
					if( ts != null && ts.Length > 0 )
					{
						for(int v=0; v<ts.Length; v++)
						{
							ret.Add(ts[v]);
						}
					}
				}
			}
			
			return ret.ToArray();
		}
		
		// 차일드 노드 내에 T컴포넌트를 모두 탐색하여 array로 리턴해준다.
		static public T[] GetComponentsInMyNodes<T>(Transform root)
		{
			List<T> myList = new List<T>();
			
			for(int i=0; i<root.childCount; i++ )
			{
				Transform t = root.GetChild(i);
				T getT = t.GetComponent<T>();
				if( getT != null )
				{
					myList.Add (getT);
				}
				
				if(t.childCount > 0 )
				{
					T[] ts = GetComponentsInMyNodes<T>(t);
					if( ts != null && ts.Length > 0 )
					{
						for(int v=0; v<ts.Length; v++)
						{
							myList.Add(ts[v]);
						}
					}
				}
			}
			
			return myList.ToArray();
		}
		
		// 차일드 노드의 모든 오브젝트 layer를 설정한다.
		static public void SetLayerChild(Transform beginTransform, int layer)
		{
			beginTransform.gameObject.layer = layer;
			
			for(int i=0; i<beginTransform.childCount; i++ )
			{
				Transform t = beginTransform.GetChild(i);
				SetLayerChild(t, layer);
			}
		}
		
		// float 변수의 값을 증가 시키되 max까지 도달하면 더이상 증가하지 않고 max를 리턴한다.
		static public float FloatIncrease(float begin, float end, float delta)
		{
			float ret = (begin + delta);
			
			if ( begin > end )
			{
				ret = (begin - delta);
				
				return ret < end ? end : ret;
			}
			
			return ret > end ? end : ret;
		}
		
		// Vector3의 값을 delta만큼 증가시킨다.
		static public Vector3 Increase(Vector3 from, Vector3 to, float delta)
		{
			Vector3 ret = new Vector3();
			
			ret.x = FloatIncrease (from.x, to.x, delta);
			ret.y = FloatIncrease (from.y, to.y, delta);
			ret.z = FloatIncrease (from.z, to.z, delta);
			
			return ret;
		}
		
		// weights의 리스트 중에 하나를 랜덤하게 선택해준다.
		// weights 값 자체가 확률값이어서 값이 큰 index가 선택될 확률이 높아진다.
		static public int RandomSwitch(float [] weights)
		{
			float total = 0.0f;
			
			for ( int i = 0; i < weights.Length; i++ )
			{
				total += weights [i];
			}
			
			float r = UnityEngine.Random.Range (0.0f, total);
			float acc = 0.0f;
			
			
			for ( int i = 0; i < weights.Length; i++ )
			{
				if ( acc <= r && r < (acc + weights [i]) )
				{
					Debug.Log ("i="+i+", total=" + total + ", acc=" + acc + ", r=" + r + ", weights[i]=" + weights [i]);
					return i;
				}
				
				acc += weights [i];
			}
			
			throw new System.Exception("PoloUtility:RandomSwitch) invalid r");
			//return -1;
		}
		
		// 두 방향 벡터 사이의 각도를 얻는다.
		static public float GetAngle(Vector2 dir1, Vector2 dir2)
		{
			float lx1 = dir1.x;
			float ly1 = dir1.y;
			float lx2 = dir2.x;
			float ly2 = dir2.y;
			
			float inner = (lx1*lx2 + ly1*ly2);// 기본내적
			float i1 = Mathf.Sqrt(lx1*lx1 + ly1*ly1); // 처음 직선의 노말라이즈 준비
			float i2 = Mathf.Sqrt(lx2*lx2 + ly2*ly2); // 두번째 직선의 노말라이즈 준비
			
			lx1 = (lx1 / i1); // 각 요소를 단위 벡터로 변환한다.
			ly1 = (ly1 / i1);
			lx2 = (lx2 / i2);
			ly2 = (ly2 / i2);
			
			//위 과정을 거치면 결과적으로 계산된 두 직선의 크기는 1이면서 방향은 이전과 같은 단위벡터가 된다.
			inner = (lx1*lx2 + ly1*ly2); //다시 내적을 구한다.
			
			// 아크 코사인을 통해 라디안을 구하고 그걸 각도로 변환하기 위해 180을 곱하고 파이로나눈다.
			float result = Mathf.Acos(inner) * 180f / Mathf.PI;
			
			// 좌우를 구분한다.
			if(lx1 < lx2) 
			{
				result = - result;
			}
			
			return result;
		}
		
		// 현재 로테이션과 두 포지션 사이의 방향벡터를 얻어서 각도를 계산한다.
		static public Quaternion GetRotation(Quaternion currentRotation, Vector3 from, Vector3 to)
		{
			Quaternion nextRotation = currentRotation;
			Vector3 diff = to - from;
			
			if ( diff != Vector3.zero && diff.sqrMagnitude > 0 )
			{
				nextRotation = Quaternion.LookRotation (diff, Vector3.up);
			}
			else
			{
				//Debug.LogError ("diff != Vector3.zero && diff.sqrMagnitude > 0");
			}
			
			return nextRotation;
		}

		// value가 min/max사이에 있는 값인지 확인한다.
		static public bool Range(int value, int min, int max)
		{
			if( value >= min && value < max )
				return true;
			
			return false;
		}

		// value가 min/max사이에 있는 값인지 확인한다.
		static public bool Range(float value, float min, float max)
		{
			if( value >= min && value < max )
				return true;

			return false;
		}
		
		// 0~255범위의 컬러값을 0~1사이의 실수형으로 normalize해준다.
		public static Color ToColor(int r, int g, int b, int a)
		{
			return new Color(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);
		}
        
        public static Color SetColorAlpha(Color c, float a)
        {
            c.a = a;
            return c;
        }
		
		// 0~1값을 1~0값으로 변환
		public static float ReverseWeight(float originWeight)
		{
			float ret = 1f / originWeight;
			return ret;
		}
		
		// 해당 컬러값으로 채워진 이미지를 생성한다.
		public static Texture2D CreateColorTexture(Color color)
		{
			const int TEXTURE_SIZE = 1;
			
			Texture2D theTexture = new Texture2D (TEXTURE_SIZE, TEXTURE_SIZE, TextureFormat.RGB24, false);
			
			for( int y=0; y<TEXTURE_SIZE; y++ )
			{
				for( int x=0; x<TEXTURE_SIZE; x++ )
				{
					theTexture.SetPixel(x, y, color);
				}
			}
			
			return theTexture;
		}

        // 바로 직전에 선택된 index를 제외하고 랜덤하게 선택되도록 한다.		
        static public int DontOverlapRandom(int count, int prevIdx)
        {
            if (count <= 1)    // 무한루프 방지
                return 0;

            int ret = UnityEngine.Random.Range(0, count);
            if (ret == prevIdx)
                return DontOverlapRandom(count, prevIdx);

            return ret;
        }

        public static Vector2 SetVectorY(Vector2 v, float y)
        {
            return new Vector2(v.x, y);
        }

        public static Vector2 SetVectorX(Vector2 v, float x)
        {
            return new Vector2(x, v.y);
        }

        public static Vector3 SetVectorX(Vector3 v, float x)
        {
            return new Vector3(x, v.y, v.z);
        }

        public static Vector3 SetVectorY(Vector3 v, float y)
        {
            return new Vector3(v.x, y, v.z);
        }

        public static Vector3 SetVectorZ(Vector3 v, float z)
        {
            return new Vector3(v.x, v.y, z);
        }

        // 두 백터가 같은 값인가?
        public static bool Same(Vector3 a, Vector3 b)
		{
			if( Vector3.Distance(a, b) <= EPSILON )
					return true;
			
			return false;
		}
		
		// 두 백터가 같은 값인가?
		public static bool Same(Vector2 a, Vector2 b)
		{
			if( Vector2.Distance(a, b) <= EPSILON )
				return true;
			
			return false;
		}
		
		// 두 실수가 같은 값인가?
		public static bool Same(float a, float b)
		{
			if( Mathf.Abs (a - b) <= EPSILON )
				return true;
			
			return false;
		}

		// 두 컬러값이 같은 값인가?		
		public static bool Same(Color a, Color b)
		{
			if( Mathf.Abs (a.r - b.r) > EPSILON )
				return false;
			if( Mathf.Abs (a.g - b.g) > EPSILON )
				return false;
			if( Mathf.Abs (a.b - b.b) > EPSILON )
				return false;
			if( Mathf.Abs (a.a - b.a) > EPSILON )
				return false;
				
			return true;
		}
		
		// 두 백터가 range범위 이내인가? 
		public static bool Similar(Vector3 a, Vector3 b, float range)
		{
			if( Vector3.Distance(a, b) <= range )
				return true;
			
			return false;
		}
		
		// 두 백터가 range범위 이내인가? 
		public static bool Similar(Vector2 a, Vector2 b, float range)
		{
			if( Vector2.Distance(a, b) <= range )
				return true;
			
			return false;
		}
		
		// 두 실수가 range범위 이내인가? 
		public static bool Similar(float a, float b, float range = 0.5f)
		{
			if( Mathf.Abs (a - b) <= range )
				return true;
			
			return false;
		}
		
		// 숫자를 999,999 형식의 콤마가 들어간 문자열로 바꿈 
		public static string Num2Comma3( int num )
		{
			string str = ""+num;
			int idx = str.Length-1;
			int num3 = 0;
			
			while( idx > 0 )
			{
				num3++;
				
				if( num3 == 3 ) {
					str = str.Substring(0, idx) + "," + str.Substring(idx, str.Length-idx);
					num3 = 0;
				}
				
				idx--;
			}
			
			return str;
		}
		
		// 벡터2 -> 3으로 변환
		static public Vector3 Vector2To3(Vector2 vPos)
		{
			return new Vector3(vPos.x, 0.0f, vPos.y);
		}
		
		// 벡터3 -> 2로 변환
		static public Vector2 Vector3To2(Vector3 vPos)
		{
			return new Vector2(vPos.x, vPos.y);
		}
		
		// 텍스텨츼 해상도를 변경한다.
		static public Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight, bool bilinear = true) 
		{
			Texture2D result = new Texture2D(targetWidth,targetHeight, TextureFormat.ARGB32,false);
			
			float incX=(1.0f / (float)targetWidth);
			float incY=(1.0f / (float)targetHeight);
			
			for (int i = 0; i < result.height; ++i) 
			{
				for (int j = 0; j < result.width; ++j) 
				{
					if( bilinear )
					{
						Color newColor = source.GetPixelBilinear((float)j / (float)result.width, (float)i / (float)result.height);
						result.SetPixel(j, i, newColor);
					}
					else
					{
						Color newColor = source.GetPixel((int)((float)j / (float)result.width * source.width), (int)((float)i / (float)result.height * source.height));
						result.SetPixel(j, i, newColor);
					}
				}
			}
			
			result.Apply();
			return result;
		}
		
		// string을 enum값으로 변환한다.
		public static T ToEnum<T>(string str)
		{
			System.Array A = System.Enum.GetValues(typeof(T));
			foreach (T t in A)
			{
				if (t.ToString() == str)
					return t;
			}
			return default(T);
		}
		
		// enum의 리스트 갯수가 몇개인지 파악한다.
		public static int EnumCount<T>()
		{
			return Enum.GetNames(typeof(T)).Length;
		}
		
		// 두 어레이를 병합한다.
		public static void ArrayMerge<T>(ref T [] arr, T [] t)
		{
			T [] newArr = new T [arr.Length + t.Length];
			arr.CopyTo(newArr, 0);
			arr.CopyTo(t, newArr.Length);
			arr = newArr;
		}
		
		// 어레이어에 element를 하나 추가한다.
		public static void ArrayAdd<T>(ref T [] arr, T t)
		{
            int curLen = 0;

            if (arr != null && arr.Length > 0)
                curLen = arr.Length;

            T[] newArr = new T[curLen + 1];

            for ( int i = 0; i < curLen; i++ )
			{
				newArr [i] = arr [i];
			}
			
			newArr [curLen] = t;
			arr = newArr;
		}
        
	}
}

