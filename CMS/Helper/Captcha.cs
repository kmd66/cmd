using CMS.Model;
using System.Drawing;

namespace CMS.Helper
{
    public static class CaptchaHelper
    {
        private static SolidBrush _bacbrush(int random)
        {
            switch (random)
            {
                case 0: return new SolidBrush(Color.FromArgb(113, 50, 50));
                case 1: return new SolidBrush(Color.FromArgb(19, 120, 56));
                case 2: return new SolidBrush(Color.FromArgb(3, 67, 107));
            }
            return new SolidBrush(Color.FromArgb(0, 0, 0, 91));
        }

        public static Captcha GenerateInPersian(Bitmap backGroundImage = null, int charLength = 4, string font = "Tahoma", bool curveX = false, bool curveY = false, int minDrawLine = 10, int maxDrawLine = 20)
        {
            Bitmap bitmap = new Bitmap(250, 42);
            if (backGroundImage != null)
                bitmap = backGroundImage;

            var chars = "123456789";
            var stringChars = new char[charLength];
            var random5 = new Random();
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random5.Next(chars.Length)];
            }

            var graphics = Graphics.FromImage(bitmap);
            var number = new String(stringChars);
            var characters = int.Parse(number).NumberToText(Language.Persian);
            var brush_bac = new SolidBrush(Color.White);
            graphics.FillRectangle(brush_bac, new Rectangle(0, 0, bitmap.Width, bitmap.Height));

            Font drawFont = new Font(font, 16/*, FontStyle.Bold*/);
            var random = new Random();

            var bacbrush = _bacbrush(random.Next(0, 3));
            graphics.DrawString(characters, drawFont, bacbrush, 25, 7);

            var bp = new Bitmap(bitmap.Width, bitmap.Height);
            if (curveX)
            {
                double k = random.Next(500, 1000);
                int xp = 5 * random.Next(1, 3);
                for (int i = 0; i < bitmap.Width; i++)
                {
                    double bd = Math.Sin(k + i / xp) * random.Next(3, 7);
                    int it = (int)bd;
                    var r = new Rectangle(i, 0, 1, bitmap.Height);
                    bp = bitmap.Clone(r, bitmap.PixelFormat);
                    graphics.DrawImage(bp, i - 1, it, 1, bitmap.Height);
                }
            }

            if (curveY)
            {
                double dyp = random.Next(5, 10);
                double yp = 0;
                int yi = dyp % 2 == 0 ? 1 : -1;
                for (int i = 0; i < bitmap.Height; i++)
                {
                    yp += (dyp / 10) * yi;

                    var r = new Rectangle(0, i, bitmap.Width, 1);
                    bp = bitmap.Clone(r, bitmap.PixelFormat);
                    graphics.DrawImage(bp, (int)yp - (60 * yi), i, bitmap.Width, 1);
                }
            }

            if (minDrawLine > 0)
            {
                Random rnd = new Random();
                var n = random.Next(minDrawLine, maxDrawLine);
                for (int i = 0; i < n; i++)
                {
                    // calculate line start and end point here using the Random class:
                    int x0 = rnd.Next(0, bitmap.Width);
                    int y0 = rnd.Next(0, bitmap.Height);
                    int x1 = rnd.Next(0, bitmap.Width);
                    int y1 = rnd.Next(0, bitmap.Height);
                    graphics.DrawLine(new Pen(_bacbrush(random.Next(0, 3)).Color, random.Next(1, 3)), x0, y0, x1, y1);
                }
            }

