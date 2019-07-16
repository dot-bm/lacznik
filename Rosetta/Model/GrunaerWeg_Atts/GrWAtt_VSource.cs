using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rosetta.CoreTools;
using Rosetta.DB_Reader;
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

namespace Rosetta.Model.GrunaerWeg_Atts
{
    /// <summary>
    ///     Klasa mapująca źródła atrybutów
    /// </summary>
    public class GrWAtt_VSource
    {
        private int __optionalSubAttFieldIndex = -1;

        private int __valueFieldIndex = -1;

        public GrWAtt_VSource(IDataRecord input)
        {
            ValueField = (string) input["NazwaPola"];
            var osaf = input["IDSubAtrybutu_NazwaPola"];
            if (!(osaf is DBNull)) OptionalSubAttField = (string) osaf;
            ID7110_AttributeID = (int) input["LX7110"];
            LX7125 = (int) input["LX7125"];
            EntityID = (byte) input["LX7101"];
            DataType = (BlaVisumDataType) (byte) input["LX7102"];
        }

        public string ValueField { get; }
        public string OptionalSubAttField { get; }
        public int ID7110_AttributeID { get; }
        public int LX7125 { get; }
        public byte EntityID { get; }

        public BlaVisumDataType DataType { get; }
        //private Type __valueFieldType = typeof(DBNull);

        public IEnumerable<string> Fields()
        {
            if (!string.IsNullOrWhiteSpace(ValueField)) yield return ValueField;
            if (!string.IsNullOrWhiteSpace(OptionalSubAttField)) yield return OptionalSubAttField;
        }

        public GrWAtt_Value GetValue(int objectID, IDataRecord rec)
        {
            if (__valueFieldIndex < 0)
            {
                var t = DBConnectionPool.FieldIndices(Fields(), rec).ToArray();
                __valueFieldIndex = t[0];
                if (t.Length > 1) __optionalSubAttFieldIndex = t[1];
            }

            //if (__valueFieldType == typeof(DBNull))
            //{
            //    __valueFieldType = rec.GetFieldType(__valueFieldIndex);
            //}
            if (rec.IsDBNull(__valueFieldIndex)) return null;
            GrWAtt_Value pva = null;
            switch (DataType)
            {
                case BlaVisumDataType.Integer:
                case BlaVisumDataType.Area:
                case BlaVisumDataType.Decimal:
                case BlaVisumDataType.Double:
                case BlaVisumDataType.Length:
                case BlaVisumDataType.Precise_time:
                case BlaVisumDataType.Time_period:
                case BlaVisumDataType.Velocity:
                    var ivs = new PhaNumericAttribValue();
                    __QuickSetupAttValue(objectID, rec, ivs);
                    var va = rec.GetValue(__valueFieldIndex);

                    var c = va.GetType();
                    ivs.Value = Convert.ToDecimal(va);
                    pva = ivs;
                    break;
                case BlaVisumDataType.String:
                    var nvs = new PhaStringAttribValue();
                    __QuickSetupAttValue(objectID, rec, nvs);
                    nvs.Value = rec.GetString(__valueFieldIndex);
                    pva = nvs;
                    break;
                default:
                    throw new InvalidExpressionException();
            }

            return pva;
        }

        private void __QuickSetupAttValue(int objectID, IDataRecord rec, GrWAtt_Value nvs)
        {
            nvs.ObjectID = objectID;
            nvs.SubID = __optionalSubAttFieldIndex >= 0
                ? (short) rec.GetIntNumber(__optionalSubAttFieldIndex)
                : Blasewitz.NO_SUBID;
            nvs.AttributeID = ID7110_AttributeID;
            nvs.EntityID = EntityID;
        }
    }
}