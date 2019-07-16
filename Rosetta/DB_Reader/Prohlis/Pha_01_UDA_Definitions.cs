using System.Collections.Generic;
using System.Linq;
using Rosetta.DB_Reader.Blasewitz_DataModel;
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

namespace Rosetta.DB_Reader.Prohlis
{
    /// <summary>
    ///     Zdobywa informacje o potrzebnych atrybutach. Przygotowuje tabelę UDA
    /// </summary>
    internal class Pha_01_UDA_Definitions : Pha_GG_Generic, IProhlisArbeitnehmer
    {
        private const string __HEADER =
            "USERATTDEF:OBJID;ATTID;CODE;NAME;DATA_TYPE;MINVALUE;MAXVALUE;DEFAULTVALUE;STRINGDEFAULTVALUE;COMMENT;MAXSTRLEN;NUMDECPLACES;FORMULA;SCALED;CROSSSECTIONLOGIC;CSLIGNORECLOSED";

        public byte EntityID => 1;

        //public override bool Prepare()
        //{
        //    return base.Prepare();
        //    //var cmd = new SqlCommand(connection: _connection.Connection, cmdText: "select [ID7110], [LX7101], [Pom], [Name], [U], [Użytkownika], [LX7102], [NumDecPlaces], [StringDefaultValue] from mgm.V7110_VisumAttributes");
        //    //try
        //    //{

        //    //    int t = 0;
        //    //    var reader = cmd.ExecuteReader();
        //    //    int nmUser = reader.GetOrdinal("Użytkownika");
        //    //    while (reader.Read()) {
        //    //        if (reader.GetInt32(0) > -1)
        //    //        {
        //    //            _phs.Attributies.Definitions.Add(new GrWAtt_Definition(reader));
        //    //            //{ EntityID = 1, AttID = reader.GetInt32(0) };
        //    //        }
        //    //        if ((reader.GetInt32(nmUser) == 1) || (reader.GetInt32(0) == -1)) _store.Add(new PhaResultRow() { Content = reader.GetString(4), EntityID = 1, OrdNum = t++ });
        //    //    }
        //    //}
        //    //catch
        //    //{
        //    //    return false;
        //    //}
        //    //return true;
        //}

        public override bool CheckPrerequisities(RoReporter reporter)
        {
            return _phs.Attributies.Definitions != null;
        }

        public override IEnumerable<BlaResultRow> PourResults()
        {
            if ((_phs.Attributies.Definitions?.Count ?? 0) == 0) return _ReturnEmpty();

            return _HeaderEnum(__HEADER, EntityID,
                    _WorkMode() == Blasewitz.Bla_WorkMode.TraFile
                        ? Blasewitz.Bla_RowMode.Add
                        : Blasewitz.Bla_RowMode.Null)
                .Concat(
                    _phs.Attributies.Definitions.UserDefined.Select((y, ix) => new BlaResultRow
                    {
                        Content = Blasewitz.ToString(_WorkMode() == Blasewitz.Bla_WorkMode.TraFile
                                      ? Blasewitz.Bla_RowMode.Add
                                      : Blasewitz.Bla_RowMode.Null) + y.VisumDefinition,
                        EntityID = EntityID, OrdNum = ix
                    }));
        }

        public override bool WorkInNetMode => true;
    }
}