using System.IO;
using Rosetta.DB_Reader;
using Rosetta.DB_Reader.Gruna;

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

namespace Rosetta
{
    public sealed class _
    {
        private static volatile _ __instance;
        private static readonly object __syncRoot = new object();

        private static DBConnectionPool __db;

        public readonly Config2 Cfg;

        private GruArgTemplateReader __gruTmpl;

        private _()
        {
            Cfg = new Config2();
            ZoneShapesStream = new MemoryStream();
            LinkShapesStream = new MemoryStream();
        }

        public Stream LinkShapesStream { get; }

        public Stream ZoneShapesStream { get; }

        public GruArgTemplateReader GrunaTemplater
        {
            get
            {
                if (__gruTmpl == null)
                    lock (__syncRoot)
                    {
                        if (__gruTmpl == null)
                            __gruTmpl = new GruArgTemplateReader();
                    }

                return __gruTmpl;
            }
        }


        public static _ I
        {
            get
            {
                if (__instance == null)
                    lock (__syncRoot)
                    {
                        if (__instance == null)
                            __instance = new _();
                    }

                return __instance;
            }
        }

        /// <summary>
        ///     Metoda do tworzenia ad-hoc połączenia na podstawie konfiguracji
        /// </summary>
        /// <returns></returns>
        public static DBConnectionPool SetupConnection(byte? connID = null)
        {
            if (__db == null) __SetupConnection(connID);
            return __db;
        }

        private static void __SetupConnection(byte? connID = null)
        {
            __db = I.Cfg.ConnectionPool(connID);


            //__db = new DBConnectionPool()
            //{
            //    Server = I.Cfg.Entry["serverName"],
            //    Database = I.Cfg.Entry["database"]
            //};
        }
    }
}