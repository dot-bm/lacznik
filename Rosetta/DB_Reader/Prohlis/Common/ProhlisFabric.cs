using System;
using System.Collections.Generic;
using System.Linq;
using Rosetta.DB_Reader.Blasewitz_DataModel;
using Rosetta.DB_Reader.Common;
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
    ///     Fabryka abstrakcyjna obiektów tworzących kod TRA
    /// </summary>
    public class ProhlisFabric : Fabric<IProhlisArbeitnehmer, byte>
    {
        //public IProhlisArbeitnehmer Build(byte Pha_ID, DBConnectionPool connection, BlaCommonStorage bcs)
        //{
        //    __local = (IProhlisArbeitnehmer)Activator.CreateInstance(_items[Pha_ID]);
        //    __local.Setup(connection, bcs);
        //    return __local;
        //}

        ///// <summary>
        ///// Uzupełniana na bieżąco konstrukcja listy
        ///// </summary>
        //private void __BuildList()
        //{
        //    Regex rx = new Regex(@"^Pha_[0-9][0-9]_.*");
        //    Type ipa = typeof(IProhlisArbeitnehmer);
        //    try
        //    {
        //        var q1 =GetType().Assembly.GetTypes();
        //        var q2 = q1.Where(t => ipa.IsAssignableFrom(t));
        //        var q3 = q2.Where(t=>!t.IsAbstract);
        //        var q4 = q3.Where(t=>rx.IsMatch(t.Name)).ToList();
        //        foreach (var e4 in q4)
        //        {
        //            string n = e4.Name.Substring(4, 2).TrimStart('0');
        //            if (string.IsNullOrWhiteSpace(n))
        //            {
        //                n = "0"; }
        //            byte i = byte.Parse(n);
        //            __AddP(i, e4);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }

        //    //__AddP(0, typeof(Pha_00_VisumHeader));
        //    //__AddP(1, typeof(Pha_01_UDA_Definitions));
        //    ////__AddP(2, typeof(Pha_02_AttReader));
        //    //__AddP(10, typeof(Pha_10_Nodes));
        //    //__AddP(21, typeof(Pha_21_LinkTypes));
        //    //__AddP(30, typeof(Pha_30_Zones));
        //    //__AddP(2, typeof(Pha_02_Network));
        //    //__AddP(20, typeof(Pha_20_Links));
        //    //__AddP(24, typeof(Pha_24_LetsGetAllThisPuT));
        //    //__AddP(32, typeof(Pha_32_Connectors));
        //    //__AddP(3, typeof(Pha_03_TSys));
        //    //__AddP(4, typeof(Pha_04_Mode));
        //    //__AddP(42, typeof(Pha_42_Stops));
        //    //__AddP(14, typeof(Pha_14_LetsGetStopsData));
        //    //__AddP(41, typeof(Pha_41_StopAreas));
        //    //__AddP(40, typeof(Pha_40_StopPoints));
        //    //__AddP(90, typeof(Pha_90_Lines));
        //}

        //private void __AddP(byte key, Type obj)
        //{
        //    __PhaS.Add(key, obj);
        //}

        public ProhlisFabric()
        {
            //__PhaS = new Dictionary<byte, Type>();
            //__storage = new PhaCommonStorage();
            _BuildList();
        }

        internal BlaCommonStorage Storage { get; private set; }
        //private Dictionary<byte, Type> __PhaS;

        public IProhlisArbeitnehmer Local { get; private set; }

        /// <summary>
        ///     Zwraca obiekt tłumaczący TRA
        /// </summary>
        /// <param name="PhA_ID">ID klasy obiektu tłumaczącego</param>
        /// <param name="connection">Połączenie do bazy danych</param>
        /// <returns>Obiekt tłumaczący TRA</returns>
        public IProhlisArbeitnehmer Build(byte PhA_ID, DBConnectionPool connection = null)
        {
            return Build(PhA_ID, connection, Storage);
        }

        public override IProhlisArbeitnehmer Build(byte kSym, DBConnectionPool connection, BlaCommonStorage bcs)
        {
            Local = base.Build(kSym, connection, bcs);
            return Local;
        }

        public void InitializeBatch(byte batchId, BlaCommonStorage bcst)
        {
            Storage = bcst;
            try
            {
                Storage.CurrentBatch = Storage.Request.Batches.Single(n => n.BatchId == batchId);
            }
            catch (Exception e)
            {
                throw new BlaEBadRequest($"Brak wartości lub duplikacja dla zadania {batchId}");
            }

            //__storage.Results.Clear();
        }

        /// <summary>
        ///     Depreciated
        /// </summary>
        /// <param name="project"></param>
        /// <param name="year"></param>
        /// <param name="variant"></param>
        /// <param name="model"></param>
        /// <param name="connection"></param>
        /// <param name="reporter"></param>
        /// <param name="workMode"></param>
        public void InitializeBatch(int project, decimal year, int? variant, int model, DBConnectionPool connection,
            RoReporter reporter, Blasewitz.Bla_WorkMode workMode)
        {
            throw new NotImplementedException("Funkcja zdeprecjonowana");
            //__storage = new BlaCommonStorage(workMode)
            //{
            //    Variant = new BlaDBVariantDefinition(connection, project, variant, year, model),
            //    Reporter = reporter
            //};
        }

        public void FinalizeBatch()
        {
            Storage = null;
        }

        //private Dictionary<byte, byte> __entityRemaper = new Dictionary<byte, byte>() { { 20, 21 }, { 21, 20 }, { 42, 40 }, { 40, 42 } };

        //public IEnumerable<string> BatchResults()
        //{
        //    if (__storage == null) { return Enumerable.Empty<string>(); }
        //    return __storage.Results
        //        .GroupJoin(__entityRemaper, o => o.EntityID, i => i.Key, (ore, ire) =>
        //        {
        //            if ((ire != null) && (ire.Count() > 0))
        //            {
        //                ore.EntityID = ire.First().Value;
        //            }
        //            return ore;
        //        })
        //        .OrderBy(a => a.EntityID).ThenBy(a => a.OrdNum).Select(a => a.Content + "\n");
        //}

        /// <summary>
        ///     Wydaje rozszerzone rezultaty wykonania (np. kształty rejonów)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> BatchExtResults()
        {
            throw new NotImplementedException();
            //return 
            //  Pha_00_VisumHeader.VisumHeader().Concat(__storage.ExtResults).OrderBy(a => a.EntityID).ThenBy(a => a.OrdNum).Select(a => a.Content + "\n");
        }

        /// <summary>
        ///     Wykonuje akcje pracobiorcy
        /// </summary>
        /// <param name="phs">Wspólne miejsce zapisu</param>
        public bool Use(BlaCommonStorage phs)
        {
            //bool r = true;
            var r = Local.Prepare();
            if (r)
            {
                if (Storage.CurrentBatch.Mode == Blasewitz.Bla_WorkMode.NetFile
                    && !Local.WorkInNetMode)
                    throw new BlaENotInNetMode("Not suppoorted mode for this Pha");
                phs.CurrentBatch.AddResults(Local.PourResults());
            }

            return r;
        }

        public bool Use()
        {
            return Use(Storage);
        }

        protected override string _Prefix()
        {
            return "Pha";
        }

        protected override string _KeyTemplate()
        {
            return "[0-9]+";
        }

        protected override byte _CastKey(string inpkey)
        {
            var n = inpkey.TrimStart('0');
            if (string.IsNullOrWhiteSpace(n)) n = "0";
            var i = byte.Parse(n);
            return i;
        }
    }
}