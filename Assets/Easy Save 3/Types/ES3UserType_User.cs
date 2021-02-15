using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("isFirst", "userBaseProperties")]
	public class ES3UserType_User : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_User() : base(typeof(User)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (User)obj;
			
			writer.WriteProperty("isFirst", instance.isFirst, ES3Type_bool.Instance);
			writer.WriteProperty("userBaseProperties", instance.userBaseProperties);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (User)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "isFirst":
						instance.isFirst = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "userBaseProperties":
						instance.userBaseProperties = reader.Read<UserBaseProperties>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new User();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_UserArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_UserArray() : base(typeof(User[]), ES3UserType_User.Instance)
		{
			Instance = this;
		}
	}
}