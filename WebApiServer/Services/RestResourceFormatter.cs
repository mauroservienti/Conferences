using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Http.Routing;
using WebApi.Commands;
using WebApi.Data.Companies.Events;
using WebApi.Data.Indexes;
using WebApiServer.ComponentModel;
using Topics.Radical.Reflection;
using Topics.Radical;
using NServiceBus;
using Raven.Client;
using WebApi.Data;
using WebApiServer.Models;
using WebApi.Data.Views;
using WebApi.Data.Commits;
using System.Collections;

namespace WebApiServer.Services
{
	public class RestResourceFormatter
	{
		class DefaultResourceFormatter : IResourceFormatter
		{
			public dynamic AsResource( object obj, UrlHelper urlHelper, Func<object, IResourceFormatter> subformatterFinder )
			{
				return obj.ToDynamic();
			}

			public bool CanHandleObject( object obj )
			{
				return false;
			}
		}


		IResourceFormatter[] formatters;
		IResourceFormatter defaultFormatter = new DefaultResourceFormatter();

		public RestResourceFormatter( IResourceFormatter[] formatters )
		{
			this.formatters = formatters;
		}

		public dynamic AsResource<T>( T obj, UrlHelper urlHelper )
		{
			if ( obj == null ) 
			{
				return null;
			}

			Func<Object, IResourceFormatter> finder = element =>
			{
				var found = this.formatters.FirstOrDefault( f => f.CanHandleObject( element ) );
				if ( found == null )
				{
					return this.defaultFormatter;
				}

				return found;
			};

			var formatter = finder( obj );

			return formatter.AsResource( obj, urlHelper, finder );
		}
	}

	class MomentResourceFormatter : IResourceFormatter
	{
		public dynamic AsResource( object obj, UrlHelper urlHelper, Func<Object, IResourceFormatter> subformatterFinder )
		{
			var moment = ( History_Stream.Moment )obj;
			var dataType = moment.Data.GetType();

			var dataFormatter = subformatterFinder( moment.Data );
			var dataResource = dataFormatter.AsResource( moment.Data, urlHelper, subformatterFinder );

			var momentResource = moment.ToDynamic( p => p.Name != "Data" );
			momentResource.Data = dataResource;

			switch ( moment.MomentType )
			{
				case History_Stream.MomentType.Event:
					var segments = moment.Id.Split( '/' );
					momentResource.Ref = urlHelper.Link( "CommitsEventsApiWithId", new
					{
						commitId = segments.Skip( 1 ).Take( 1 ).Single(),
						eventId = segments.Last()
					} );
					break;

				case History_Stream.MomentType.Command:
					momentResource.Ref = urlHelper.Link( "DefaultApiWithId", new
					{
						controller = "CommandRequests",
						id = moment.Id.Split( '/' ).Last()
					} );
					break;

				case History_Stream.MomentType.Response:
					momentResource.Ref = urlHelper.Link( "DefaultApiWithId", new
					{
						controller = "CommandResponses",
						id = moment.Id.Split( '/' ).Last()
					} );
					break;

				case History_Stream.MomentType.Exception:
					momentResource.Ref = urlHelper.Link( "DefaultApiWithId", new
					{
						controller = "CommandExceptions",
						id = moment.Id.Split( '/' ).Last()
					} );
					break;

				default:
					throw new NotSupportedException();
			}

			return momentResource;
		}

		public bool CanHandleObject( Object obj )
		{
			return obj is History_Stream.Moment;
		}
	}

	class DomainEventResourceFormatter : IResourceFormatter
	{
		public dynamic AsResource( object obj, UrlHelper urlHelper, Func<Object, IResourceFormatter> subformatterFinder )
		{
			var @event = ( IDomainEvent )obj;
			var resource = obj.ToDynamic();
			resource.Ref = urlHelper.Link( "DefaultApiWithId", new
			{
				controller = @event.AggregateId.Split( '/' ).First(),
				id = @event.AggregateId.Split( '/' ).Last()
			} );

			return resource;
		}

		public bool CanHandleObject( Object obj )
		{
			return obj is IDomainEvent;
		}
	}

