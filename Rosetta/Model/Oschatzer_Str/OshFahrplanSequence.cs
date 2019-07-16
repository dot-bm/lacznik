using System;
using System.Collections.Generic;
using System.Linq;
using Rosetta.CoreTools;
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

namespace Rosetta.Model.Oschatzer_Str
{
    public class OshFahrplanSequence
    {
        public readonly List<OshFahrplanSequenceElement> Elements;
        public readonly int LX3121;
        public readonly int LX3125;
        public readonly int LX4610;
        public int LX4650;
        public string NazwaKl;
        public string SymbolKl;
        public TSys TSys;

        public OshFahrplanSequence(IEnumerable<OshFahrplanSequenceElement> elements)
        {
            Elements = elements.OrderBy(y => y.LpPunktu).Select((u, i) =>
            {
                u.LpPunktu = i;
                return u;
            }).ToList();
            var e1 = Elements.First();
            LX4610 = e1.LX4610;
            LX3125 = e1.LX3125;
            LX3121 = e1.LX3121;
            SymbolKl = e1.SymbolKl;
            NazwaKl = e1.NazwaKl;
            LX4650 = e1.LX4650;
        }

        public Tuple<int, int, int, TSys> HashLine => new Tuple<int, int, int, TSys>(
            Math.Min(Elements.First().LX4010, Elements.Last().LX4010),
            Math.Max(Elements.First().LX4010, Elements.Last().LX4010), LX4650, TSys);

        public string HashLineRoute => string.Join(" ", Elements.Select(el => el.LX4010.ToString()));

        public bool IsValid => Elements.Count >= 2 &&
                               Elements.First().Status.In(new[]
                                   {OshFahrplanElementType.NurEingang, OshFahrplanElementType.RichtigeHaltestelle}) &&
                               Elements.Last().Status.In(new[]
                                   {OshFahrplanElementType.RichtigeHaltestelle, OshFahrplanElementType.NurAusgang});

        public OshFahrplanSequence AdaptTSys(TSys tsys)
        {
            TSys = tsys;
            return this;
        }

        public string GenStreckeName(int maxUHs)
        {
            var uber = Elements.Skip(1).Take(Elements.Count - 2).Where(u => u.Status.In(new[]
            {
                OshFahrplanElementType.RichtigeHaltestelle, OshFahrplanElementType.NurEingang,
                OshFahrplanElementType.NurAusgang
            })).ToList();
            var tsh = Math.Max(uber.Count / maxUHs, 1);
            var uber2 = uber.OrderBy(y => y.LpPunktu).Select(y => new {gr = y.LpPunktu / tsh, element = y})
                .GroupBy(t => t.gr)
                .Select(ingr =>
                    ingr.OrderByDescending(ne => ne.element.Node.HalteStelle.CountHEvents)
                        .ThenBy(ne => ne.element.Node.ObjectId).FirstOrDefault()).Where(y => y != null)
                .Select(y => y.element).AddValues(Elements.First(), Elements.Last()).OrderBy(y => y.LpPunktu)
                .Select(y => y.Node.HalteStelle.Name);
            return string.Join(" – ", uber2);
        }
    }
}