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
    internal class Pha_30_Zones : Pha_GB_Brewer, IProhlisArbeitnehmer
    {
        protected Dictionary<int, string> _list;

        public Pha_30_Zones() : base(30)
        {
            _list = new Dictionary<int, string>();
        }

        protected override string _HEader_ConstString => "$+ZONE:NO;CODE;NAME;XCOORD;YCOORD";

        protected string _qoFieldList => "ID1100, Definit, Shape";

        protected string _qoSourceFunName => "[mgm].[F7220_T30_Zones_Core]";

        protected override SqlDataReader _QueryOwnDef(BlaDBVariantDefinition pvd, string fieldlist, string funcname)
        {
            var com = new SqlCommand(
                string.Format("select {0} from {1}(@R_SliceID, @Date, @Tolerance)", fieldlist, funcname),
                _connection.Connection);
            com.Parameters.AddWithValue("@R_SliceID", pvd.iLX118R);
            com.Parameters.AddWithValue("@Date", pvd.Date);
            com.CommandTimeout = 0;
            com.Parameters.AddWithValue("@Tolerance", 30);
            return com.ExecuteReader();
        }

        protected override void _AskAboutOwnData(BlaCommonStorage phs)
        {
            using (var r = _QueryOwnDef(phs.Variant, _qoFieldList, _qoSourceFunName))
            {
                var i = 0;
                while (r.Read())
                {
                    var id = r.GetInt32(0);
                    if (id == -1)
                    {
                        phs.ExtResults.Add(new BlaResultRow
                            {EntityID = 30, OrdNum = -1, Content = "$*ZONE:NO;WKTSURFACE"});
                        continue;
                    }

                    //Dodane w celu rozdzielenia zapisywania kształtów rejonów w jednym pliku
                    phs.ExtResults.Add(new BlaResultRow
                        {EntityID = 30, OrdNum = i++, Content = string.Format("{0};{1}", id, r.GetString(2))});
                    _list.Add(id, r.GetString(1));
                }
            }
        }

        protected override IEnumerable<Tuple<int, int?>> _OwnObj_IdentsOnly()
        {
            return _list.Select(a => new Tuple<int, int?>(a.Key, null));
        }

        protected override IEnumerable<BlaBinResultRow> _OwnObj_AsString()
        {
            return _list.Select(a => new BlaBinResultRow
                {EntityID = EntityID, OrdNum = a.Key, RelObjID = null, Content = a.Value});
        }
    }
}