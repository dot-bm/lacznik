using System;
using System.Linq;
using Rosetta.CoreTools;
using Rosetta.Model.Mickten;
using Rosetta.Model.Oschatzer_Str;
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

namespace Rosetta.DB_Reader.Reick
{
    internal class Rck_FhP_Fahrplan : Rck_Generic, IReickQuelle
    {
        public override bool Prepare()
        {
            //LpPunktu, LX4010, LX3125, LX3121, Valid, LX4002, LX4601, Czas, LX4650, Symbol, Nazwa
            var j = _platform.EntityReader[461].Query(_cpool, _platform.Variant)
                .Select(n => new OshFahrplanSequenceElement(n)).ToList();
            var jj = j.Join(_platform.Graph.Nodes.Items, jk => jk.LX4010, ni => ni.ObjectId,
                (jk, ni) => jk.AdaptNode(ni));
            var jjj = jj.GroupBy(el => el.LX4610).Select(g => new OshFahrplanSequence(g))
                .Join(_platform.DemFoundation.TSysList.PuT(), jk => jk.LX3121, ts => ts.ID,
                    (jk, ts) => jk.AdaptTSys(ts)).ToList();
            jjj.Where(l => !l.IsValid).OrderBy(t => t.SymbolKl).ThenBy(t => t.LX4610).ToList().ForEach(lv =>
                _platform.Reporter.AddMessage(new RoReMessage
                {
                    EvType = RoReEventType.Warning,
                    Message =
                        $"Sekwencja zatrzymań o id {lv.LX4610} (klasa PuT {lv.SymbolKl}) jest nieprawidłowa i nie zostanie przetworzona"
                }));
            jjj.Where(l => l.IsValid)
                .SelectMany(l => l.Elements.Where(ne => ne.Status.In(new[]
                {
                    OshFahrplanElementType.RichtigeHaltestelle, OshFahrplanElementType.NurEingang,
                    OshFahrplanElementType.NurAusgang
                }))).GroupBy(ne => ne.Node.HalteStelle).Select(ng => new Tuple<MckStop, int>(ng.Key, ng.Count()))
                .ToList().ForEach(hs => hs.Item1.CountHEvents = hs.Item2);
            _platform.PuT.Lines.ParseAndAdd(jjj.Where(l => l.IsValid));
            //.Join(_platform.DemFoundation.TSysList.PuT(), jk => jk.LX3121, ts => ts.ID, (jk, ts) => jk.AdaptTSys(ts))
            return true;
        }

        public override bool CheckPrerequisities(RoReporter reporter)
        {
            return _CheckIf_TSysList_Exists(reporter);
        }
    }
}