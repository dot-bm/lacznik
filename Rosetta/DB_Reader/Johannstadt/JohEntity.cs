using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace Rosetta.DB_Reader.Johannstadt
{
    public class JohEntity
    {
        private readonly int __lx7121_ArgTmpl;
        public readonly byte EntityId;
        public readonly string FName_Seq;
        public readonly string FuncName;
        public readonly int Id;
        public readonly string O_Id_FName;

        public JohEntity(IDataRecord inprec)
        {
            Id = inprec.GetInt32(0);
            EntityId = inprec.GetByte(1);
            FuncName = $"[{inprec.GetString(2)}].[{inprec.GetString(3)}]";
            O_Id_FName = inprec.GetString(4);
            FName_Seq = inprec.IsDBNull(5) ? "" : inprec.GetString(5);
            __lx7121_ArgTmpl = inprec.GetInt32(6);
        }

        public IEnumerable<IDataRecord> Query(DBConnectionPool pool, BlaDBVariantDefinition variantDef)
        {
            return pool.ExecuteQuery(
                __GrunaCommand(),
                _.I.GrunaTemplater.PrepareCmd(__lx7121_ArgTmpl, variantDef));
        }

        private string __GrunaCommand()
        {
            return
                $"select {O_Id_FName}{(FName_Seq.Length > 0 ? "," : "")}{FName_Seq} from {FuncName}({_.I.GrunaTemplater.BracketContent(__lx7121_ArgTmpl)})";
        }

        public SqlDataReader Reader(DBConnectionPool pool, BlaDBVariantDefinition variantDef)
        {
            return pool.ExecuteReader(
                __GrunaCommand(),
                _.I.GrunaTemplater.PrepareCmd(__lx7121_ArgTmpl, variantDef));
        }
    }
}