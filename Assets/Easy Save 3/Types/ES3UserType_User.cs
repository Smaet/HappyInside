using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("isFirst", "userBaseProperties", "enabled", "name")]
	public class ES3UserType_User : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_User() : base(typeof(User)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (User)obj;
			
			writer.WriteProperty("isFirst", instance.isFirst, ES3Type_bool.Instance);
			writer.WriteProperty("userBaseProperties", instance.userBaseProperties);
			writer.WriteProperty("enabled", instance.enabled, ES3Type_bool.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
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
					case "enabled":
						instance.enabled = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
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