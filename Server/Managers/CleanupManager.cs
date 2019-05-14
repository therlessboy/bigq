﻿using BigQ.Core;
using System; 
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BigQ.Server.Managers
{
    internal class CleanupManager : IDisposable
    {
        #region Public-Members

        #endregion

        #region Private-Members

        private bool _Disposed = false;
         
        private ServerConfiguration _Config;
        private readonly object _ActiveSendMapLock = new object();
        private Dictionary<string, DateTime> _ActiveSendMap = new Dictionary<string, DateTime>();

        private CancellationTokenSource _TokenSource;
        private CancellationToken _Token;

        #endregion

        #region Constructors-and-Factories

        /// <summary>
        /// Instantiate the object.
        /// </summary> 
        /// <param name="config">ServerConfiguration instance.</param>
        /// <param name="activeSendMap">Active send map instance.</param>
        public CleanupManager( 
            ServerConfiguration config, 
            Dictionary<string, DateTime> activeSendMap)
        { 
            if (config == null) throw new ArgumentNullException(nameof(config));
             
            _Config = config;
            _ActiveSendMap = activeSendMap;

            _TokenSource = new CancellationTokenSource();
            _Token = _TokenSource.Token;

            Task.Run(() => CleanupTask(), _Token);
        }

        #endregion

        #region Public-Methods

        /// <summary>
        /// Tear down and dispose of background workers.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Private-Methods

        protected virtual void Dispose(bool disposing)
        {
            if (_Disposed)
            {
                return;
            }

            if (disposing)
            {
                _TokenSource.Cancel();
                _TokenSource.Dispose();
            }

            _Disposed = true;
        }

        private void CleanupTask()
        {
            try
            {
                bool firstRun = true;

                while (true)
                {
                    #region Wait

                    if (!firstRun)
                    {
                        Task.Delay(5000).Wait();
                    }
                    else
                    {
                        firstRun = false;
                    }

                    #endregion

                    #region Process

                    lock (_ActiveSendMapLock)
                    {
                        foreach (KeyValuePair<string, DateTime> curr in _ActiveSendMap)
                        {
                            if (String.IsNullOrEmpty(curr.Key)) continue;
                            if (DateTime.Compare(DateTime.Now.ToUniversalTime(), curr.Value) > 0)
                            {
                                if (_ActiveSendMap.ContainsKey(curr.Key)) _ActiveSendMap.Remove(curr.Key);
                            }
                        }
                    }

                    #endregion
                }
            }
            catch (ThreadAbortException)
            {
                // do nothing
            }
            catch (Exception)
            {

            }
        }

        #endregion
    }
}