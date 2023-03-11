using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    internal class Filiere : Model
    {
        string code;
        string designation;
        public Filiere(string code, string designation) 
        {
            id = 0;
            this.code = code;
            this.designation = designation;
        }

        public override string ToString()
        {
            return "ID: " + id + " Code: " + code + " Designation: " + designation;
        }
    }
}
