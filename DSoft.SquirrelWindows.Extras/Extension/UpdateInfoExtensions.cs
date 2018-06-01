using Squirrel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Squirrel
{
    public static class UpdateInfoExtensions
    {
        public static bool NeedsUpdate(this UpdateInfo updates)
        {
            return updates.ReleasesToApply.Count > 0;
        }
    }
}
