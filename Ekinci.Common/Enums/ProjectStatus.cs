using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ekinci.Common.Enums
{
    public enum ProjectStatus
    {
        [Description("Devam Eden Proje")]
        Continue=0,
        [Description("Planlanan Projeler")]
        Planning=1,
        [Description("Biten Projeler")]
        Ending=2,


    }
}
