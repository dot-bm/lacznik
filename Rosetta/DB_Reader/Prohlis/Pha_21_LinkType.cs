using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rosetta.CoreTools;
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
    internal class Pha_21_LinkTypes : Pha_GA_GenericAutomated, IProhlisArbeitnehmer
    {
        protected int _idJohQuery = 210;

        protected List<BlaBinResultRow> _ownobj;

        public Pha_21_LinkTypes() : base(21)
        {
        }

        protected override string _HEader_ConstString => "LINKTYPE:NO;NAME;RANK;TSYSSET";

        public override bool WorkInNetMode => true;

        protected override IEnumerable<Tuple<int, int?>> _OwnObj_IdentsOnly()
        {
            if (_ownobj == null) _ReadEntityData();
            ;
            return _ownobj.Select(n => new Tuple<int, int?>(n.OrdNum, n.RelObjID));
        }

        protected void _ReadEntityData()
        {
            _ownobj = _phs.EntityReader[_idJohQuery].Query(_connection, _phs.Variant).Select(y => _OnReadDataRow(y))
                .ToList();
        }

        protected BlaBinResultRow _OnReadDataRow(IDataRecord dataRecord)
        {
            //ID40T0, Nazwa, Rank, LX4002
            return new BlaBinResultRow
            {
                EntityID = EntityID,
                RelObjID = null,
                OrdNum = dataRecord.GetIntNumber(0),
                Content =
                    $"{dataRecord.GetIntNumber(0)};{(dataRecord.IsDBNull(1) ? "" : dataRecord.GetString(1))};{(dataRecord.IsDBNull(2) ? 9 : dataRecord.GetIntNumber(2))};"
            };
        }

        protected override IEnumerable<BlaBinResultRow> _OwnObj_AsString()
        {
            if (_ownobj == null) _ReadEntityData();
            ;
            return _ownobj;
        }
    }
}