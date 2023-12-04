using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slutuppgift.DATA
{
    internal class DataAccess
    {
        public enum BookTitles
        {
            [Description("Metro 2033")] Metro, [Description("Lord of the rings")] Lotr, [Description("Judge Dredd")] Dredd,
            [Description("Game Of Thrones")] GOT, [Description("Silent Hill")] SH, [Description("Batman Bin Suparman")] FMARVEL, Halo,
            [Description("The Picture of Dorian Gray")] DG, [Description("Never Let Me Go")] Never, [Description("The Road")] VÄG, [Description("March: Book One (Oversized Edition)")] Stor, [Description("The Hobbit")] Liten,
            [Description("Pride and Prejudice")] TroddeDettaVaEnFilm, [Description("A Tale of Two Cities")] SimCity, [Description("Crime and Punishment")] HörtDennaVaBra, [Description("We Should All Be Feminists")] IVissaFall, [Description("Persepolis")] NuräckerD,
        }














            private string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                return attribute.Description;
            }

            return value.ToString();
        }
    }
}

