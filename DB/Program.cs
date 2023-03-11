using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Filiere F1 = new Filiere("GINF", "genie informatique");
            Filiere F2 = new Filiere("GINF", "genie informatique");
            Filiere F3 = new Filiere("GINF", "genie informatique");
            Filiere F4 = new Filiere("GINF", "genie informatique");
            Filiere F5 = new Filiere("GINF", "genie informatique");

            F1.save();
            F2.save();
            F3.save();
            F4.save();
            F5.save();

            Filiere F = F1.find();
            F1 = Filiere.find<Filiere>(2);
            F2 = F;

            Console.WriteLine(F1 + "\n" + F2);

            
          

        }
    }
}
