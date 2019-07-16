using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

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
    public class NonUniqueKeyException : Exception
    {
        public NonUniqueKeyException()
        {
        }

        public NonUniqueKeyException(string message) : base(message)
        {
        }

        public NonUniqueKeyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NonUniqueKeyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class UniqueList<T, TIx> : List<T>
    {
        private readonly HashSet<TIx> __index = new HashSet<TIx>();
        private readonly Func<T, TIx> __indexValueExtractor;
        public bool SuppressNonUniqueException = false;

        public UniqueList(Func<T, TIx> indexValueExtractor)
        {
            __indexValueExtractor = indexValueExtractor;
        }

        public UniqueList(Func<T, TIx> indexValueExtractor, int capacity) : base(capacity)
        {
            __indexValueExtractor = indexValueExtractor;
        }

        public UniqueList(Func<T, TIx> indexValueExtractor, IEnumerable<T> collection) : base(collection)
        {
            __indexValueExtractor = indexValueExtractor;
        }

        public new void Add(T element)
        {
            var iElement = __indexValueExtractor(element);
            if (__index.Any(c => c.Equals(iElement)))
            {
                if (!SuppressNonUniqueException)
                    throw new NonUniqueKeyException($"Attemp to add element with non-unique key {iElement.ToString()}");
            }
            else
            {
                base.Add(element);
                __index.Add(iElement);
            }
        }

        public new void AddRange(IEnumerable<T> collection)
        {
            var ge = collection.Select(r => new {key = __indexValueExtractor(r), obj = r}).GroupBy(t => t.key)
                .Select(t => new {key = t.Key, obj = t.First(), innproper = t.Count() == 1}).GroupJoin(__index,
                    ae => ae.key,
                    ix => ix, (ae, ixs) => new {ae.key, ae.obj.obj, proper = ae.innproper && !ixs.Any()}).ToList();
            if (!SuppressNonUniqueException && !ge.All(r => r.proper))
                throw new NonUniqueKeyException(
                    $"Attemp to add element(s) with non-unique key: {ge.Select(h => h.key).ToString(", ")}");
            var proper = ge.Where(g => g.proper).ToList();
            base.AddRange(proper.Select(n => n.obj));
            __index.AddValues(proper.Select(y => y.key).ToArray());
        }

        public new void Remove(T item)
        {
            base.Remove(item);
            __index.Remove(__indexValueExtractor(item));
        }

        public new void Clear()
        {
            base.Clear();
            __index.Clear();
        }

        public new void RemoveAt(int index)
        {
            __index.Remove(__indexValueExtractor(base[index]));
            base.RemoveAt(index);
        }

        public new void RemoveRange(int index, int count)
        {
            throw new NotImplementedException();
        }
    }

    public static class UniqueListHelper
    {
        public static UniqueList<T, TIx> ToUniqueList<T, TIx>(this IEnumerable<T> collection, Func<T, TIx> keyExtractor)
        {
            var nlist = new UniqueList<T, TIx>(keyExtractor);
            nlist.AddRange(collection);
            return nlist;
        }
    }
}