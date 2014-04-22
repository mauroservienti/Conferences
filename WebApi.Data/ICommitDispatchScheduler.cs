using System.Collections.Generic;
using WebApi.Data.Commits;

namespace WebApi.Data
{
    public interface ICommitDispatchScheduler
    {
		bool IsSynchronous { get; }
		void ScheduleDispatch( Commit commit );
		void ScheduleDispatch( IEnumerable<Commit> commits );
    }
}
