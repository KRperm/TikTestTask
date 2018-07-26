using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tikTestTask
{
    //Экземпляры класса MenuItem содержат информацию о пункте меню и привязаной к нему действию(FormAction)
    class MenuItem
    {
        public delegate void FormAction(MenuForm Menu);
        
        private string ItemName;
        private string ItemDescription;
        private FormAction ItemAction;
        private MenuForm ItemParent;

        public string Name
        {
            get
            {
                return ItemName;
            }
        }
        public string Description
        {
            get
            {
                return ItemDescription;
            }
        }

        public MenuItem(string name, string description, FormAction action, MenuForm parent)
        {
            ItemName = name;
            ItemDescription = description;
            ItemAction = action;
            ItemParent = parent;
        }

        public void Execute()
        {
            ItemAction(ItemParent);
        }


    }
}