	class CommitResourceFormatter : IResourceFormatter
	{
		public dynamic AsResource( object obj, UrlHelper urlHelper, Func<Object, IResourceFormatter> subformatterFinder )
		{
			var commit = ( Commit )obj;
			var resource = obj.ToDynamic( p => p.Name != "Events" );

			resource.Self = urlHelper.Link( "DefaultApiWithId", new
			{
				controller = commit.Id.Split( '/' ).First(),
				id = commit.Id.Split( '/' ).Last()
			} );

			var temp = new List<dynamic>();
			foreach ( var @event in commit.Events )
			{
				var eventFormatter = subformatterFinder( @event );
				var eventResource = eventFormatter.AsResource( @event, urlHelper, subformatterFinder );

				var segments = @event.Id.Split( '/' );
				eventResource.Self = urlHelper.Link( "CommitsEventsApiWithId", new
				{
					commitId = segments.Skip( 1 ).Take( 1 ).Single(),
					eventId = segments.Last()
				} );

				temp.Add( eventResource );
			}

			resource.Events = temp;

			return resource;
		}

		public bool CanHandleObject( Object obj )
		{
			return obj is Commit;
		}
	}

	abstract class AbstractPagedResultsResourceFormatter : IResourceFormatter
	{
		protected abstract void DefineLinks( object obj, dynamic vmResource, UrlHelper urlHelper );

		public virtual dynamic AsResource( object obj, UrlHelper urlHelper, Func<object, IResourceFormatter> subformatterFinder )
		{
			var vmResource = obj.ToDynamic( p => p.Name != "Results" );
			var temp = new List<dynamic>();
			foreach ( var item in this.GetResults( obj ) )
			{
				var itemFormatter = subformatterFinder( item );
				var itemResource = itemFormatter.AsResource( item, urlHelper, subformatterFinder );
				temp.Add( itemResource );
			}

			vmResource.Results = temp;

			this.DefineLinks( obj, vmResource, urlHelper );

			return vmResource;
		}

		protected abstract IEnumerable<Object> GetResults( object obj );

		public abstract bool CanHandleObject( object obj );
	}

	class MomentPagedResultsResourceFormatter : AbstractPagedResultsResourceFormatter
	{
		protected override void DefineLinks( object obj, dynamic vmResource, UrlHelper urlHelper )
		{
			var vm = ( PagedResultsViewModel<History_Stream.Moment> )obj;

			vmResource.Self = urlHelper.Link( "DefaultApiGet", new { controller = "History", p = vm.PageIndex, s = vm.PageSize } );
			vmResource.Prev = ( vm.TotalResults > 0 && vm.PageIndex > 0 ) ? urlHelper.Link( "DefaultApiGet", new { controller = "History", p = vm.PageIndex - 1, s = vm.PageSize } ) : null;
			vmResource.Next = ( vm.TotalResults > 0 && vm.TotalPages < vm.PageIndex + 1 ) ? urlHelper.Link( "DefaultApiGet", new { controller = "History", p = vm.PageIndex + 1, s = vm.PageSize } ) : null;	
		}

		protected override IEnumerable<object> GetResults( object obj )
		{
			var vm = ( PagedResultsViewModel<History_Stream.Moment> )obj;
			var items = vm.Results;

			return items;
		}

		public override bool CanHandleObject( Object obj )
		{
			return obj is PagedResultsViewModel<History_Stream.Moment>;
		}
	}

	class CompanyViewPagedResultsResourceFormatter : AbstractPagedResultsResourceFormatter
	{
		protected override void DefineLinks( object obj, dynamic vmResource, UrlHelper urlHelper )
		{
			var vm = ( PagedResultsViewModel<CompanyView> )obj;

			vmResource.Self = urlHelper.Link( "DefaultApiGet", new { controller = "Companies", p = vm.PageIndex, s = vm.PageSize } );
			vmResource.Prev = ( vm.TotalResults > 0 && vm.PageIndex > 0 ) ? urlHelper.Link( "DefaultApiGet", new { controller = "Companies", p = vm.PageIndex - 1, s = vm.PageSize } ) : null;
			vmResource.Next = ( vm.TotalResults > 0 && vm.TotalPages < vm.PageIndex + 1 ) ? urlHelper.Link( "DefaultApiGet", new { controller = "Companies", p = vm.PageIndex + 1, s = vm.PageSize } ) : null;
		}

		protected override IEnumerable<object> GetResults( object obj )
		{
			var vm = ( PagedResultsViewModel<CompanyView> )obj;
			var items = vm.Results;

			return items;
		}

		public override bool CanHandleObject( Object obj )
		{
			return obj is PagedResultsViewModel<CompanyView>;
		}
	}

