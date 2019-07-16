using System;
using System.Collections.Generic;
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

namespace Rosetta.DB_Reader.Prohlis
{
    internal class Pha_32_Connectors : Pha_GB_Brewer, IProhlisArbeitnehmer
    {
        private readonly List<Tuple<int, int, bool, string>> __list = new List<Tuple<int, int, bool, string>>();

        public Pha_32_Connectors() : base(32)
        {
        }

        protected override string _HEader_ConstString => "$+CONNECTOR:ZONENO;NODENO;DIRECTION;TSYSSET";

        protected string _qoFieldList => "ZoneID, NodeID, Dir, Content";

        protected string _qoSourceFunName => "[mgm].[F7220_T32_Connectors_Core]";

        protected override void _ExpandParams(SqlParameterCollection paramc, BlaDBVariantDefinition pvd)
        {
            paramc.AddWithValue("@Rej_SliceID", pvd.iLX118R);
        }

        protected override string _QueryString()
        {
            return
                "select {0} from {1}(@K_ConstrSliceID, @S_ConstrSliceID, @K_ParamSliceID, @S_ParamSliceID, @Przebieg_SliceID, @Kurs_SliceID, @Rej_SliceID, @Year, @simplystop, @ArcResults)";
        }

        protected override void _AskAboutOwnData(BlaCommonStorage phs)
        {
            using (var r = _QueryOwnDef(phs.Variant, _qoFieldList, _qoSourceFunName))
            {
                _ParseRow(r);
            }
        }

        protected override IEnumerable<Tuple<int, int?>> _OwnObj_IdentsOnly()
        {
            return __list.Select(y => new Tuple<int, int?>(y.Item1, y.Item2));
        }


        protected override IEnumerable<BlaBinResultRow> _OwnObj_AsString()
        {
            return __list.Select(y => new BlaBinResultRow
                {OrdNum = y.Item1, RelObjID = y.Item2, EntityID = EntityID, Content = y.Item4});
        }

        protected void _ParseRow(SqlDataReader r)
        {
            while (r.Read())
            {
                var id = r.GetInt32(0);
                if (id == -1) continue;
                __list.Add(new Tuple<int, int, bool, string>(id, r.GetInt32(1), r.GetBoolean(2), r.GetString(3)));
            }
        }
    }
}