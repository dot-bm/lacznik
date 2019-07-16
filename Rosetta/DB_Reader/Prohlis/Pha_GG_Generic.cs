using System;
using System.Collections.Generic;
using System.Linq;
using Rosetta.DB_Reader.Blasewitz_DataModel;
using Rosetta.Reporting;

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
    /// <summary>
    ///     Bardzo ogólny szablon Prohlis, nie wykonujący nic poza przypisaniami
    /// </summary>
    internal class Pha_GG_Generic
    {
        protected DBConnectionPool _connection;

        protected BlaCommonStorage _phs;

        //public virtual IEnumerable<byte> Dependencies()
        //{
        //    yield break;
        //}

        public virtual bool WorkInNetMode => false;

        public virtual void Setup(DBConnectionPool connection, BlaCommonStorage phs)
        {
            _connection = connection;
            _phs = phs;
        }

        protected IEnumerable<BlaResultRow> _ReturnEmpty()
        {
            return Enumerable.Empty<BlaResultRow>();
        }

        protected Blasewitz.Bla_WorkMode _WorkMode()
        {
            return _phs.CurrentBatch.Mode;
        }

        public virtual bool Prepare()
        {
            return true;
        }

        protected IEnumerable<BlaResultRow> _HeaderEnum(string nvalue, byte entityID, Blasewitz.Bla_RowMode rowmode)
        {
            return Enumerable.Repeat(
                new BlaResultRow
                    {Content = "$" + Blasewitz.ToString(rowmode) + nvalue, EntityID = entityID, OrdNum = -1}, 1);
        }

        public virtual IEnumerable<BlaResultRow> PourResults()
        {
            throw new NotImplementedException();
        }

        public virtual bool CheckPrerequisities(RoReporter reporter)
        {
            return true;
        }
    }
}