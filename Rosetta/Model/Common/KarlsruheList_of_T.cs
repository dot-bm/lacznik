using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rosetta.CoreTools;
using Rosetta.DB_Reader;
using Rosetta.DB_Reader.Blasewitz_DataModel;
using Rosetta.KarlsruherStr;
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

namespace Rosetta.Model
{
    public abstract class KarlsruheList<T> : IKarlsruheReadyCollection where T : IKarlsruheReady
    {
        protected UniqueList<T, int> _kitems;

        public KarlsruheList()
        {
            _kitems = new UniqueList<T, int>(_keyExtractor);
        }

        public T this[int id]
        {
            get
            {
                var a = _kitems.FirstOrDefault(d => d.ObjectId == id);
                return a != null
                    ? a
                    : throw new ArgumentException($"Brak elementu o id {id}");
            }
        }

        public IEnumerable<T> Items => _kitems;

        public abstract string HeaderCore { get; }

        IEnumerable<IKarlsruheReady> IKarlsruheReadyCollection.Items()
        {
            return Items.Cast<IKarlsruheReady>();
        }

        protected virtual int _keyExtractor(T item)
        {
            return item.ObjectId;
        }

        public abstract bool Load(DBConnectionPool pool, BlaCommonStorage bls);

        protected virtual T _KarObjCreation(IDataRecord inprec)
        {
            return default(T);
        }

        protected bool _Load(DBConnectionPool pool, BlaCommonStorage platform, int johQueryIndex)
        {
            var b = true;
            try
            {
                _kitems.AddRange(platform.EntityReader[johQueryIndex].Query(pool, platform.Variant)
                    .Select(d => _KarObjCreation(d)));
                platform.Reporter.AddMessage(new RoReMessage
                {
                    Caller = GetType().Name, EvType = RoReEventType.Info, Message = $"Ilość elementów: {_kitems.Count}"
                });
            }
            catch (Exception e)
            {
                b = false;
                platform.Reporter.AddException(e, GetType().Name);
            }

            return b;
        }
    }
}