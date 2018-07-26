using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tikTestTask
{
    class TagItem
    {
        private object TagValue = null;
        private string TagName = null;
        private List<TagItem> TagChildrens = new List<TagItem>();
        private TagItem TagParent = null;

        public List<TagItem> Childrens
        {
            get
            {
                return TagChildrens;
            }
        }

        public object Value
        {
            get
            {
                if (Type == "None")
                {
                    return "None";
                }
                return TagValue;
            }
        }

        public string Name
        {
            get
            {
                return TagName;
            }
        }

        public TagItem Parent
        {
            get
            {
                return TagParent;
            }
        }

        public string Type
        {
            get
            {
                return IdentifyAndGetValueType(TagValue);
            }
        }

        //Выводит уровень вложености
        public int Level
        {
            get
            {
                int Level = 0;
                TagItem cursor = TagParent;
                //Увеличивать инкремент и присваивать элементу его родителя до тех пор, пока у элемента не окажется родителя
                while (cursor != null)
                {
                    Level++;
                    cursor = cursor.TagParent;
                }
                return Level;
            }
        }

        //Выводит полный путь
        public string FullPath
        {
            get
            {
                string path = TagName;
                TagItem cursor = TagParent;
                //Дабавлять имя элемента с точкой к результирующей строке и присваивать элементу его родителя до тех пор, пока у элемента не окажется родителя
                while (cursor != null)
                {
                    path = string.Format("{0}.{1}", cursor.TagName, path);
                    cursor = cursor.TagParent;
                }
                return path;
            }
        }

        public TagItem(string name, object value, TagItem parent)
        {
            TagParent = parent;
            if (!SetValue(value))
            {
                throw new Exception("Значение не является одним из доступных типов (double, int, bool, none)");
            }
            if (!SetName(name))
            {
                throw new Exception("Не уникальное имя среди дочерних объектов родителя");
            }
        }

        //Добавляет ребенка к тегу
        public TagItem AddChild(string name, object value)
        {
            TagItem newTag = new TagItem(name, value, this);
            TagChildrens.Add(newTag);
            return newTag;
        }
        //Находим и удаляем ребенка
        public bool RemoveChildByName(string name)
        {
            TagItem itemToRemove = GetChildByName(name);
            if(itemToRemove != null)
            {
                TagChildrens.Remove(itemToRemove);
                return true;
            }
            return false;
        }
        //Устанавливаем имя тегу, если он не корневой и имя уникально среди детей родителя этого элемента
        public bool SetName(string name)
        {
            if (TagParent == null || TagParent.IsChildNameUnique(name))
            {
                TagName = name;
                return true;
            }
            return false;
        }
        //Устанавливаем значение, если тип значения соответсвует одному из перечисленных
        public bool SetValue(object value)
        {
            if (IdentifyAndGetValueType(value) != "Unknown")
            {
                TagValue = value;
                return true;
            }
            return false;
        }
        //Отвечает на вопрос - Есть ли дети с таким именем?
        public bool IsChildNameUnique(string name)
        {
            foreach (TagItem item in TagChildrens)
            {
                if (item.Name == name)
                {
                    return false;
                }
            }
            return true;
        }
        //Находит ребенка по имени, иначе null
        public TagItem GetChildByName(string name)
        {
            foreach(TagItem item in TagChildrens)
            {
                if (item.Name == name)
                {
                    return item;
                }
            }
            return null;
        }
        //Идентифицирует тип и возвращает название типа
        static public string IdentifyAndGetValueType(object value)
        {
            
            if (value == null)
            {
                return "None";
            }
            if (value is int)
            {
                return "Int";
            }
            if (value is double)
            {
                return "Double";
            }
            if (value is bool)
            {
                return "Bool";
            }
            return "Unknown";
        }
    }
}
