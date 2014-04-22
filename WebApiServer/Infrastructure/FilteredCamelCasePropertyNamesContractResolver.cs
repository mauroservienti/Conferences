using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebApiServer.Infrastructure
{
	public class FilteredCamelCasePropertyNamesContractResolver : DefaultContractResolver
	{
		public FilteredCamelCasePropertyNamesContractResolver()
		{
			this.AssembliesToInclude = new HashSet<Assembly>();
			this.TypesToInclude = new HashSet<Type>();
			this.ShouldConvertToCamelCase = t => false;
		}

		public HashSet<Assembly> AssembliesToInclude { get; set; }
		public HashSet<Type> TypesToInclude { get; set; }
		public Func<Type, Boolean> ShouldConvertToCamelCase { get; set; }

		protected override JsonProperty CreateProperty( MemberInfo member, MemberSerialization memberSerialization )
		{
			var jsonProperty = base.CreateProperty( member, memberSerialization );

			var type = member.DeclaringType;
			if ( this.TypesToInclude.Contains( type ) || this.AssembliesToInclude.Contains( type.Assembly ) || this.ShouldConvertToCamelCase( type ) )
			{
				jsonProperty.PropertyName = ToCamelCase( jsonProperty.PropertyName );
			}

			return jsonProperty;
		}

		static string ToCamelCase( string value )
		{
			if ( string.IsNullOrWhiteSpace( value ) )
			{
				return value;
			}

			if ( value.All( c => Char.IsUpper( c ) ) )
			{
				return value.ToLower();
			}

			var firstChar = value[ 0 ];
			if ( char.IsLower( firstChar ) )
			{
				return value;
			}

			return Char.ToLowerInvariant( firstChar ) + value.Substring( 1 );
		}
	}
}
