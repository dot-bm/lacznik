﻿using System;
using System.Collections.Generic;
using System.Globalization;
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
    internal class Pha_40_StopPoints : Pha_GA_GenericAutomated
    {
        public Pha_40_StopPoints() : base(40)
        {
        }

        public override bool WorkInNetMode => true;

        protected override string _HEader_ConstString =>
            "STOPPOINT:NO;STOPAREANO;CODE;NAME;TYPENO;TSYSSET;DIRECTED;NODENO;LINKNO;FROMNODENO;RELPOS";

        protected override IEnumerable<Tuple<int, int?>> _OwnObj_IdentsOnly()
        {
            return _phs.PuT.Stops.Items.Select(psi => new Tuple<int, int?>(psi.Id, null));
        }

        protected override IEnumerable<BlaBinResultRow> _OwnObj_AsString()
        {
            return _phs.PuT.Stops.Items.Select(x => new BlaBinResultRow
            {
                EntityID = EntityID, OrdNum = x.Id, RelObjID = null,
                Content = string.Format(CultureInfo.InvariantCulture, "{0};{0};{1};{1};{2};{3};0;{0};;;0.00", x.Id,
                    x.Name, x.TypeNo, x.HaltOfTSys)
            });
        }
    }
}