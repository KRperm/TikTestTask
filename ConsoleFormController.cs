using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tikTestTask
{
    //Данный класс управляет формами меню
    class ConsoleFormController
    {
        //Все формы хранятся в коллекции 
        private Dictionary<string, MenuForm> Forms = new Dictionary<string, MenuForm>();
        //Форма, с которой работает класс в данный момент
        private MenuForm CurrentForm;
        private bool IsControllRunning;

        public MenuForm Form
        {
            get
            {
                return CurrentForm;
            }
        }

        public ConsoleFormController()
        {
            Console.CursorVisible = false;
        }
        public void Exit()
        {
            IsControllRunning = false;
        }

        public MenuForm AddMenu(string name, string header, bool haveInput)
        {
            MenuForm menu = new MenuForm(header, haveInput);
            Forms.Add(name, menu);
            return menu;
        }

        //Выводит на консоль указанную форму
        public void Draw(MenuForm formToDraw)
        {
            int line = 1;
            Console.Clear();
            Console.ResetColor();
            if (formToDraw.Header.Length > 0)
            {
                Console.SetCursorPosition(1, line++);
                Console.Write(formToDraw.Header);
            }
            if (formToDraw.CurrentItemDescription.Length > 0)
            {
                Console.SetCursorPosition(1, line++);
                Console.Write(formToDraw.CurrentItemDescription);
            }
            if (formToDraw.Warning.Length > 0)
            {
                Console.SetCursorPosition(1, line++);
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write(formToDraw.Warning);
                Console.ResetColor();
            }
            if (formToDraw.Confirm.Length > 0)
            {
                Console.SetCursorPosition(1, line++);
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Green;
                Console.Write(formToDraw.Confirm);
                Console.ResetColor();
            }
            if (formToDraw.HaveInput || formToDraw.Items.Count > 0)
            {
                line++;
            }
            if (formToDraw.HaveInput)
            {
                Console.SetCursorPosition(1, line++);
                Console.Write("> {0}", formToDraw.Input);
            }
            for(int i = 0; i < formToDraw.Items.Count; i++)
            {
                Console.SetCursorPosition(1, line++);
                if (formToDraw.Cursor == i)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.Write(formToDraw.Items[i].Name);
                    Console.ResetColor();
                }else
                {
                    Console.Write(formToDraw.Items[i].Name);
                }
            }
            if (formToDraw.Info.Length > 0)
            {
                line++;
                Console.SetCursorPosition(0, line++);
                Console.Write(formToDraw.Info);
            }
        }

        public bool ChangeForm(string name)
        {
            bool result = Forms.TryGetValue(name, out CurrentForm);
            CurrentForm.Input = "";
            CurrentForm.Warning = "";
            CurrentForm.Cursor = 0;
            return result;
        }

        //Функция, которая запускает работу с меню
        public void Start(string name)
        {
            IsControllRunning = true;
            ChangeForm(name);

            while (IsControllRunning)
            {
                Draw(CurrentForm);
                ConsoleKeyInfo input = Console.ReadKey();
                switch (input.Key)
                {
                    case ConsoleKey.Enter:
                        CurrentForm.ExecuteUnderCursor();
                        break;
                    case ConsoleKey.UpArrow:
                        CurrentForm.Cursor--;
                        break;
                    case ConsoleKey.DownArrow:
                        CurrentForm.Cursor++;
                        break;
                    case ConsoleKey.Backspace:
                        if (CurrentForm.HaveInput && CurrentForm.Input.Length > 0)
                        {
                            CurrentForm.Input = CurrentForm.Input.Substring(0, CurrentForm.Input.Length - 1);
                        }
                        break;
                    default:
                        if (CurrentForm.HaveInput)
                        {
                            CurrentForm.Input += input.KeyChar;
                        }
                        break;
                }
            }
        }
    }
}
