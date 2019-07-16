using System;
using System.Collections.Generic;
using System.Data;
using Rosetta.CoreTools;
using Rosetta.Model.Mickten;

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
    public class OshFahrplanSequenceElement
    {
        private readonly bool __valid;

        public List<OshFahrplanSequenceElement> Children;
        public int LpPunktu;
        public int LX3121;
        public int LX3125;
        public byte LX4002;
        public int LX4010;
        public byte LX4602;
        public int LX4610;
        public int LX4650;
        public string NazwaKl;
        public MckNode Node;
        public string SymbolKl;
        public int TRun;
        public int TStop;

        //LX4610, LpPunktu, LX4010, LX3125, LX3121, Valid, LX4002, LX4601, Czas, LX4650, Symbol, Nazwa
        public OshFahrplanSequenceElement(IDataRecord r)
        {
            var i = -1;
            try
            {
                LX4610 = r.GetIntNumber(++i);
                LpPunktu = r.GetIntNumber(++i);
                LX4010 = r.GetIntNumber(++i);
                LX3125 = r.GetIntNumber(++i);
                LX3121 = r.GetIntNumber(++i);
                __valid = r.GetIntNumber(++i) == 1;
                LX4002 = (byte) r.GetIntNumber(++i);
                LX4602 = (byte) r.GetIntNumber(++i);
                TRun = r.GetIntNumber(++i);
                TStop = r.GetIntNumber(++i);
                LX4650 = r.GetIntNumber(++i);
                SymbolKl = r.IsDBNull(++i) ? "—" : r.GetString(i);
                NazwaKl = r.IsDBNull(++i) ? "" : r.GetString(i);
            }
            catch (Exception e)
            {
                throw new RoE_DataReaderException($"Błąd przy odczycie/konwersji pola {i}", e);
            }
        }

        public OshFahrplanElementType Status =>
            Node.HasHalteStelle
                ? Node.HalteStelle.HaltOfTSys.IsOfTSys(LX3121)
                    ? __WieGehstSekwention()
                    : OshFahrplanElementType.HaltestelleMitSperrenTSys
                : OshFahrplanElementType.NurDerKnoten;

        public bool IsValid =>
            __valid && (Node?.HasHalteStelle ?? false) && Node.HalteStelle.HaltOfTSys.IsOfTSys(LX3121);

        private OshFahrplanElementType __WieGehstSekwention()
        {
            switch (LX4602)
            {
                case 0:
                    return OshFahrplanElementType.RichtigeHaltestelleAberUberspringen;
                case 1:
                    return OshFahrplanElementType.RichtigeHaltestelle;
                case 2:
                    return OshFahrplanElementType.NurEingang;
                case 3:
                    return OshFahrplanElementType.NurAusgang;
                default:
                    throw new ArgumentException();
            }
        }

        public OshFahrplanSequenceElement AdaptNode(MckNode node)
        {
            Node = node;
            return this;
        }
    }
}