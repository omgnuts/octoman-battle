using System;
using System.Threading;

namespace common
{
    public class TryLock : IDisposable
    {
        private readonly object _locked;

        private bool _hasLock;

        private TryLock(object obj)
        {
            if (!Monitor.TryEnter(obj)) return;
            _hasLock = true;
            _locked = obj;
        }

        public void Dispose()
        {
            if (!_hasLock) return;
            Monitor.Exit(_locked);
            _hasLock = false;
        }

        public static void Execute(object objLock, Action action)
        {
            if (new TryLock(objLock)._hasLock) action();
        }
        
    }
}