﻿using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CiagoarM.Commons
{
    public class BaseModel :ObservableObject
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void LogInfo(string Message, [CallerMemberName] string propertyName = "")
        {
            _log.Info($"[{propertyName}]  {Message} ");
        }

        public void LogError(string Message, [CallerMemberName] string propertyName = "")
        {
            _log.Error($"[{propertyName}]★{Message} ");
        }

        public void LogException(string Message, [CallerMemberName] string propertyName = "")
        {
            _log.Fatal($"[{propertyName}]★★★{Message} ");
        }

    }
}
