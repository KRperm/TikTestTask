using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tikTestTask
{
    //Экземпляры класса MenuForm Содержат информацию о пунктах меню и некоторых текстовых полях
    class MenuForm
    {
        public string Header;
        //При вводе с клавиатуры, это значение заполняется
        public string Input = "";
        //Для вывода больших объемов информации
        public string Info = "";
        private string MenuWarning = "";
        private string MenuConfirm = "";
        //Указатель на элемент меню
        private int MenuCursor = 0;
        private bool MenuHaveInput;
        //Здесь хранятся все пункты меню
        private List<MenuItem> MenuItems = new List<MenuItem>();

        public string Warning
        {
            get
            {
                return MenuWarning;
            }
            set
            {
                MenuWarning = value;
                MenuConfirm = "";
            }
        }
        public string Confirm
        {
            get
            {
                return MenuConfirm;
            }
            set
            {
                MenuWarning = "";
                MenuConfirm = value;
            }
        }

        public List<MenuItem> Items
        {
            get
            {
                return MenuItems;
            }
        }

        public int Cursor
        {
            get
            {
                return MenuCursor;
            }
            set
            {
                if (value >= MenuItems.Count)
                {
                    MenuCursor = 0;
                } else if (value < 0)
                {
                    MenuCursor = MenuItems.Count - 1;
                }
                else
                {
                    MenuCursor = value;
                }
            }
        }

        public string CurrentItemDescription
        {
            get
            {
                return MenuItems[MenuCursor].Description;
            }
        }
        public string CurrentItemName
        {
            get
            {
                return MenuItems[MenuCursor].Name;
            }
        }
        public bool HaveInput
        {
            get
            {
                return MenuHaveInput;
            }
        }

        public MenuForm(string header, bool haveInput)
        {
            Header = header;
            MenuHaveInput = haveInput;
        }

        public MenuItem AddItem(string name, string description, MenuItem.FormAction action)
        {
            MenuItem item = new MenuItem(name, description, action, this);
            MenuItems.Add(item);
            return item;
        }

        //Исполняет функцию пункта меню, на которую указывает указатель
        public void ExecuteUnderCursor()
        {
            MenuItems[MenuCursor].Execute();
        }
    }
}
