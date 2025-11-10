using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elixir.Management
{
    public class Category
    {
        public string name { get; set; }
        public enum IconType { Setting, Person, Moving, Visual, World, Room, Info, Fun, Splash }
        public IconType Icon { get; set; }

        public Module[] buttons { get; set; }

        public Category(string name, IconType iconType, Module[] buttons)
        {
            this.name = name;
            this.buttons = buttons;
            this.Icon = iconType;
        }
    }
}
