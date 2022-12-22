using System;
using System.Collections.ObjectModel;

namespace Timezone
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime a = new DateTime(DateTime.UtcNow.Ticks, DateTimeKind.Unspecified);
            ToVnTime(a);

            Console.ReadKey();
        }

        static void GetTimeZones()
        {
            Console.WriteLine("Hello World!");

            ReadOnlyCollection<TimeZoneInfo> tzCollection = TimeZoneInfo.GetSystemTimeZones();

            foreach (TimeZoneInfo timeZone in tzCollection)
                Console.WriteLine("   {0}: {1}", timeZone.Id, timeZone.DisplayName);
        }

        static void ToVnTime(DateTime d)
        {
            d = DateTime.SpecifyKind(d, DateTimeKind.Utc);
            Console.WriteLine("{0} {1}.", d, d.Kind);

            TimeZoneInfo gmt7Zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            TimeZoneInfo gmtZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");


            if (d.Kind == DateTimeKind.Utc || d.Kind == DateTimeKind.Unspecified)
            {
                //vnt
                var a = TimeZoneInfo.ConvertTime(d, gmt7Zone);
                Console.WriteLine("{0} {1}.", a, a.Kind);

                //vnt
                var b = a.AddDays(-1).Date.AddHours(8);
                Console.WriteLine("{0} {1}.", b, b.Kind);

                //utc
                var c = TimeZoneInfo.ConvertTimeToUtc(b, gmt7Zone);
                Console.WriteLine("{0} {1}.", c, c.Kind);
            }
        }
    }
}
