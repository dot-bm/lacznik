using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Rosetta.DB_Reader.Blasewitz_DataModel;
using Rosetta.DB_Reader.Reick;

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

namespace Rosetta.DB_Reader.Common
{
    public abstract class Fabric<T, TKey> where T : IReickQuelle
    {
        protected Dictionary<TKey, Type> _items = new Dictionary<TKey, Type>();
        protected abstract string _Prefix();

        protected abstract string _KeyTemplate();

        protected void _AddP(TKey key, Type type)
        {
            _items.Add(key, type);
        }

        public virtual T Build(TKey kSym, DBConnectionPool connection, BlaCommonStorage bcs)
        {
            var local = (T) Activator.CreateInstance(_items[kSym]);
            local.Setup(connection, bcs);
            return local;
        }

        protected abstract TKey _CastKey(string inpkey);

        protected void _BuildList()
        {
            var rx = new Regex($"^{_Prefix()}_{_KeyTemplate()}_");
            var ipa = typeof(T);
            var q1 = GetType().Assembly.GetTypes();
            var q2 = q1.Where(t => ipa.IsAssignableFrom(t));
            var q3 = q2.Where(t => !t.IsAbstract);
            var q4 = q3.Where(t => rx.IsMatch(t.Name)).ToList();
            var pxLength = _Prefix().Length;
            foreach (var e4 in q4)
            {
                var n = rx.Match(e4.Name).Value.Remove(0, pxLength).Trim('_');

                _AddP(_CastKey(n), e4);
            }
        }
    }
}