	class PersonViewPagedResultsResourceFormatter : AbstractPagedResultsResourceFormatter
	{
		protected override void DefineLinks( object obj, dynamic vmResource, UrlHelper urlHelper )
		{
			var vm = ( PagedResultsViewModel<PersonView> )obj;

			vmResource.Self = urlHelper.Link( "DefaultApiGet", new { controller = "People", p = vm.PageIndex, s = vm.PageSize } );
			vmResource.Prev = ( vm.TotalResults > 0 && vm.PageIndex > 0 ) ? urlHelper.Link( "DefaultApiGet", new { controller = "People", p = vm.PageIndex - 1, s = vm.PageSize } ) : null;
			vmResource.Next = ( vm.TotalResults > 0 && vm.TotalPages < vm.PageIndex + 1 ) ? urlHelper.Link( "DefaultApiGet", new { controller = "People", p = vm.PageIndex + 1, s = vm.PageSize } ) : null;
		}

		protected override IEnumerable<object> GetResults( object obj )
		{
			var vm = ( PagedResultsViewModel<PersonView> )obj;
			var items = vm.Results;

			return items;
		}

		public override bool CanHandleObject( Object obj )
		{
			return obj is PagedResultsViewModel<PersonView>;
		}
	}

	class SearchResultsResourceFormatter : AbstractPagedResultsResourceFormatter
	{
		protected override void DefineLinks( object obj, dynamic vmResource, UrlHelper urlHelper )
		{
			var vm = ( SearchResultsViewModel<Parties_Search_FullText.SearchResult> )obj;

			vmResource.Self = urlHelper.Link( "DefaultApiGet", new { controller = "Search", p = vm.PageIndex, s = vm.PageSize, q = vm.Query } );
			vmResource.Prev = ( vm.TotalResults > 0 && vm.PageIndex > 0 ) ? urlHelper.Link( "DefaultApiGet", new { controller = "Search", p = vm.PageIndex - 1, s = vm.PageSize, q = vm.Query } ) : null;
			vmResource.Next = ( vm.TotalResults > 0 && vm.TotalPages < vm.PageIndex + 1 ) ? urlHelper.Link( "DefaultApiGet", new { controller = "Search", p = vm.PageIndex + 1, s = vm.PageSize, q = vm.Query } ) : null;
		}

		protected override IEnumerable<object> GetResults( object obj )
		{
			var vm = ( SearchResultsViewModel<Parties_Search_FullText.SearchResult> )obj;
			var items = vm.Results;

			return items;
		}

		public override bool CanHandleObject( Object obj )
		{
			return obj is SearchResultsViewModel<Parties_Search_FullText.SearchResult>;
		}
	}

	class SearchResourceFormatter : IResourceFormatter
	{
		public dynamic AsResource( object obj, UrlHelper urlHelper, Func<Object, IResourceFormatter> subformatterFinder )
		{
			var @event = ( Parties_Search_FullText.SearchResult )obj;
			var resource = obj.ToDynamic();
			resource.Ref = urlHelper.Link( "DefaultApiWithId", new
			{
				controller = @event.Id.Split( '/' ).First(),
				id = @event.Id.Split( '/' ).Last()
			} );

			return resource;
		}

		public bool CanHandleObject( Object obj )
		{
			return obj is Parties_Search_FullText.SearchResult;
		}
	}

	class CompanyViewResourceFormatter : IResourceFormatter
	{
		public dynamic AsResource( object obj, UrlHelper urlHelper, Func<Object, IResourceFormatter> subformatterFinder )
		{
			var view = ( CompanyView )obj;
			var resource = obj.ToDynamic( p => p.Name != "Id" );
			resource.Self = urlHelper.Link( "DefaultApiWithId", new
			{
				controller = view.Id.Split( '/' ).First(),
				id = view.Id.Split( '/' ).Last()
			} );

			return resource;
		}

		public bool CanHandleObject( Object obj )
		{
			return obj is CompanyView;
		}
	}

	class PersonViewResourceFormatter : IResourceFormatter
	{
		public dynamic AsResource( object obj, UrlHelper urlHelper, Func<Object, IResourceFormatter> subformatterFinder )
		{
			var view = ( PersonView )obj;
			var resource = obj.ToDynamic( p => p.Name != "Id" );
			resource.Self = urlHelper.Link( "DefaultApiWithId", new
			{
				controller = view.Id.Split( '/' ).First(),
				id = view.Id.Split( '/' ).Last()
			} );

			return resource;
		}

		public bool CanHandleObject( Object obj )
		{
			return obj is PersonView;
		}
	}

	class CommandResourceFormatter : IResourceFormatter
	{
		public dynamic AsResource( object obj, UrlHelper urlHelper, Func<Object, IResourceFormatter> subformatterFinder )
		{
			return obj.ToDynamic();
		}

