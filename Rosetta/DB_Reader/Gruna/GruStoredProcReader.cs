using System.Collections.Generic;
using System.Linq;
using Rosetta.CoreTools;
using Rosetta.DB_Reader.Blasewitz_DataModel;
using Rosetta.Model.GrunaerWeg_Atts;

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
    /// </summary>
    public class GruFuncOrStoredProcReader
    {
        private readonly List<GrWAtt_VSource> __attsources;

        private readonly DBConnectionPool __bind;
        private readonly string __commandText;
        private readonly string __objIDField;
        private readonly int __procArgTmpl;

        /// <summary>
        ///     Konstruktor
        /// </summary>
        /// <param name="bind">Połączenie do bazy danych</param>
        /// <param name="procID">ID funkcji/procedury z tabeli [7125]</param>
        /// <param name="procName">Nazwa funkcji (ze schemą)</param>
        /// <param name="objIDField">Nazwa pola zawierająca dane ObjectID</param>
        /// <param name="attsrcs">Wyliczenie źródeł atrybutów</param>
        /// <param name="pargtempl">ID szablonu argumentów funkcji</param>
        public GruFuncOrStoredProcReader(DBConnectionPool bind, int procID, string procName, string objIDField,
            int pargtempl, IEnumerable<GrWAtt_VSource> attsrcs)
        {
            __bind = bind;
            __attsources = attsrcs.ToList();
            __commandText =
                $"select {string.Join(", ", __attsources.SelectMany(w => w.Fields().Distinct().OrderBy(r => r)))}, {objIDField} from {procName}({_.I.GrunaTemplater.BracketContent(pargtempl)})";
            __procArgTmpl = pargtempl;
            __objIDField = objIDField;
        }

        /// <summary>
        ///     Odpytuje funkcję wydając wyniki w postaci wyliczenia GrWAtt_Value
        /// </summary>
        /// <returns></returns>
        public IEnumerable<GrWAtt_Value> Ask(BlaDBVariantDefinition var)
        {
            var objIDindex = -1;
            foreach (var row in __bind.ExecuteQuery(__commandText, _.I.GrunaTemplater.PrepareCmd(__procArgTmpl, var)))
            {
                if (objIDindex < 0) objIDindex = DBConnectionPool.FieldIndices(new[] {__objIDField}, row).First();
                var objID = row.GetIntNumber(objIDindex);
                foreach (var att in __attsources)
                {
                    var a = att.GetValue(objID, row);
                    if (a != null) yield return a;
                }
            }

            //yield break;
        }
    }
}