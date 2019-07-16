using System.Collections.Generic;
using System.Linq;
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
    public class PosLinieStrecke : IKarlsruheReady
    {
        private readonly List<PosFahrPlan> __fahrPlanen;

        private readonly List<PosLineStreckeEinKnoten> __knoten;

        public PosLinieStrecke(PosLinie derEltern, IEnumerable<OshFahrplanSequence> yg)
        {
            var myList = yg.OrderBy(u => u.LX4610).ToList();
            var yg1 = myList.First();
            ObjectId = yg1.LX4610;
            Name =
                $"{yg1.GenStreckeName(int.Parse(_.I.Cfg.Templater("StopNamesInRouteName").Coalesce("3")))} (#{ObjectId})";

            Linie = derEltern;
            Richtung = yg1.Elements.First().LX4010 <= yg1.Elements.Last().LX4010
                ? PosStreckeRichtung.Hin
                : PosStreckeRichtung.Zu;
            IstRund = yg1.Elements.First().LX4010 == yg1.Elements.Last().LX4010;
            __knoten = yg1.Elements.Select(ne => new PosLineStreckeEinKnoten(this, ne)).ToList();
            __fahrPlanen = yg.Select(g => new PosFahrPlan(this, g)).ToList();
        }

        public string Name { get; }
        public PosStreckeRichtung Richtung { get; }
        public bool IstRund { get; }

        public IEnumerable<PosFahrPlan> Profiles =>
            __fahrPlanen == null ? Enumerable.Empty<PosFahrPlan>() : __fahrPlanen;

        public IEnumerable<PosLineStreckeEinKnoten> Items =>
            __knoten == null ? Enumerable.Empty<PosLineStreckeEinKnoten>() : __knoten;

        public PosLinie Linie { get; }

        public string Core()
        {
            return $"{Linie.Name};{Name};{Richtung.ToStr()};{(IstRund ? 1 : 0)}";
        }

        public int ObjectId { get; }
    }
}