using System;
using System.Data.SqlClient;
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
    //30, GQB
    internal abstract class Pha_GB_Brewer : Pha_GA_GenericAutomated
    {
        //protected DataTable _arcre;
        //protected DataTable _tt4052;

        //protected GrWAtt_Definitions _attribs;

        //2, 30, GQB
        public Pha_GB_Brewer(byte entityID) : base(entityID)
        {
        }


        protected abstract void _AskAboutOwnData(BlaCommonStorage phs);

        protected virtual void _ExpandParams(SqlParameterCollection paramc, BlaDBVariantDefinition pvd)
        {
        }

        protected virtual SqlDataReader _QueryOwnDef(BlaDBVariantDefinition pvd, string fieldlist, string funcname)
        {
            var com = new SqlCommand(string.Format(_QueryString(), fieldlist, funcname), _connection.Connection);
            com.CommandTimeout = 0;
            pvd.PrepareSqlCommandParams(com, EntityID);
            _ExpandParams(com.Parameters, pvd);
            return com.ExecuteReader();
        }

        protected virtual string _QueryString()
        {
            return
                "select {0} from {1}(@K_ConstrSliceID, @S_ConstrSliceID, @K_ParamSliceID, @S_ParamSliceID, @Przebieg_SliceID, @Kurs_SliceID, @Year, @simplystop, @ArcResults)";
        }

        public override bool Prepare()
        {
            try
            {
                _AskAboutOwnData(_phs);
            }
            catch (Exception e)
            {
                _phs.Reporter.AddException(e, "Pha " + EntityID);
                return false;
            }

            return true;
        }
    }


    //abstract class Pha_GBU_BrewerUnaryIdentifier : Pha_GB_Brewer, IProhlisArbeitnehmer
    //{

    //    public Pha_GBU_BrewerUnaryIdentifier(byte entityID) : base(entityID) {

    //    }


    //    public override IEnumerable<PhaResultRow> PourResults()
    //    {
    //        PhaAttribDefinitions myatts = _GatherMyAttribDefs();
    //        var sta = _OO_IdentsOnly().GroupJoin(_values, o => o, v => v.ObjectID, (o, tail) => new { ObjectID = o, tail });
    //        var sta2 = sta.OrderBy(y => y.ObjectID).Select(y => myatts.ProduceOutput(y.tail));
    //        return (_OwnValues().OrderBy(a => a.OrdNum).Zip(sta2, (c, t) => new { c, t }).Select((a, i) => new PhaResultRow() { EntityID = EntityID, OrdNum = i, Content = a.c.Content + ((myatts.Count > 0) ? (((!String.IsNullOrWhiteSpace(a.c.Content)) ? ";" : "") + a.t) : "") })).Concat(Enumerable.Repeat(new PhaResultRow() { EntityID = EntityID, OrdNum = -1, Content = _HEader_PushComplete(myatts) }, 1));
    //    }
}