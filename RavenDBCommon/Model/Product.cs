using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenDBApplication.Model
{
	public class Product
	{
		public String Id { get; private set; }
		public String Name { get; set; }
		public List<ProductAttribute> Attributes { get; set; }
	}

	public class ProductAttribute
	{
		public String Name { get; set; }

		public AttributeValue Value { get; set; }

		internal Object FieldValue
		{
			get
			{
				return this.Value.GetValue();
			}
		}
	}

	public abstract class AttributeValue
	{
		public abstract Object GetValue();
	}

	public class AttributeValue<T> : AttributeValue
	{
		public T Value { get; set; }

		public override object GetValue()
		{
			return this.Value;
		}

		public override string ToString()
		{
			return this.Value.ToString();
		}
	}
}
