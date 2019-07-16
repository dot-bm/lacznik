using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
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
    internal class Pha_00_VisumHeader : Pha_GG_Generic, IProhlisArbeitnehmer
    {
        public const byte EntID = 0;

        //public static IEnumerable<str>

        public override IEnumerable<BlaResultRow> PourResults()
        {
            return VisumHeader(this);
            //c.AddRange(__YieldVisumGarbages(c.Count));        
        }

        public byte EntityID => EntID;

        public override bool WorkInNetMode => true;

        public static IEnumerable<BlaResultRow> VisumHeader(Pha_00_VisumHeader tpha)
        {
            var y = 0;
            string nort;
            switch (tpha._WorkMode())
            {
                case Blasewitz.Bla_WorkMode.NetFile:
                    nort = "NET";
                    break;
                case Blasewitz.Bla_WorkMode.TraFile:
                    nort = "TRANS";
                    break;
                case Blasewitz.Bla_WorkMode.DemFile:
                    nort = "DEMAND";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Nieznany tryb pracy");
            }

            yield return new BlaResultRow {EntityID = EntID, OrdNum = y++, Content = "$VISION"};
            yield return new BlaResultRow
                {EntityID = EntID, OrdNum = y++, Content = "* Łącznik – DB to Model Transfer File/Network"};
            yield return new BlaResultRow
                {EntityID = EntID, OrdNum = y++, Content = "*"};
            var assembly = Assembly.GetCallingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            yield return new BlaResultRow
                {EntityID = EntID, OrdNum = y++, Content = $"* {fvi.ProductName} v. {fvi.FileVersion}"};
            yield return new BlaResultRow
                {EntityID = EntID, OrdNum = y++, Content = "$VERSION:VERSNR;FILETYPE;LANGUAGE;UNIT"};
            yield return new BlaResultRow {EntityID = EntID, OrdNum = y++, Content = "10;" + nort + ";ENG;KM"};
        }

        /// <summary>
        ///     Prawdopodobnie do wysłania do TSys
        /// </summary>
        /// <param name="starti"></param>
        /// <returns></returns>
        private static IEnumerable<BlaResultRow> __YieldVisumGarbages(int starti)
        {
            var y = starti;
            yield return new BlaResultRow {EntityID = EntID, OrdNum = y++, Content = "$-DEMANDSEGMENT:CODE"};
            yield return new BlaResultRow {EntityID = EntID, OrdNum = y++, Content = "C"};
            yield return new BlaResultRow {EntityID = EntID, OrdNum = y++, Content = "X"};
            yield return new BlaResultRow {EntityID = EntID, OrdNum = y++, Content = "$-MODE:CODE"};
            yield return new BlaResultRow {EntityID = EntID, OrdNum = y++, Content = "C"};
            yield return new BlaResultRow {EntityID = EntID, OrdNum = y++, Content = "X"};
            yield return new BlaResultRow {EntityID = EntID, OrdNum = y++, Content = "$-TSYS:CODE"};
            yield return new BlaResultRow {EntityID = EntID, OrdNum = y++, Content = "B"};
            yield return new BlaResultRow {EntityID = EntID, OrdNum = y++, Content = "C"};
        }
    }
}