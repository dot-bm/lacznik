using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
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
    ///     Definicja wykorzystywanych atrybutów (w tym UDA)
    /// </summary>
    public class GrWAtt_Definition
    {
        public const string SQLCOMMAND =
            "SELECT ID7110, LX7101, SubAttr, Nazwa, VisumString, Użytkownika, LX7102, NumDecPlaces, StringDefaultValue, LX711G, LX7103 FROM [mgm].[F7110_VisumAttributes] (@dslice, @model)";

        public GrWAtt_Definition(IDataRecord r)
        {
            var f = -1;
            try
            {
                AttID = r.GetInt32(++f);
                EntityID = r.GetByte(++f);
                SubAttrID = (short) (r.IsDBNull(++f) ? -1 : r.GetInt32(2));
                Name = r.GetString(++f);
                VisumDefinition = r.IsDBNull(++f) ? "" : r.GetString(4);
                //switch (r.GetInt32(++f))
                //{
                //    case 0:
                //        UserDefined = false;
                //        break;
                //    case 1:
                //        UserDefined = false;
                //        break;
                //    default:
                //        throw new BlaEUnexpectedValue("Wartość pola \"Użytkownika\" może wynosić tylko 0 lub 1");
                //}
                UserDefined = r.GetBoolean(++f);
                DataType = (BlaVisumDataType) r.GetByte(++f);
                __NumDecPlaces = r.IsDBNull(++f) ? 0 : Math.Max(r.GetInt32(7), 0);
                __DefStringValue = r.IsDBNull(++f) ? "" : r.GetString(8);
            }
            catch (InvalidCastException ice)
            {
                throw new InvalidCastException("Nieprawidłowe rzutowanie w polu " + f + ", typ pola to " +
                                               r.GetFieldType(f));
            }
        }

        public GrWAtt_Definition(SqlDataReader r)
        {
            //[ID7110], [LX7101], [Pom], [Name], [U], [Użytkownika], [LX7102], [NumDecPlaces], [StringDefaultValue]
            AttID = r.GetInt32(0);
            DataType = (BlaVisumDataType) r.GetInt32(6);
            EntityID = (byte) r.GetInt32(1);
            SubAttrID = (short) r.GetInt32(2);
            Name = r.GetString(3);
            __DefStringValue = r.GetString(8);
            __NumDecPlaces = Math.Max(r.GetInt32(7), 0);
            __Format = "F" + (DataType == BlaVisumDataType.Integer ? "0" : __NumDecPlaces.ToString());
        }

        /// <summary>
        ///     Formatuje w zadany sposób wejściową wartość
        /// </summary>
        /// <param name="pav"></param>
        /// <returns></returns>
        public string Format(GrWAtt_Value pav)
        {
            if (pav == null) return __DefStringValue;
            var pnv = pav as PhaNumericAttribValue;
            switch (DataType)
            {
                case BlaVisumDataType.Area:
                    return pnv.Value.ToString(__Format, CultureInfo.InvariantCulture) + "km2";
                case BlaVisumDataType.Length:
                    return pnv.Value.ToString(__Format, CultureInfo.InvariantCulture) + "km";
                case BlaVisumDataType.Precise_time:
                    return pnv.Value.ToString(__Format, CultureInfo.InvariantCulture) + "s";
                case BlaVisumDataType.String:
                    var sav = pav as PhaStringAttribValue;
                    return sav != null ? sav.Value : "";
                case BlaVisumDataType.Time_period:
                    return pnv.Value.ToString("F0", CultureInfo.InvariantCulture) + "s";
                case BlaVisumDataType.Velocity:
                    return pnv.Value.ToString(__Format, CultureInfo.InvariantCulture) + "km/h";
                default:
                    //int, dec, double...
                    return pnv.Value.ToString(__Format, CultureInfo.InvariantCulture);
            }

            throw new IndexOutOfRangeException();
        }

        #region Definitions

        public bool UserDefined { get; }
        public string Name { get; }

        /// <summary>
        ///     LX7110
        /// </summary>
        public int AttID { get; }

        /// <summary>
        ///     LX7101
        /// </summary>
        public byte EntityID { get; }

        public short SubAttrID { get; }
        public string VisumDefinition { get; }
        public BlaVisumDataType DataType { get; }
        private readonly int __NumDecPlaces;
        private readonly string __Format;
        private readonly string __DefStringValue;

        #endregion
    }
}