using System;
using System.Collections.Generic;
using System.Linq;
using Rosetta.KarlsruherStr;
using Rosetta.Model.Oschatzer_Str;
using Rosetta.Model.Trachau;

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
    public class PosLinie : IKarlsruheReady
    {
        private readonly List<PosLinieStrecke> __lstrecken;

        public PosLinie(Tuple<int, int, int, TSys> hash, IEnumerable<OshFahrplanSequence> input, string nameFormat)
        {
            var cinput = input.ToList();
            var cel = cinput.First();
            ObjectId = cinput.Min(f => f.LX4610);
            TSys = cel.TSys;
            FareId = cel.LX4650;
            object[] nameton =
            {
                ObjectId.ToString(), cel.Elements.First().Node.HalteStelle.Name,
                cel.Elements.Last().Node.HalteStelle.Name, TSys.Symbol, cel.SymbolKl, cel.NazwaKl, FareId.ToString()
            };
            Name = string.Format(nameFormat, nameton);
            __lstrecken = cinput.GroupBy(y => y.HashLineRoute).Select(yg => new PosLinieStrecke(this, yg)).ToList();
        }

        public IEnumerable<PosLinieStrecke> Strecken => __lstrecken;

        public string Name { get; }
        public TSys TSys { get; }

        /// <summary>
        ///     Reserved for future use
        /// </summary>
        public int FareId { get; }

        public string Core()
        {
            return $"{Name};{TSys.Symbol}";
        }

        public int ObjectId { get; }
    }
}