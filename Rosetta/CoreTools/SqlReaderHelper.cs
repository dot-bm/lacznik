using System;
using System.Data;
using System.Data.SqlClient;

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

namespace Rosetta.CoreTools
{
    public static class SqlReaderHelper
    {
        public static string GetStringOrDefault(this SqlDataReader reader, int i)
        {
            return reader.IsDBNull(i) ? "" : reader.GetString(i);
        }

        public static int? GetInt32OrDefault(this SqlDataReader reader, int i)
        {
            return reader.IsDBNull(i) ? (int?) null : reader.GetInt32(i);
        }

        public static short? GetInt16OrDefault(this SqlDataReader reader, int i)
        {
            return reader.IsDBNull(i) ? (short?) null : reader.GetInt16(i);
        }

        public static byte? GetByteOrDefault(this SqlDataReader reader, int i)
        {
            return reader.IsDBNull(i) ? (byte?) null : reader.GetByte(i);
        }

        public static decimal? GetDecimalOrDefault(this SqlDataReader reader, int i)
        {
            return reader.IsDBNull(i) ? (decimal?) null : reader.GetDecimal(i);
        }

        public static decimal GetFixNumber(this IDataRecord rec, int fieldIndex)
        {
            var stype = rec.GetDataTypeName(fieldIndex);
            decimal v;
            switch (stype)
            {
                case "tinyint":
                    v = rec.GetByte(fieldIndex);
                    break;
                case "int":
                    v = rec.GetInt32(fieldIndex);
                    break;
                case "decimal":
                    v = rec.GetDecimal(fieldIndex);
                    break;
                case "float":
                    v = (decimal) rec.GetDouble(fieldIndex);
                    break;
                default:
                    throw new ArgumentException($"Nieznay typ pola {stype}");
            }

            //object o = rec[fieldIndex];
            return v;
        }

        public static decimal? GetFixNumberOrDefault(this IDataRecord rec, int fieldIndex)
        {
            if (rec.IsDBNull(fieldIndex)) return null;
            return rec.GetFixNumber(fieldIndex);
        }

        public static int GetIntNumber(this IDataRecord rec, int fieldIndex)
        {
            var stype = rec.GetDataTypeName(fieldIndex);
            int v;
            switch (stype)
            {
                case "tinyint":
                    v = rec.GetByte(fieldIndex);
                    break;
                case "int":
                    v = rec.GetInt32(fieldIndex);
                    break;
                case "smallint":
                    v = rec.GetInt16(fieldIndex);
                    break;
                default:
                    throw new ArgumentException($"Nieznay typ pola {stype}");
            }

            //object o = rec[fieldIndex];
            return v;
        }

        public static int GetIntNumber(this SqlDataReader rec, int fieldIndex)
        {
            var stype = rec.GetDataTypeName(fieldIndex);
            int v;
            switch (stype)
            {
                case "tinyint":
                    v = rec.GetByte(fieldIndex);
                    break;
                case "int":
                    v = rec.GetInt32(fieldIndex);
                    break;
                case "smallint":
                    v = rec.GetInt16(fieldIndex);
                    break;
                default:
                    throw new ArgumentException($"Nieznay typ pola {stype}");
            }

            //object o = rec[fieldIndex];
            return v;
        }

        public static int? GetIntNumberOrDefault(this IDataRecord rec, int fieldIndex)
        {
            if (rec.IsDBNull(fieldIndex)) return null;
            return rec.GetIntNumber(fieldIndex);
            //string stype = rec.GetDataTypeName(fieldIndex);
            //int v;
            //switch (stype)
            //{
            //    case "tinyint":
            //        v = rec.GetByte(fieldIndex);
            //        break;
            //    default:
            //        throw new ArgumentException($"Nieznay typ pola {stype}");
            //}
            ////object o = rec[fieldIndex];
            //return v;
        }

        //public static SqlGeometry GetGeometry(this SqlDataReader rec, int fieldName)
        //{
        //    //SpatialBuilder sb = new SpatialBuilder();
        //    SqlGeometry g = new SqlGeometry();
        //    if (!rec.IsDBNull(fieldName))
        //    {
        //        var c = rec.GetSqlBytes(fieldName);
        //        var c1 = c.Stream;
        //        var bn = new BinaryReader(c1);
        //        g.Read(bn);
        //    }
        //    return g;
        //}

        //public static DbGeometry GetGeometryFromWKText(this IDataRecord rec, int fieldIndex, int srid)
        //{
        //    if (rec.IsDBNull(fieldIndex))
        //    {
        //        return null;
        //    }
        //    string wkt = rec.GetString(fieldIndex);
        //    DbGeometry ge = DbGeometry.FromText(wkt, srid);
        //    return ge;
        //}


        //public static IEnumerable<Tuple<decimal, decimal>> GetLinePoints(this SqlGeometry geom)
        //{
        //    //SqlGeometry g = new SqlGeometry();
        //    //var n = rec.GetData(fieldName);
        //    //var s = ((SqlDataReader)n).GetSqlBytes(fieldName).Stream;
        //    //g.Read(new BinaryReader(s));
        //    // as SqlGeometry;
        //    //g.Read(new BinaryReader(rec.GetValue(0).Stream));
        //    //if (g == null)
        //    //{
        //    //    //return Enumerable.Empty<Tuple<decimal, decimal>>();
        //    //    yield break;
        //    //}
        //    if ((bool) geom.STIsEmpty())
        //    {
        //        yield break;
        //    }
        //    for (int i = 1; i < geom.STNumPoints() - 1; i++)
        //    {
        //        var po = geom.STPointN(i);
        //        yield return new Tuple<decimal, decimal>((decimal)(double)geom.STX, (decimal)(double)geom.STY);
        //    }
        //}
    }
}