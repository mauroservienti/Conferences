using System;

namespace WebApi.Infrastructure
{
	public interface IOperationContextManager
	{
		IOperationContext GetCurrent();
	}

	public interface IOperationContext : IDisposable
	{
		IOperationContext ForOperation( String correlationId );
		String CorrelationId { get; }

		void Add( string key, Object value );
		T Get<T>( string key );
	}
}
