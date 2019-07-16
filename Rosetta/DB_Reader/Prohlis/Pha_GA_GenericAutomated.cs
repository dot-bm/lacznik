using System;
using System.Collections.Generic;
using System.Linq;
using Rosetta.DB_Reader.Blasewitz_DataModel;
using Rosetta.Model.GrunaerWeg_Atts;

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
    ///     Klasa wprowadzająca pewne szkielety działań. Posiada nagłówek, automatycznie budowany z atrybutów (i abstrakcyjnej
    ///     definicji). Wydawanie danych metodą szablonową. Za to kompletnie nie pyta o własne dane encji
    /// </summary>
    //GB, 40, 41, 42, 90, 91
    internal abstract class Pha_GA_GenericAutomated : Pha_GG_Generic, IProhlisArbeitnehmer
    {
        public Pha_GA_GenericAutomated(byte entityID)
        {
            EntityID = entityID;
        }

        /// <summary>
        ///     Potomek musi zdefiniować podstawową strukturę nagłówka Visum
        /// </summary>
        protected abstract string _HEader_ConstString { get; }

        /// <summary>
        ///     Numer Encji
        /// </summary>
        public byte EntityID { get; }

        //protected 

        public override IEnumerable<BlaResultRow> PourResults()
        {
            //atrybuty własne
            var myObjects = _OwnObj_AsString().ToList();

            if (myObjects.Count == 0) return _ReturnEmpty();

            var myatts = _phs.Attributies.Definitions.OfEntity(EntityID).ToArray();

            var firstAttSep = _AttValuesSeparator(_phs.Attributies.Definitions.CountOfEntity((byte) (EntityID % 100)));

            var attsOfObj = _EnsureAllObjectsHaveAttValues()
                .OrderBy(y => y.ObjectID).ThenBy(y => y.RelObjID)
                .Select(y => myatts.ProduceOutput(y.Tail));

            return myObjects.OrderBy(a => a.OrdNum).ThenBy(y => y.RelObjID)
                .Zip(attsOfObj, (obj, att) => new {obj, att})
                .SelectMany(zipObj => zipObj.obj.Explode(firstAttSep + zipObj.att))
                //new BlaResultRow() { EntityID = EntityID, OrdNum = inx, Content = zipObj.obj.Content + firstAttSep + zipObj.att }) 
                .Concat(_HEader_PushComplete());
        }

        protected virtual Blasewitz.Bla_RowMode _RowModeInTra()
        {
            return Blasewitz.Bla_RowMode.Add;
        }

        /// <summary>
        ///     Przygotowuje kompletną definicję nagłówka (zbiera z atrybutów)
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<BlaResultRow> _HEader_PushComplete()
        {
            var ht = _phs.Attributies.Definitions.OfEntity(EntityID).HeaderTail();
            if (_HEader_ConstString.Last() == ':') ht = ht.TrimStart(';');
            var hea = _HEader_ConstString + ht;
            return _HeaderEnum(hea, EntityID,
                _phs.CurrentBatch.Mode == Blasewitz.Bla_WorkMode.TraFile
                    ? _RowModeInTra()
                    : Blasewitz.Bla_RowMode.Null);
            //return hea;
        }


        //protected GrWAtt_Definitions _GatherMyAttribDefs()
        //{
        //    return new GrWAtt_Definitions(_phs.Attributies.Definitions.OfEntity(EntityID));
        //}

        //protected virtual IEnumerable<GrWAtt_Value> _Values()
        //{
        //    return _phs.Attributies.Values.OfEntity(EntityID);
        //}

        /// <summary>
        ///     Możliwa do przeładowania metoda zwracająca pierwszy po części stałej separator
        /// </summary>
        /// <param name="activeAttCount">Liczba aktywnych parametrów</param>
        /// <returns>Separator bądź ciag separujacy</returns>
        protected virtual string _AttValuesSeparator(int activeAttCount)
        {
            return activeAttCount > 0 ? ";" : "";
        }

        //protected IEnumerable<BlaResultRow> _PourRe

        /// <summary>
        ///     Wylicza identyfikatory obiektów (jeśli trzeba – podwójne)
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<Tuple<int, int?>> _OwnObj_IdentsOnly();

        /// <summary>
        ///     Wynikowe wartości podstawowe jako stringi
        /// </summary>
        protected abstract IEnumerable<BlaBinResultRow> _OwnObj_AsString();

        /// <summary>
        ///     Zbiera wartości atrybutów dla encji, a następnie powoduje, że każdy rekord obiektu posiada wymagane wartości
        ///     atrybutów (puste, jeśli trzeba)
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<PhaInterFerryStruct> _EnsureAllObjectsHaveAttValues()
        {
            return _OwnObj_IdentsOnly().GroupJoin(_phs.Attributies.Values.OfEntity(EntityID),
                o => new {ObjID = o.Item1, RelID = o.Item2}, v => new {ObjID = v.ObjectID, RelID = v.RelObjID},
                (o, na) => new PhaInterFerryStruct {ObjectID = o.Item1, RelObjID = o.Item2, Tail = na});
        }
    }
}