using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Rosetta.DB_Reader.Blasewitz_DataModel;
using Rosetta.Model.GrunaerWeg_Atts;
using Rosetta.Reporting;

//Łącznik – DB entries to Model Transfer File/Network converter
//(c) 2017 Bartłomiej Mikulski
//https://github.com/dot-bm/lacznik
//
//This file is part of Łącznik.
//
//Łącznik is free software: you can redistribute it and/or modify
//it under the terms of the GNU Lesser General Public License as published by
//the Free Software Foundation; either version 3 of the License, or
//(at your option) any later version.
//
//Łącznik is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU Lesser General Public License for more details.
//
//You should have received a copy of the GNU Lesser General Public License
//along with Łącznik.  If not, see <http://www.gnu.org/licenses/>.
//
//Ten plik jest częścią Łącznika.
//
//Łącznik jest wolnym oprogramowaniem: możesz go rozprowadzać dalej
//i/lub modyfikować na warunkach Mniejszej Powszechnej Licencji Publicznej GNU,
//wydanej przez Fundację Wolnego Oprogramowania - według wersji 3 tej
//Licencji lub (według twojego wyboru) którejś z późniejszych wersji.
//
//Łącznik rozpowszechniany jest z nadzieją, iż będzie on
//użyteczny - jednak BEZ JAKIEJKOLWIEK GWARANCJI, nawet domyślnej
//gwarancji PRZYDATNOŚCI HANDLOWEJ albo PRZYDATNOŚCI DO OKREŚLONYCH
//ZASTOSOWAŃ. W celu uzyskania bliższych informacji sięgnij do Mniejszej Powszechnej Licencji Publicznej GNU.
//
//Z pewnością wraz z Łącznikiem otrzymałeś też egzemplarz
//Mniejszej Powszechnej Licencji Publicznej GNU (GNU Lesser General Public License).
//Jeśli nie - zobacz <http://www.gnu.org/licenses/>. 

namespace Rosetta.DB_Reader.Gruna
{
    /// <summary>
    ///     Klasa służąca do odczytywania wartości parametrów dla podanej encji
    ///     Kolejność działań:
    ///     1. Odczyt listy atrybutów do wydania (odrzucanie nieużywanych atrybutów)
    ///     2. Odczytywanie atrybutów. W tym celu:
    ///     2.1. Ustalamy listę funkcji wydających
    ///     2.2. Zadajemy zapytanie funkcji wydającej
    ///     2.3. Odczytujemy na podstawie wskazanych pól (+ew. wartości domyślne)
    ///     2.4. Wydajemy enumerację
    /// </summary>
    public static class GruNauerReader
    {
        private const string SQLCOMMAND_ATT = @"SELECT y.[ID7120]
              ,y.[LX7110]
              ,y.[LX7125]
              ,y.[NazwaPola]
              ,y.[IDSubAtrybutu_NazwaPola]
              ,a.LX7101
	          ,a.LX7102
	          ,a.LX7103
	          ,a.GenTwórzZłożony
	          ,f.LX7121
	          ,f.[Schema]
	          ,f.NazwaFunkcji
	          ,f.ObjectID_NazwaPola
          FROM [mgm].[7120_Y_ŹródłaAtrybutów] y
          inner join [mgm].[7110_Y_VisumAtrybuty] a on y.LX7110 = a.ID7110
          inner join [mgm].[7125_Y_FunkcjeWydająceAtrybuty] f on y.LX7125 = f.ID7125
          where y.[LX7110] in ({0})";

        public static IEnumerable<GrWAtt_Value> Parse( /*byte EntityID,*/ DBConnectionPool connection,
            IEnumerable<GrWAtt_Definition> defs, BlaDBVariantDefinition var, RoReporter reporter = null)
        {
            var attids = defs.Select(a => a.AttID).Distinct().ToArray();
            if (attids.Length < 1) return Enumerable.Empty<GrWAtt_Value>();

            var zahada = string.Join(", ", attids);
            var cmd = new SqlCommand(string.Format(SQLCOMMAND_ATT, zahada), connection.Connection);
            var re = cmd.ExecuteReader();
            reporter?.AddInfo("Odczyt definicji funkcji wydających atrybuty");
            var rer = re.Cast<IDataRecord>();
            var fields = rer.Select(r => new
            {
                id = r["LX7110"], proc = string.Format("[{0}].[{1}]", r["Schema"], r["NazwaFunkcji"]),
                procid = (int) r["LX7125"], pargtempl = (int) r["LX7121"], objectid = (string) r["ObjectID_NazwaPola"],
                attsrc = new GrWAtt_VSource(r)
            }).ToList();
            rer = null;
            re = null;
            var fieldsByProcs = fields.GroupBy(q => q.procid).Select(q =>
            {
                var pro = q.First();
                reporter?.AddInfo($"Odczyt funkcji {pro.proc} ({pro.procid})");
                return new GruFuncOrStoredProcReader(connection, pro.procid, objIDField: pro.objectid,
                    pargtempl: pro.pargtempl, procName: pro.proc, attsrcs: q.Select(w => w.attsrc));
            });
            return fieldsByProcs.SelectMany(t => t.Ask(var));
            //return Enumerable.Empty<GrWAtt_Value>();
        }
    }
}