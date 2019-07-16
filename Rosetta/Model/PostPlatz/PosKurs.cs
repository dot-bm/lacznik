using System;
using System.Data;
using System.Data.SqlClient;
using Rosetta.CoreTools;
using Rosetta.KarlsruherStr;
using Rosetta.Model.Oschatzer_Str;

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

namespace Rosetta.Model.PostPlatz
{
    public class PosKurs : IKarlsruheReady
    {
        private readonly PosFahrPlan __plan;
        private IDataRecord r;

        public PosKurs(PosFahrPlan plan, SqlDataReader r, bool parseOperator = false)
        {
            __plan = plan;
            ObjectId = r.GetIntNumber(0);
            Name = r.IsDBNull(2) ? "" : r.GetString(2);
            AbfahrtZeit = r.GetTimeSpan(3);

            __plan.KursHinzufügen(this);
        }

        //No, Dep, LineName, LineRouteName, DirectionCode, TimeProfileName, FromTProfItemIndex, ToTProfItemIndex

        public string Name { get; }
        public TimeSpan AbfahrtZeit { get; }

        public string Core()
        {
            return
                $"{ObjectId};{AbfahrtZeit};{__plan.Strecke.Linie.Name};{__plan.Strecke.Name};{__plan.Strecke.Richtung.ToStr()};{__plan.Name};1;{__plan.HalteStelleAnzahl};{Name}";
        }

        public int ObjectId { get; }
    }
}