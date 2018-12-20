using Caliburn.Micro;
using Serilog;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.Destructurers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesonet.WindowsParty.Interfaces;

namespace Tesonet.WindowsParty.Services
{
    public class SerilogLogService : ILog
    {
        ILogger _allLogger;
        ILogger _errorlogger;

        public SerilogLogService(Type type, IConfigurationService configuration)
        {
            _allLogger = new LoggerConfiguration()
                .WriteTo.Async(config => config.Console())
                .WriteTo.Async(config => config.RollingFile(configuration.TraceLogFile, retainedFileCountLimit: 1, fileSizeLimitBytes: 5 * 1000 * 1000, shared: true))
                .MinimumLevel.Debug().CreateLogger();
            _errorlogger = new LoggerConfiguration().Enrich.WithExceptionDetails(new DestructuringOptionsBuilder().WithDefaultDestructurers().WithRootName("Exception"))
                .WriteTo.Async(config => config.RollingFile(configuration.ErrorLogFile, retainedFileCountLimit: 1, fileSizeLimitBytes: 5 * 1000 * 1000, shared: true))
                .MinimumLevel.Error().CreateLogger();
        }

        public void Error(Exception exception)
        {
            _allLogger.Error(exception, "Exception");
            _errorlogger.Error(exception, "Exception");
        }

        public void Info(string format, params object[] args)
        {
            _allLogger.Information(format, args);
        }

        public void Warn(string format, params object[] args)
        {
            _allLogger.Warning(format, args);
        }
    }
}
