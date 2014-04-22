using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace WebApiServer.Infrastructure
{
	public class WindsorJasonContainerProxy : Jason.Configuration.IJasonContainer
	{
		IWindsorContainer container;

		public WindsorJasonContainerProxy( IWindsorContainer container )
		{
			this.container = container;
		}

		public void RegisterAsTransient( IEnumerable<Type> contracts, Type implementation )
		{
			this.container.Register( Component.For( contracts ).ImplementedBy( implementation ).LifestyleTransient().IsFallback() );
		}

		public void RegisterAsSingleton( IEnumerable<Type> contracts, Type implementation )
		{
			this.container.Register( Component.For( contracts ).ImplementedBy( implementation ).LifestyleSingleton().IsFallback() );
		}

		public void RegisterInstance( IEnumerable<Type> contracts, object instance )
		{
			this.container.Register( Component.For( contracts ).Instance( instance ).LifestyleSingleton().IsFallback() );
		}

		public void RegisterTypeAsTransient( Type implementation )
		{
			this.container.Register( Component.For( implementation ).LifestyleTransient().IsFallback() );
		}

		public void RegisterTypeAsSingleton( Type implementation )
		{
			this.container.Register( Component.For( implementation ).LifestyleSingleton().IsFallback() );
		}

		public IEnumerable<T> ResolveAll<T>()
		{
			return this.container.ResolveAll<T>();
		}

		public T Resolve<T>()
		{
			return this.container.Resolve<T>();
		}

		public IEnumerable<object> ResolveAll( Type t )
		{
			return this.container.ResolveAll( t ).Cast<Object>();
		}

		public object Resolve( Type t )
		{
			return this.container.Resolve( t );
		}

		public void Release( object instance )
		{
			this.container.Release( instance );
		}

		public object GetService( Type serviceType )
		{
			if ( this.container.Kernel.HasComponent( serviceType ) )
			{
				return this.container.Resolve( serviceType );
			}

			return null;
		}

		public bool IsRegistered( Type type )
		{
			return this.container.Kernel.HasComponent( type );
		}

		public bool IsRegistered<TType>()
		{
			return this.IsRegistered( typeof( TType ) );
		}
	}
}