		public bool CanHandleObject( Object obj )
		{
			var t = obj.GetType();
			if ( !t.IsAbstract && t.Namespace != null && !t.IsNested )
			{
				return t.Is<ICommand>() || ( t.Namespace != null && t.Namespace.IsLike( "WebApi*.Commands" ) && t.Name.EndsWith( "Command" ) );
			}

			return false;
		}
	}

	class EnumerableResourceFormatter : IResourceFormatter
	{
		public dynamic AsResource( object obj, UrlHelper urlHelper, Func<Object, IResourceFormatter> subformatterFinder )
		{
			var enumerable = ( IEnumerable )obj;

			var resource = new List<dynamic>();
			foreach ( var item in enumerable )
			{
				var itemFormatter = subformatterFinder( item );
				var itemResource = itemFormatter.AsResource( item, urlHelper, subformatterFinder );

				resource.Add( itemResource );
			}

			return resource;
		}

		public bool CanHandleObject( Object obj )
		{
			var t = obj.GetType();
			if ( t.IsGenericType )
			{
				var x = typeof( IEnumerable ).IsAssignableFrom( t.GetGenericTypeDefinition() );
				return x;
			}
			return false;
		}
	}

	//class DynamicList<T> : DynamicObject, IList<T>
	//{
	//	List<T> storage = new List<T>();
	//	Dictionary<String, Object> dynamicData = new Dictionary<string, object>();

	//	public override bool TryGetMember( GetMemberBinder binder, out object result )
	//	{
	//		if ( dynamicData.ContainsKey( binder.Name ) ) 
	//		{
	//			result = dynamicData[ binder.Name ];
	//			return true;
	//		}

	//		return base.TryGetMember( binder, out result );
	//	}

	//	public override bool TrySetMember( SetMemberBinder binder, object value )
	//	{
	//		if ( dynamicData.ContainsKey( binder.Name ) )
	//		{
	//			dynamicData[ binder.Name ] = value;
	//		}
	//		else
	//		{
	//			dynamicData.Add( binder.Name, value );
	//		}

	//		return true;
	//	}

	//	public override IEnumerable<string> GetDynamicMemberNames()
	//	{
	//		return this.dynamicData.Keys.OfType<String>();
	//	}

	//	public int IndexOf( T item )
	//	{
	//		return this.storage.IndexOf( item );
	//	}

	//	public void Insert( int index, T item )
	//	{
	//		this.storage.Insert( index, item );
	//	}

	//	public void RemoveAt( int index )
	//	{
	//		this.storage.RemoveAt( index );
	//	}

	//	public T this[ int index ]
	//	{
	//		get
	//		{
	//			return this.storage[ index ];
	//		}
	//		set
	//		{
	//			this.storage[ index ] = value;
	//		}
	//	}

	//	public void Add( T item )
	//	{
	//		this.storage.Add( item );
	//	}

	//	public void Clear()
	//	{
	//		this.storage.Clear();
	//	}

	//	public bool Contains( T item )
	//	{
	//		return this.storage.Contains( item );
	//	}

	//	public void CopyTo( T[] array, int arrayIndex )
	//	{
	//		this.storage.CopyTo( array, arrayIndex );
	//	}

	//	public int Count
	//	{
	//		get { return this.storage.Count; }
	//	}

	//	public bool IsReadOnly
	//	{
	//		get { return ( ( IList )this.storage ).IsReadOnly; }
	//	}

	//	public bool Remove( T item )
	//	{
	//		return ( this.storage ).Remove( item );
	//	}

	//	public IEnumerator<T> GetEnumerator()
	//	{
	//		return this.storage.GetEnumerator();
	//	}

	//	IEnumerator IEnumerable.GetEnumerator()
	//	{
	//		return ( ( IEnumerable )this.storage ).GetEnumerator();
	//	}
	//}

	public static class ResourcesExtensions
	{
		public static dynamic ToDynamic<T>( this T value )
		{
			return value.ToDynamic( p => true );
		}

		public static dynamic ToDynamic<T>( this T value, Func<PropertyDescriptor, Boolean> propertyFilter )
		{
			IDictionary<string, object> expando = new ExpandoObject();

			foreach ( var property in TypeDescriptor.GetProperties( value.GetType() ).OfType<PropertyDescriptor>().Where( propertyFilter ) )
			{
				expando.Add( property.Name, property.GetValue( value ) );
			}

			return expando as ExpandoObject;
		}
	}
}