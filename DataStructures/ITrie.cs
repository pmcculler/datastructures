using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures
{
    public interface ITrie
    {
        bool Contains(string s);

        void Add(string word);

        List<string> GetAllEntries();
    }
}
