using CMS.Model.Files;
using System.Collections.Generic;
using System.Text.Json;

namespace CMS.Model
{
    public class Option
    {
        public Guid Id { get; set; }
        public OptionType Type { get; set; }
        public string Text { get; set; }

        private static List<Option> listInstance = new List<Option>();

        public Option() { }
        public static Option Instance(Option model)
        {
            string jsonString = JsonSerializer.Serialize(model);

            var item = JsonSerializer.Deserialize<Option>(jsonString);
            if (item == null)
                return new Option();
            return item;
        }

        public static void SetInstance(List<Option> model)=> listInstance = model;
        public static List<Option> GetItem()
        {
            List<Option> list = new List<Option>();
            foreach (var item in listInstance)
                list.Add(Instance(item));
            return list;
        }

        public static Option GetItem(OptionType model)
        {
            var option = listInstance.FirstOrDefault(x => x.Type == model);
            if (option != null)
                return Instance(option);
            return new Option() { Id = Guid.NewGuid(), Type = model, Text = "" };
        }
        public static List<Option> GetItem(List<OptionType> model)
        {
            List<Option> list = new List<Option>();
            foreach(var type in model)
            {
                var option = listInstance.FirstOrDefault(x => x.Type == type);
                if (option != null)
                    list.Add(Instance(option));
                else
                    list.Add(new Option() { Id = Guid.NewGuid(), Type = type, Text = "" });
            }
            return list;
        }
    }
}
