using System.Collections.Generic;
using System.Linq;
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

namespace Rosetta.Model.Trachau
{
    /// <summary>
    ///     Klasa zarządzająca listą TSys'ów
    /// </summary>
    public class TSysList
    {
        private const string __QUERY =
            "SELECT ID31U0, Symbol, Nazwa, LX3002, [Type], PCU, LX4002, ID3111, ID3112 FROM [dem].[F310V_U_TSys] (@demSlice)";

        private readonly List<TSys> __itslist;

        public TSysList(DBConnectionPool pool, BlaCommonStorage bls)
        {
            //var tsyses = pool.ExecuteQuery(__QUERY, new SqlParameter[] { new SqlParameter("@demSlice", bls.Variant.iLX3050) });
            var tsyses = bls.EntityReader[3].Query(pool, bls.Variant);
            __itslist = tsyses.Select(r =>
            {
                switch (r.GetByte(3))
                {
                    case 0:
                    case 1:
                    case 2:
                        return new TSys(r);
                    case 100:
                        return new PrTTSys(r);
                    default:
                        throw new BlaEUnexpectedValue("Nieznany typ TSys");
                }
            }).ToList();
        }

        public TSys this[int Id]
        {
            get { return __itslist?.FirstOrDefault(c => c.ID == Id); }
        }

        public IEnumerable<TSys> Items()
        {
            return __itslist;
        }

        public IEnumerable<TSys> Pedestran()
        {
            return __itslist?.Where(y => y.Type == Trachau.TSysType.PuTWalk) ?? Enumerable.Empty<TSys>();
        }

        public IEnumerable<TSys> PrT()
        {
            return __itslist?.Where(y => y.Type == Trachau.TSysType.PrT) ?? Enumerable.Empty<TSys>();
        }

        public IEnumerable<TSys> PuT()
        {
            return __itslist?.Where(y => (y.Type == Trachau.TSysType.PuT) | (y.Type == Trachau.TSysType.PuTAux)) ??
                   Enumerable.Empty<TSys>();
        }
    }
}