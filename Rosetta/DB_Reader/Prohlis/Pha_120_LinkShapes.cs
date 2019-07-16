using System.Collections.Generic;
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
    internal class Pha_120_LinkShapes : Pha_GG_Generic, IProhlisArbeitnehmer
    {
        public byte EntityID => 120;

        public override IEnumerable<BlaResultRow> PourResults()
        {
            var en = _phs.Graph.Links.Items().Where(y => y.HasMidPoints).ToList();
            if (en.Count == 0)
            {
            }
            else
            {
                yield return _HeaderEnum("LINKPOLY:FROMNODENO;TONODENO;INDEX;XCOORD;YCOORD;ZCOORD", EntityID,
                    _phs.CurrentBatch.Mode == Blasewitz.Bla_WorkMode.TraFile
                        ? Blasewitz.Bla_RowMode.Add
                        : Blasewitz.Bla_RowMode.Null).First();
                var i = 0;
                foreach (var link in en)
                {
                    var k = 1;
                    foreach (var xyPoint in link.MidPoints)
                        //int k = 1;
                        yield return new BlaResultRow
                        {
                            Content =
                                $"{link.P1.ObjectId};{link.P2.ObjectId};{k++};{xyPoint.sX};{xyPoint.sY};0",
                            EntityID = EntityID,
                            OrdNum = i++
                        };
                }
            }

            //return base.PourResults();
        }

        public override bool WorkInNetMode => true;
    }
}