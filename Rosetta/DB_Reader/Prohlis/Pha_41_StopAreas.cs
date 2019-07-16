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
    internal class Pha_41_StopAreas : Pha_GA_GenericAutomated
    {
        public Pha_41_StopAreas() : base(41)
        {
        }

        public override bool WorkInNetMode => true;

        protected override string _HEader_ConstString => "STOPAREA:NO;STOPNO;CODE;NAME;NODENO;TYPENO;XCOORD;YCOORD";

        protected override IEnumerable<Tuple<int, int?>> _OwnObj_IdentsOnly()
        {
            return _phs.PuT.Stops.Items.Select(psi => new Tuple<int, int?>(psi.Id, null));
        }

        protected override IEnumerable<BlaBinResultRow> _OwnObj_AsString()
        {
            return _phs.PuT.Stops.Items.Select(psi => new BlaBinResultRow
            {
                EntityID = 41, OrdNum = psi.Id, RelObjID = null,
                Content =
                    $"{psi.Id};{psi.Id};{psi.Name};{psi.Name};{psi.AtNode.ObjectId};{psi.TypeNo};{psi.X.ToString("F2", CultureInfo.InvariantCulture)};{psi.Y.ToString("F2", CultureInfo.InvariantCulture)}"
            });
        }
    }
}