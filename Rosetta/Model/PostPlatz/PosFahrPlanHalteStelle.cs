using System;
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
    public class PosFahrPlanHalteStelle : IKarlsruheReady
    {
        private const int __ERSTE = 0;
        private readonly PosFahrPlan __eltern;

        private readonly int __index;
        private readonly int __streckePunktIndex;

        public PosFahrPlanHalteStelle(PosFahrPlan fhPlan, int index,
            Tuple<PosLineStreckeEinKnoten, OshFahrplanSequenceElement> elementy, int fahrZeit)
        {
            __eltern = fhPlan;
            __index = index;
            Möglichkeiten = new Func<OshFahrplanElementType, PosFahrgastMöglichkeiten>(o =>
            {
                switch (o)
                {
                    case OshFahrplanElementType.NurEingang: return PosFahrgastMöglichkeiten.NurEingang;
                    case OshFahrplanElementType.NurAusgang: return PosFahrgastMöglichkeiten.NurAusgang;
                    case OshFahrplanElementType.RichtigeHaltestelle:
                        return PosFahrgastMöglichkeiten.AlleMöglichkeiten;
                    default:
                        throw new ArgumentException();
                }
            })(elementy.Item2.Status);
            FahrZeitBis_InMinutes = fahrZeit;
            HaltZeitInMinutes = elementy.Item2.TStop;
            __streckePunktIndex = elementy.Item1.ObjectId;
        }

        public PosFahrgastMöglichkeiten Möglichkeiten { get; set; }
        public int FahrZeitBis_InMinutes { get; }
        public int HaltZeitInMinutes { get; set; }
        public int AkumZeitBis_InMinutes { get; set; }

        public string AnkunftZeit => __index == __ERSTE
            ? "00:00:00"
            : $"{AkumZeitBis_InMinutes / 60:D2}:{AkumZeitBis_InMinutes % 60:D2}:00";

        public string AbfahrtZeit =>
            __index == __ERSTE
                ? "00:00:00"
                : $"{(AkumZeitBis_InMinutes + HaltZeitInMinutes) / 60:D2}:{(AkumZeitBis_InMinutes + HaltZeitInMinutes) % 60:D2}:00";

        //Index,LineName,LineRouteName,DirectionCode,TimeProfileName,LRItemIndex,Alight,Board,Arr,Dep

        public string Core()
        {
            return
                $"{__index + 1};{__eltern.Strecke.Linie.Name};{__eltern.Strecke.Name};{__eltern.Strecke.Richtung.ToStr()};{__eltern.Name};{__streckePunktIndex + 1};{(Möglichkeiten.In(new[] {PosFahrgastMöglichkeiten.NurAusgang, PosFahrgastMöglichkeiten.AlleMöglichkeiten}) ? 1 : 0)};{(Möglichkeiten.In(new[] {PosFahrgastMöglichkeiten.NurEingang, PosFahrgastMöglichkeiten.AlleMöglichkeiten}) ? 1 : 0)};{AnkunftZeit};{AbfahrtZeit}";
        }

        public int ObjectId { get; }
    }
}