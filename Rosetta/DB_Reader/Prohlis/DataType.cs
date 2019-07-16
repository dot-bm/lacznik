using System.Collections.Generic;
using Rosetta.Model.GrunaerWeg_Atts;

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
    //public class BlaResultRow
    //{
    //    /// <summary>
    //    /// ID encji
    //    /// </summary>
    //    public byte EntityID;
    //    /// <summary>
    //    /// Numer w kolejności
    //    /// </summary>
    //    public int OrdNum;
    //    /// <summary>
    //    /// Zawartość
    //    /// </summary>
    //    public string Content;
    //}

    //public class BlaBinResultRow : BlaResultRow
    //{
    //    public int? RelObjID;
    //}

    //public class PhaNumberAttribStruct {
    //    public int St { get; private set; }
    //    public int ObjectID { get; private set; }
    //    public int RelObjID { get; private set; }
    //    public int AttributeID { get; private set; }
    //    public short SubID { get; private set; }
    //    public decimal Value { get; private set; }
    //    public PhaNumberAttribStruct(SqlDataReader r)
    //    {
    //        St = r.GetInt32(0);
    //        ObjectID = r.GetInt32(1);
    //        RelObjID = r.GetInt32(2);
    //        AttributeID = r.GetInt32(3);
    //        SubID = r.GetInt16(4);
    //        Value = r.GetDecimal(5);
    //    }
    //}


    public struct PhaInterFerryStruct
    {
        public int ObjectID;
        public int? RelObjID;
        public IEnumerable<GrWAtt_Value> Tail;
    }
}