﻿using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesonet.WindowsParty.Services
{
    class Log4netService : ILog
    {
        #region Fields
        private readonly log4net.ILog _innerLogger;
        #endregion

        #region Constructors
        public Log4netService(Type type)
        {
            _innerLogger = log4net.LogManager.GetLogger(type);
        }
        #endregion

        #region ILog Members
        public void Error(Exception exception)
        {
            _innerLogger.Error(exception.Message, exception);
        }
        public void Info(string format, params object[] args)
        {
            _innerLogger.InfoFormat(format, args);
        }
        public void Warn(string format, params object[] args)
        {
            _innerLogger.WarnFormat(format, args);
        }
        #endregion
    }
}
