using System;
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
    public class PosFahrPlan : IKarlsruheReady
    {
        private readonly List<PosFahrPlanHalteStelle> __haltestellen;

        private UniqueList<PosKurs, int> __abfahrten;

        public PosFahrPlan(PosLinieStrecke strecke, OshFahrplanSequence sequence)
        {
            ObjectId = sequence.LX4610;
            //Name = sequence.LX4610.ToString();
            Strecke = strecke;
            __haltestellen = new List<PosFahrPlanHalteStelle>();
            __Parse(sequence);
        }

        public string Name => ObjectId.ToString();

        public PosLinieStrecke Strecke { get; }

        public IEnumerable<PosFahrPlanHalteStelle> HalteStellen =>
            __haltestellen ?? Enumerable.Empty<PosFahrPlanHalteStelle>();

        public int HalteStelleAnzahl => __haltestellen.Count;

        public IEnumerable<PosKurs> Kurse => __abfahrten ?? Enumerable.Empty<PosKurs>();

        public string Core()
        {
            return $"{Strecke.Linie.Name};{Strecke.Name};{Strecke.Richtung.ToStr()};{Name}";
        }
        //LineName LineRouteName DirectionCode Name

        public int ObjectId { get; }


        private void __Parse(OshFahrplanSequence sequence)
        {
            var zl = Strecke.Items.Zip(sequence.Elements,
                (kn, el) => new Tuple<PosLineStreckeEinKnoten, OshFahrplanSequenceElement>(kn, el)).ToList();
            var el1 = zl.First();
            var fhs = new PosFahrPlanHalteStelle(this, 0, el1, 0)
            {
                Möglichkeiten = PosFahrgastMöglichkeiten.NurEingang
            };
            __haltestellen.Add(fhs);
            var index = 0;
            var fahrZeit = 0;
            var akumFahrZeit = 0;
            foreach (var el in zl.Skip(1))
            {
                fahrZeit += el.Item2.TRun;
                if (el.Item2.Status.In(new[]
                {
                    OshFahrplanElementType.NurAusgang, OshFahrplanElementType.NurEingang,
                    OshFahrplanElementType.RichtigeHaltestelle
                }))
                {
                    akumFahrZeit += fahrZeit;
                    fhs = new PosFahrPlanHalteStelle(this, ++index, el, fahrZeit)
                        {AkumZeitBis_InMinutes = akumFahrZeit};
                    akumFahrZeit += el.Item2.TStop;
                    __haltestellen.Add(fhs);
                    fahrZeit = 0;
                }
            }

            //Ostani tylko wysiadają – poprawka
            fhs.Möglichkeiten = PosFahrgastMöglichkeiten.NurAusgang;
            fhs.HaltZeitInMinutes = 0;
        }

        public void KursHinzufügen(PosKurs kurs)
        {
            if (__abfahrten == null) __abfahrten = new UniqueList<PosKurs, int>(k => k.ObjectId);
            __abfahrten.Add(kurs);
        }
    }
}