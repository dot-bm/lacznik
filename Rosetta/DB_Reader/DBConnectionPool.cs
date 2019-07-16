using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security;

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

namespace Rosetta.DB_Reader
{
    /// <summary>
    ///     Struktura zarządzania użytkownikiem w połączeniu
    /// </summary>
    public class DBUser4Pool
    {
        public string Password;
        public string User;
    }

    /// <summary>
    ///     Opakowanie dla połączenia – ulatwi jego zarządzaniem
    /// </summary>
    public class DBConnectionPool
    {
        private SqlConnection __bind;

        private DBUser4Pool __credentails;
        private string __database;

        /// <summary>
        ///     Użycie MARS – MultipleActiveResultSets
        /// </summary>
        private bool __mars = true;

        private string __server;
        private int __timeout = 30;

        internal DBConnectionPool(SqlConnection cn)
        {
            __bind = cn;
        }

        public DBConnectionPool()
        {
        }

        /// <summary>
        ///     Użycie MARS – MultipleActiveResultSets
        /// </summary>
        public bool MARS
        {
            get => __mars;
            set
            {
                if (value != __mars) __ResetAfterSettingsChange();
                __mars = value;
            }
        }

        public int TimeOut
        {
            get => __timeout;
            set
            {
                if (value != __timeout) __ResetAfterSettingsChange();
                __timeout = value;
            }
        }

        internal SqlConnection Connection
        {
            get
            {
                if (__bind == null) __bind = new SqlConnection();
                __OpenConnection();
                return __bind;
            }
        }

        public string Server
        {
            get => __server;
            set
            {
                __ResetAfterSettingsChange();
                __server = value;
            }
        }

        public string Database
        {
            get => __database;
            set
            {
                __ResetAfterSettingsChange();
                __database = value;
            }
        }

        public DBUser4Pool Credentials
        {
            get => __credentails;
            set
            {
                __ResetAfterSettingsChange();
                __credentails = value;
            }
        }

        public void __ResetAfterSettingsChange()
        {
            if (__bind != null && __bind.State == ConnectionState.Open) __bind.Close();
        }

        private void __OpenConnection()
        {
            if (__bind.State != ConnectionState.Open)
            {
                __bind.ConnectionString =
                    string.Format(
                        "Server={0};Database={1};Trusted_Connection={2};Connection Timeout={3};MultipleActiveResultSets={4}",
                        __server, __database, __SPPI(), __timeout, __mars);
                if (__SPPI())
                {
                    __bind.Open();
                }
                else
                {
                    var sst = new SecureString();
                    foreach (var s in __credentails.Password) sst.AppendChar(s);
                    var cre = new SqlCredential(__credentails.User, sst);
                    __bind.Credential = cre;
                    __bind.Open();
                }
            }
        }

        private bool __SPPI()
        {
            return __credentails == null;
        }

        public SqlDataReader ExecuteReader(string command, IEnumerable<SqlParameter> parameters = null)
        {
            var cmd = new SqlCommand(command, Connection);
            if (parameters != null) cmd.Parameters.AddRange(parameters.ToArray());
            cmd.CommandTimeout = 0;
            return cmd.ExecuteReader();
        }

        public IEnumerable<IDataRecord> ExecuteQuery(string command, IEnumerable<SqlParameter> parameters = null)
        {
            return ExecuteReader(command, parameters).Cast<IDataRecord>();
        }

        public static IEnumerable<int> FieldIndices(IEnumerable<string> fieldNames, IDataRecord rec)
        {
            var count = rec.FieldCount;
            var fnames = new string[count];
            for (var i = 0; i < count; i++) fnames[i] = rec.GetName(i);
            return fieldNames.Select(y => Array.FindIndex(fnames, u => u == y));
        }
    }
}