using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Rosetta.DB_Reader.Blasewitz_DataModel;

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
    ///     Malutka klasa do rozwiązywania problemów z szablonami argumentów procedur/funkcji
    /// </summary>
    public class GruArgTemplateReader
    {
        private readonly List<Tuple<int, byte[]>> __tmpllist;

        public GruArgTemplateReader()
        {
            var cmd = new SqlCommand(
                "SELECT [LX7121], [Ord], [LX7105]  FROM[mgm].[7122_Y_SzablonyParametrów_Zawartość]",
                _.SetupConnection().Connection);
            var r = cmd.ExecuteReader();
            __tmpllist = r.Cast<IDataRecord>()
                .Select(q => new {LX7121 = q.GetInt32(0), Ord = q.GetByte(1), LX7105 = q.GetByte(2)})
                .GroupBy(q => q.LX7121).Select(q =>
                    new Tuple<int, byte[]>(q.Key, q.OrderBy(y => y.Ord).Select(y => y.LX7105).ToArray())).ToList();
            //specjalne ustawienie dla braku parametrów
            __tmpllist.Add(new Tuple<int, byte[]>(0, new byte[] { }));
        }

        public string BracketContent(int argTId)
        {
            return string.Join(",", __tmpllist.First(y => y.Item1 == argTId).Item2.Select(t => "@p" + t.ToString()));
        }

        public void PrepareCmd(int argTId, SqlCommand cmd, BlaDBVariantDefinition var)
        {
            cmd.Parameters.AddRange(__tmpllist.First(a => a.Item1 == argTId).Item2
                .Select(g => new SqlParameter("@p" + g.ToString(), var.Get4Params(g))).ToArray());
        }

        public IEnumerable<SqlParameter> PrepareCmd(int argTId, BlaDBVariantDefinition var)
        {
            return __tmpllist.First(a => a.Item1 == argTId).Item2
                .Select(g => new SqlParameter("@p" + g.ToString(), var.Get4Params(g)));
        }
    }
}