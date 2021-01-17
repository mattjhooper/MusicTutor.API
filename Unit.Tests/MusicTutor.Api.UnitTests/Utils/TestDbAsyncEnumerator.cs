using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MusicTutor.Api.UnitTests.Utils
{
    public class TestDbAsyncEnumerator<T> 
    {
        private readonly IEnumerator<T> _inner;

        public TestDbAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public void Dispose()
        {
            // Do not dispose the inner enumerator, since it might be enumerated again, 
            // reset it instead
            _inner.Reset();
        }

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_inner.MoveNext());
        }

        public T Current
        {
            get { return _inner.Current; }
        }

        object IDbAsyncEnumerator.Current
        {
            get { return Current; }
        }
    }
}