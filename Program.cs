using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace tikTestTask
{
    class Program
    {
        static private TagStorage Storage = new TagStorage();
        static private ConsoleFormController Controller = new ConsoleFormController();
        static private TagItem bufferItem;
        static private string bufferName;

        static void Main(string[] args)
        {
            //Инициализация формы главного меню
            MenuForm menu = Controller.AddMenu("Main", "Премещение - стрелки вверх и вниз. Чтобы выбрать действие нажмите на Enter", false);
            menu.AddItem("Загрузить из файла", "Загрузить дерево тегов из XML файла", LoadXML);
            menu.AddItem("Сохранить в файл", "Загрузить дерево тегов в XML файл", SaveXML);
            menu.AddItem("Вывести", "Вывод построчного списка тэгов", OutputAllTags);
            menu.AddItem("Добавить", "Добавление нового тэга", ToAddTagForm);
            menu.AddItem("Переименовать", "Переименовать существующий тег", ToRenameTagForm);
            menu.AddItem("Удалить", "Удалить тег по полному имени", ToRemoveTagForm);
            menu.AddItem("Выйти", "Закрыть это приложение", ExitFromProgram);

            //Инициализация формы удаления элементов
            menu = Controller.AddMenu("RemoveForm", "Введите имя тега. Нажмите backspace, чтобы удалить последний введенный символ", true);
            menu.AddItem("Ввод", "Начать поиск тега по введенному имени и его последующее удаление", RemoveFormSearchAndRemoveTag);
            menu.AddItem("Отмена", "Вернуться в главное меню", ExitToMainMenu);

            //Инициализация форм для переименования элементов
            //Данная форма ищет тег
            menu = Controller.AddMenu("RenameForm", "Введите путь до тега без элемента Root. Нажмите backspace, чтобы удалить последний введенный символ", true);
            menu.AddItem("Ввод", "Введите полный путь для тега, который хотите переименовать", RenameTagFormInputPath);
            menu.AddItem("Отмена", "Вернуться в главное меню", ExitToMainMenu);
            //Данная форма переименовывает найденый тег
            menu = Controller.AddMenu("RenameForm1InputName", "Введите имя", true);
            menu.AddItem("Ввод", "Введите новое имя тега", RenameTagFormInputNameAndRenameTag);
            menu.AddItem("Отмена", "Вернуться в главное меню", ExitToMainMenu);


            //Инициализация форм для добавления элементов
            //Данная форма ищет родителя
            menu = Controller.AddMenu("AddTagForm", "Введите имя родителя нового тега. Нажмите backspace, чтобы удалить последний введенный символ", true);
            menu.AddItem("Ввод", "Введите имя тега, который будет родителем для нового тега", AddTagFormSearchParent);
            menu.AddItem("Отмена", "Вернуться в главное меню", ExitToMainMenu);
            //Данная форма для ввода имени нового имени
            menu = Controller.AddMenu("AddTagForm1InputChildName", "Введите имя", true);
            menu.AddItem("Ввод", "Введите имя для нового тега", AddTagFormInputChildName);
            menu.AddItem("Отмена", "Вернуться в главное меню", ExitToMainMenu);
            //данная форма для выбора типа элемента и создания нового тега
            menu = Controller.AddMenu("AddTagForm2ChoseType", "Выберите тип нового элемента", false);
            menu.AddItem("None", "Без значения", AddTagFormAddNone);
            menu.AddItem("Bool", "Логический тип. начальное значение - False", AddTagFormAddBool);
            menu.AddItem("Double", "Число с плавающей точкой. начальное значение - 0", AddTagFormAddDouble);
            menu.AddItem("Int", "Целочисленное число. начальное значение - 0", AddTagFormAddInt);
            menu.AddItem("Отмена", "Вернуться в главное меню", ExitToMainMenu);

            Controller.Start("Main");
        }

        //Выводит информацию о том, почему ввод был не валиден
        static private string GetReasonForInvalidString(string raw)
        {
            if (raw == "Root")
            {
                return "Имя не должно быть Root";
            }else
            {
                return "Есть запрещенные символы. Используйте только буквы латинского и кириллического алфавита и цифры";
            }
        }

        static private void ExitFromProgram(MenuForm menu)
        {
            Controller.Exit();
        }

        static private void ExitToMainMenu(MenuForm menu)
        {
            Controller.ChangeForm("Main");
        }

        static private void LoadXML(MenuForm menu)
        {
            if (Storage.LoadStructureFromFile())
            {
                menu.Confirm = "Структура загружена";
            }
            else
            {
                menu.Warning = "Файл со структурой не найден";
            }
        }

        static private void SaveXML(MenuForm menu)
        {
            menu.Confirm = "Структура сохранена";
            Storage.SaveStructureToFile();
        }

        static private void OutputAllTags(MenuForm menu)
        {
            menu.Info = string.Format("Вывод:\n{0}", Storage.GetAllTagsAsString());
        }

        //Функции, которые используют формы для добавления тегов
        static private void ToAddTagForm(MenuForm menu)
        {
            Controller.ChangeForm("AddTagForm");
        }
        static private void AddTagFormSearchParent(MenuForm menu)
        {
            bufferItem = Storage.FindItemByName(menu.Input);
            if(menu.Input == "Root")
            {
                bufferItem = Storage.Root;
                Controller.ChangeForm("AddTagForm1InputChildName");
            }
            else if (bufferItem == null)
            {
                menu.Warning = "Тег с таким именем не найден";
            }else
            {
                Controller.ChangeForm("AddTagForm1InputChildName");
            }
        }
        static private void AddTagFormInputChildName(MenuForm menu)
        {
            string oldName = bufferItem.Name;
            if (Regex.IsMatch(menu.Input, "[^a-zA-Zа-яА-Я0-9]") || menu.Input == "Root")
            {
                menu.Warning = GetReasonForInvalidString(menu.Input);
            }
            else if (bufferItem.IsChildNameUnique(menu.Input))
            {
                bufferName = menu.Input;
                Controller.ChangeForm("AddTagForm2ChoseType");
            }
            else
            {
                menu.Warning = "Имя уже используется";
            }
        }
        static private void AddTagFormAddNone(MenuForm menu)
        {
            bufferItem.AddChild(bufferName, null);
            Controller.ChangeForm("Main");
            Controller.Form.Confirm = string.Format("Элемент '{0}' добавлен в структуру", bufferName);
        }
        static private void AddTagFormAddBool(MenuForm menu)
        {
            bufferItem.AddChild(bufferName, false);
            Controller.ChangeForm("Main");
            Controller.Form.Confirm = string.Format("Элемент '{0}' добавлен в структуру", bufferName);
        }
        static private void AddTagFormAddInt(MenuForm menu)
        {
            bufferItem.AddChild(bufferName, 0);
            Controller.ChangeForm("Main");
            Controller.Form.Confirm = string.Format("Элемент '{0}' добавлен в структуру", bufferName);
        }
        static private void AddTagFormAddDouble(MenuForm menu)
        {
            bufferItem.AddChild(bufferName, 0.0);
            Controller.ChangeForm("Main");
            Controller.Form.Confirm = string.Format("Элемент '{0}' добавлен в структуру", bufferName);
        }

        //Функции, которые используют формы для переименования тегов
        static private void ToRenameTagForm(MenuForm menu)
        {
            Controller.ChangeForm("RenameForm");
        }
        static private void RenameTagFormInputPath(MenuForm menu)
        {
            TagItem target = Storage.GetItemByFullPath(menu.Input);
            if (menu.Input == "Root")
            {
                menu.Warning = "Корневой элемент переименовывать нельзя";
            }
            else if (target != null)
            {
                bufferItem = target;
                Controller.ChangeForm("RenameForm1InputName");
            }
            else
            {
                menu.Warning = "Не смог найти тег по указаному пути";
            }
        }
        static private void RenameTagFormInputNameAndRenameTag(MenuForm menu)
        {
            string oldName = bufferItem.Name;
            if (Regex.IsMatch(menu.Input, "[^a-zA-Zа-яА-Я0-9]") || menu.Input == "Root")
            {
                menu.Warning = GetReasonForInvalidString(menu.Input);
            }
            else if(bufferItem.SetName(menu.Input))
            {
                Controller.ChangeForm("Main");
                Controller.Form.Confirm = string.Format("Вы переименовали элемент '{0}' на '{1}'", oldName, bufferItem.Name);
            }
            else
            {
                menu.Warning = "Имя уже используется";
            }
        }

        //Функции, которые используют формы для удаления тегов
        static private void ToRemoveTagForm(MenuForm menu)
        {
            Controller.ChangeForm("RemoveForm");
        }
        static private void RemoveFormSearchAndRemoveTag(MenuForm menu)
        {
            TagItem item = Storage.FindItemByName(menu.Input);
            if (menu.Input == "Root")
            {
                menu.Warning = "Корневой тег удалять нельзя";
            }
            else if (item == null)
            {
                menu.Warning = "Тег с таким именем не найден";
            }
            else
            {
                item.Parent.RemoveChildByName(item.Name);
                Controller.ChangeForm("Main");
                Controller.Form.Confirm = string.Format("Тег {0} успешно удален", menu.Input);
            }
        }
    }
}
