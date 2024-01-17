using System.Text.Json;

namespace CMS.Model
{
    public class Option
    {
        public Guid Id { get; set; }
        public OptionType Type { get; set; }
        public string Text { get; set; }


        public Option() { }
        public static Option Instance(Option model)
        {
            string jsonString = JsonSerializer.Serialize(model);

            var item = JsonSerializer.Deserialize<Option>(jsonString);
            if (item == null)
                return new Option();
            return item;
        }

        private static List<Option> listInstance = new List<Option>();
        public static void SetInstance(List<Option> model)=> listInstance = model;
        public static List<Option> GetItems()
        {
            List<Option> list = new List<Option>();
            foreach (var item in listInstance)
                list.Add(Instance(item));
            return list;
        }

        public static List<Option> GetItems(List<OptionType> model)
        {
            List<Option> list = new List<Option>();
            foreach (var item in listInstance)
            {
                var option = model.FirstOrDefault(x => x == item.Type);
                if(option != OptionType.Unknown)
                    list.Add(Instance(item));
            }
            return list;
        }
    }
}
