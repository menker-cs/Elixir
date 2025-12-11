using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elixir.Components
{
    public class Category
    {
        public string name { get; set; }

        public Module[] buttons { get; set; }

        public Category(string name, Module[] buttons)
        {
            this.name = name;
            this.buttons = buttons;
        }
    }
}