            return new Captcha
            {
                Code = number.ToMd5(),
                Base64Image = BitmapToBase64(bitmap)
            };
        }
        public static string BitmapToBase64(Bitmap bi)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            bi.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] byteImage = ms.ToArray();
            var SigBase64 = Convert.ToBase64String(byteImage);

            return SigBase64;
        }
        public static bool Validate(Captcha c)
        {
            if (c.Code == c.Text.ToMd5())
                return true;
            return false;
        }
    }
    public class Captcha
    {
        public string Text { get; set; }

        public string Code { get; set; }

        public string Base64Image { get; set; }

        public DateTime ExpireDate { get; set; }
    }

    public enum Language
    {
        /// <summary>
        /// English Language
        /// </summary>
        English,

        /// <summary>
        /// Persian Language
        /// </summary>
        Persian
    }

    public enum DigitGroup
    {
        /// <summary>
        /// Ones group
        /// </summary>
        Ones,

        /// <summary>
        /// Teens group
        /// </summary>
        Teens,

        /// <summary>
        /// Tens group
        /// </summary>
        Tens,

        /// <summary>
        /// Hundreds group
        /// </summary>
        Hundreds,

        /// <summary>
        /// Thousands group
        /// </summary>
        Thousands
    }

    public class NumberToText
    {
        /// <summary>
        /// Digit's group
        /// </summary>
        public DigitGroup Group { set; get; }

        /// <summary>
        /// Number to word language
        /// </summary>
        public Language Language { set; get; }

        /// <summary>
        /// Equivalent names
        /// </summary>
        public IList<string> Names { set; get; }
    }

    public static class HumanReadableInteger
    {
        #region Fields (4)

        private static readonly IDictionary<Language, string> And = new Dictionary<Language, string>
  {
   { Language.English, " " },
   { Language.Persian, " و " }
  };
        private static readonly IList<NumberToText> NumberWords = new List<NumberToText>
  {
   new NumberToText { Group= DigitGroup.Ones, Language= Language.English, Names=
    new List<string> { string.Empty, "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" }},
   new NumberToText { Group= DigitGroup.Ones, Language= Language.Persian, Names=
    new List<string> { string.Empty, "یک", "دو", "سه", "چهار", "پنج", "شش", "هفت", "هشت", "نه" }},

   new NumberToText { Group= DigitGroup.Teens, Language= Language.English, Names=
    new List<string> { "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" }},
   new NumberToText { Group= DigitGroup.Teens, Language= Language.Persian, Names=
    new List<string> { "ده", "یازده", "دوازده", "سیزده", "چهارده", "پانزده", "شانزده", "هفده", "هجده", "نوزده" }},

   new NumberToText { Group= DigitGroup.Tens, Language= Language.English, Names=
    new List<string> { "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" }},
   new NumberToText { Group= DigitGroup.Tens, Language= Language.Persian, Names=
    new List<string> { "بیست", "سی", "چهل", "پنجاه", "شصت", "هفتاد", "هشتاد", "نود" }},

   new NumberToText { Group= DigitGroup.Hundreds, Language= Language.English, Names=
    new List<string> {string.Empty, "One Hundred", "Two Hundred", "Three Hundred", "Four Hundred",
     "Five Hundred", "Six Hundred", "Seven Hundred", "Eight Hundred", "Nine Hundred" }},
   new NumberToText { Group= DigitGroup.Hundreds, Language= Language.Persian, Names=
    new List<string> {string.Empty, "یکصد", "دویست", "سیصد", "چهارصد", "پانصد", "ششصد", "هفتصد", "هشتصد" , "نهصد" }},

   new NumberToText { Group= DigitGroup.Thousands, Language= Language.English, Names=
     new List<string> { string.Empty, " Thousand", " Million", " Billion"," Trillion", " Quadrillion", " Quintillion", " Sextillian",
   " Septillion", " Octillion", " Nonillion", " Decillion", " Undecillion", " Duodecillion", " Tredecillion",
   " Quattuordecillion", " Quindecillion", " Sexdecillion", " Septendecillion", " Octodecillion", " Novemdecillion",
   " Vigintillion", " Unvigintillion", " Duovigintillion", " 10^72", " 10^75", " 10^78", " 10^81", " 10^84", " 10^87",
   " Vigintinonillion", " 10^93", " 10^96", " Duotrigintillion", " Trestrigintillion" }},
   new NumberToText { Group= DigitGroup.Thousands, Language= Language.Persian, Names=
     new List<string> { string.Empty, " هزار", " میلیون", " میلیارد"," هزار میلیارد"," هزار", " کوآدریلیون", " کادریلیون", " Sextillian",
   " Septillion", " Octillion", " Nonillion", " Decillion", " Undecillion", " Duodecillion", " Tredecillion",
   " Quattuordecillion", " Quindecillion", " Sexdecillion", " Septendecillion", " Octodecillion", " Novemdecillion",
   " Vigintillion", " Unvigintillion", " Duovigintillion", " 10^72", " 10^75", " 10^78", " 10^81", " 10^84", " 10^87",
   " Vigintinonillion", " 10^93", " 10^96", " Duotrigintillion", " Trestrigintillion" }},
  };
        private static readonly IDictionary<Language, string> Negative = new Dictionary<Language, string>
  {
   { Language.English, "Negative " },
   { Language.Persian, "منهای " }
  };
        private static readonly IDictionary<Language, string> Zero = new Dictionary<Language, string>
  {
   { Language.English, "Zero" },
   { Language.Persian, "صفر" }
  };

        #endregion Fields

        #region Methods (7)

        // Public Methods (5)

        /// <summary>
        /// display a numeric value using the equivalent text
        /// </summary>
        /// <param name="number">input number</param>
        /// <param name="language">local language</param>
        /// <returns>the equivalent text</returns>
        public static string NumberToText(this int number, Language language)
        {
            return NumberToText((long)number, language);
        }


        /// <summary>
        /// display a numeric value using the equivalent text
        /// </summary>
        /// <param name="number">input number</param>
        /// <param name="language">local language</param>
        /// <returns>the equivalent text</returns>
        public static string NumberToText(this uint number, Language language)
        {
            return NumberToText((long)number, language);
        }

        /// <summary>
        /// display a numeric value using the equivalent text
        /// </summary>
        /// <param name="number">input number</param>
        /// <param name="language">local language</param>
        /// <returns>the equivalent text</returns>
        public static string NumberToText(this byte number, Language language)
        {
            return NumberToText((long)number, language);
        }

        /// <summary>
        /// display a numeric value using the equivalent text
        /// </summary>
        /// <param name="number">input number</param>
        /// <param name="language">local language</param>
        /// <returns>the equivalent text</returns>
        public static string NumberToText(this decimal number, Language language)
        {
            return NumberToText((long)number, language);
        }

        /// <summary>
        /// display a numeric value using the equivalent text
        /// </summary>
        /// <param name="number">input number</param>
        /// <param name="language">local language</param>
        /// <returns>the equivalent text</returns>
        public static string NumberToText(this double number, Language language)
        {
            return NumberToText((long)number, language);
        }

        /// <summary>
        /// display a numeric value using the equivalent text
        /// </summary>
        /// <param name="number">input number</param>
        /// <param name="language">local language</param>
        /// <returns>the equivalent text</returns>
        public static string NumberToText(this long number, Language language)
        {
            if (number == 0)
            {
                return Zero[language];
            }

            if (number < 0)
            {
                return Negative[language] + NumberToText(-number, language);
            }

            return wordify(number, language, string.Empty, 0);
        }
        // Private Methods (2)

        private static string getName(int idx, Language language, DigitGroup group)
        {
            return NumberWords.Where(x => x.Group == group && x.Language == language).First().Names[idx];
        }

        private static string wordify(long number, Language language, string leftDigitsText, int thousands)
        {
            if (number == 0)
            {
                return leftDigitsText;
            }

            var wordValue = leftDigitsText;
            if (wordValue.Length > 0)
            {
                wordValue += And[language];
            }

            if (number < 10)
            {
                wordValue += getName((int)number, language, DigitGroup.Ones);
            }
            else if (number < 20)
            {
                wordValue += getName((int)(number - 10), language, DigitGroup.Teens);
            }
            else if (number < 100)
            {
                wordValue += wordify(number % 10, language, getName((int)(number / 10 - 2), language, DigitGroup.Tens), 0);
            }
            else if (number < 1000)
            {
                wordValue += wordify(number % 100, language, getName((int)(number / 100), language, DigitGroup.Hundreds), 0);
            }
            else
            {
                wordValue += wordify(number % 1000, language, wordify(number / 1000, language, string.Empty, thousands + 1), 0);
            }

            if (number % 1000 == 0) return wordValue;
            var getNames = getName(thousands, language, DigitGroup.Thousands);
            if (getNames.Trim() == "میلیارد")
            {
                if (wordValue.Contains(" هزار میلیارد"))
                {
                    wordValue = wordValue.Replace("میلیارد", "");
                }
            }
            return wordValue + getName(thousands, language, DigitGroup.Thousands);
        }

        #endregion Methods
    }
}
