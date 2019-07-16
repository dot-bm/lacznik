using System.Data.SqlClient;
using Rosetta.Model.Kaditz;

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

namespace Rosetta.DB_Reader.Reick
{
    internal class Rck_LnR_LinkRailCollector : Rck_LinkCollcetor_OfT<KdzRailLink>, IReickQuelle
    {
        public Rck_LnR_LinkRailCollector() : base(2010, 2011, 2012)
        {
        }
        //public override bool Prepare()
        //{
        //    bool b = true;
        //    try
        //    {
        //        using (var rea = _platform.EntityReader[2010].Reader(_cpool, _platform.Variant))
        //        {
        //            while (rea.Read())
        //            {
        //                var y = new KdzRailLink(rea, _platform.Graph.Nodes);
        //                _platform.Graph.Links.Add(y);
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        _platform.Reporter.AddException(e, this.GetType().Name);
        //        b = false;
        //    }
        //    return b;
        //}

        protected override KdzRailLink _MakeLink(SqlDataReader rea)
        {
            return new KdzRailLink(rea, _platform.Graph.Nodes);
        }
    }
}