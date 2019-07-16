using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rosetta.DB_Reader.Prohlis;

namespace Rosetta.DB_Reader.Gruna
{
    struct GruATemResult
    {
        string Paramstring;
        int[] ParamValues;
    }

    static class GruArgTemplateHelper
    {
        static GruATemResult Get(SqlConnection conn, int idTemplate, PhaVarDefinition vardef)
        {
            throw new NotImplementedException();
        }
    }
}
