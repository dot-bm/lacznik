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

namespace Rosetta.DB_Reader.Blasewitz_DataModel
{
    /// <summary>
    ///     Klasa zawierająca definicję przetwarzanego wariantu/roku
    /// </summary>
    public class BlaDBVariantDefinition
    {
        private static DataTable _arcre;
        private static DataTable _tt4052;

        public BlaDBVariantDefinition(DBConnectionPool conn, BlaRequest req)
        {
            Year = req.Slice.Year;
            iModel = req.Slice.Model;
            var com = new SqlCommand(
                "select LX3050, LX418C, LX428C, LX468P, LX418P, LX428P, LX468K, LX118R, LX338V, Nazwa from [mgm].[F7210_H02_VarRecord] (@Project, @Year, @Model" +
                (req.Slice.Variant != null ? ", @Variant" : ", null") + ")", conn.Connection);
            com.Parameters.AddWithValue("@Project", req.Slice.ProjectID);
            com.Parameters.AddWithValue("@Year", req.Slice.Year);
            com.Parameters.AddWithValue("@Model", req.Slice.Model);
            if (req.Slice.Variant != null)
            {
                var spa = com.Parameters.AddWithValue("@Variant", req.Slice.Variant);
                spa.DbType = DbType.Int32;
            }

            var r = com.ExecuteReader();
            if (!r.Read()) throw new ArgumentException("Nie można odczytać tabeli wariantów");
            iLX3050 = r.GetInt32(0);
            iLX418C = r.GetInt32(1);
            iLX428C = r.GetInt32(2);
            iLX468P = r.GetInt32(3);
            iLX418P = r.GetInt32(4);
            iLX428P = r.GetInt32(5);
            iLX468K = r.GetInt32(6);
            iLX118R = r.GetInt32(7);
            iLX338V = r.GetInt32(8);
            Nazwa = r.IsDBNull(9) ? "" : r.GetString(9);
        }

        public int iLX338V { get; }
        public int iLX3050 { get; }
        public int iLX418C { get; }
        public int iLX428C { get; }
        public int iLX468P { get; }
        public int iLX418P { get; }
        public int iLX428P { get; }
        public int iLX468K { get; }
        public int iLX118R { get; }
        public short Year { get; }
        public int iModel { get; }
        public string Nazwa { get; }

        public DateTime Date => new DateTime(Year, 6, 30);

        public object Get4Params(byte parId)
        {
            //0 – Rok
            //1	LX118R	Rejony
//            2   LX3050 Demand
//3   LX418C Kolej-Konstrukcja
//4   LX418P Kolej-Parametry
//5   LX428C Drogi-Konstrukcja
//6   LX428P Drogi-Parametry
//7   LX468P Rozkłady-Przebiegi
//8   LX468K Rozkłady-Kursy
            //9 LX338V  Scenariusz zmiennych
            //10 Model danych
            switch (parId)
            {
                case 0: return Year;
                case 1: return iLX118R;
                case 2: return iLX3050;
                case 3: return iLX418C;
                case 4: return iLX418P;
                case 5: return iLX428C;
                case 6: return iLX428P;
                case 7: return iLX468P;
                case 8: return iLX468K;
                case 9: return iLX338V;
                case 10: return iModel;
            }

            throw new IndexOutOfRangeException();
        }

        /// <summary>
        ///     Przygotowuje ustalone parametry komendy SQL
        /// </summary>
        /// <param name="com">Ustawiana komenda</param>
        /// <param name="KSlice">Wpisać przekrój kolejowy?</param>
        /// <param name="SSlice">Wpisać przekrój drogowy?</param>
        /// <param name="PuTSlice">Wpisać przekrój dla kursów?</param>
        /// <param name="Rok">Wpisać rok?</param>
        /// <param name="Structured">Wpisać parametry strukturalne (obsolete)?</param>
        public void PrepareSqlCommandParams(SqlCommand com, bool KSlice = true, bool SSlice = true,
            bool PuTSlice = true, bool Rok = true, bool Structured = true)
        {
            if (KSlice)
            {
                com.Parameters.AddWithValue("@K_ConstrSliceID", iLX418C);
                com.Parameters.AddWithValue("@K_ParamSliceID", iLX418P);
            }

            if (SSlice)
            {
                com.Parameters.AddWithValue("@S_ConstrSliceID", iLX428C);
                com.Parameters.AddWithValue("@S_ParamSliceID", iLX428P);
            }

            if (PuTSlice)
            {
                com.Parameters.AddWithValue("@Przebieg_SliceID", iLX468P);
                com.Parameters.AddWithValue("@Kurs_SliceID", iLX468K);
            }

            if (Rok) com.Parameters.AddWithValue("@Year", Year);
            if (Structured)
            {
                var parStops = com.Parameters.AddWithValue("@simplystop", _tt4052);
                parStops.SqlDbType = SqlDbType.Structured;
                parStops.TypeName = "[mgm].[__TT4052_ResultStruct]";
                var parArcre = com.Parameters.AddWithValue("@ArcResults", _arcre);
                parArcre.SqlDbType = SqlDbType.Structured;
                parArcre.TypeName = "[dbo].[__ArcResults]";
            }
        }

        public void PrepareSqlCommandParams(SqlCommand com, byte EntityID)
        {
            com.Parameters.AddWithValue("@EntityId", EntityID);
            PrepareSqlCommandParams(com);
        }

        private static void __MakeComplexParams()
        {
            if (_arcre == null)
            {
                _arcre = new DataTable();
                _arcre.Columns.AddRange(new[]
                {
                    new DataColumn("StartLX4010", typeof(int)), new DataColumn("EndLX4010", typeof(int)),
                    new DataColumn("LX4010", typeof(int)), new DataColumn("PredecessorLX4010", typeof(int)),
                    new DataColumn("OrderInArc", typeof(short)), new DataColumn("LX4002", typeof(byte))
                });
            }

            if (_tt4052 == null)
            {
                _tt4052 = new DataTable();
                _tt4052.Columns.AddRange(new[]
                {
                    new DataColumn("LX4110", typeof(int)), new DataColumn("LX4002", typeof(byte)),
                    new DataColumn("LX4050", typeof(short)), new DataColumn("LX4105", typeof(byte)),
                    new DataColumn("Nazwa", typeof(string)), new DataColumn("X", typeof(decimal)),
                    new DataColumn("Y", typeof(decimal)), new DataColumn("K_PomSymbol", typeof(string)),
                    new DataColumn("TSys", typeof(string)), new DataColumn("SumaLinii", typeof(int)),
                    new DataColumn("SumaZatrzymań", typeof(int))
                });
            }
        }
    }
}