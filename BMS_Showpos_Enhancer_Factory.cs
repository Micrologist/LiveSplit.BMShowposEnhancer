using LiveSplit.Model;
using LiveSplit.UI.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: ComponentFactory(typeof(BMS_Showpos_Enhancer_Factory))]

namespace LiveSplit.UI.Components
{
    class BMS_Showpos_Enhancer_Factory : IComponentFactory
    {
        public string ComponentName
        {
            get { return "Black Mesa Showpos Enhancer"; }
        }

        public string Description
        {
            get { return ""; }
        }

        public ComponentCategory Category
        {
            get { return ComponentCategory.Control; }
        }

        public IComponent Create(LiveSplitState state)
        {
            return new BMS_Showpos_Enhancer(state);
        }

        public string UpdateName
        {
            get { return this.ComponentName; }
        }

        public string UpdateURL
        {
            get { return null; }
        }

        public Version Version
        {
            get { return Version.Parse("1.0.0"); }
        }

        public string XMLURL
        {
            get { return null; }
        }
    }
}
