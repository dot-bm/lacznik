using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Rosetta.DB_Reader.Blasewitz_DataModel;
using Rosetta.DB_Reader.Prohlis;
using Rosetta.DB_Reader.Reick;
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

namespace Rosetta.DB_Reader
{
    /// <summary>
    ///     Klasa agregująca przetwarzanie danych – fasada
    /// </summary>
    public class DBReaderBatch
    {
        public readonly Dictionary<byte, Stream> Results;
        private readonly DBConnectionPool __conn;
        private ProhlisFabric __phafabric;
        private ReickFabric __rckfabric;
        private readonly BlaCommonStorage __storage;

        /// <param name="project">ID projektu</param>
        /// <param name="year">Rok prognozy</param>
        /// <param name="variant">Ewentualne ID wariantu</param>
        /// <param name="model">ID modelu – ma znaczenie dla wydawanych parametrów</param>
        public DBReaderBatch(BlaRequest request, RoReporter rep = null, byte? connID = null)
        {
            Reporter = rep ?? new RoReporter();
            Results = new Dictionary<byte, Stream>();
            try
            {
                Reporter.AddMessage(new RoReMessage
                    {Caller = "Inicjalizacja", EvType = RoReEventType.ModuleCall, Message = "Inicjalizacja systemu"});
                __conn = _.SetupConnection(connID);
                Reporter.AddMessage(new RoReMessage
                {
                    Caller = "Inicjalizacja", Message = $"DBServer: {__conn.Server}, Database: {__conn.Database}",
                    EvType = RoReEventType.Info
                });
                __storage = new BlaCommonStorage(__conn, request);
                __storage.Reporter = Reporter;
                __storage.Variant = new BlaDBVariantDefinition(__conn, request);
            }
            catch (Exception e)
            {
                Reporter.AddException(e, "Inicjalizacja");
            }
        }

        public RoReporter Reporter { get; }

        public Stream Log => Reporter.SaveLog();

        /// <param name="rcks">Lista odpytywanych (wg kolejności) komponentów źródła</param>
        public void FragQuellen(string[] rcks)
        {
            if (!Reporter.IsOK) return;
            __rckfabric = new ReickFabric();
            var b = rcks.All(p => __DoRck(p));
        }

        /// <summary>
        ///     Wykonuje zlecony ciąg transformacji
        /// </summary>
        /// <param name="batch">Batch request</param>
        public bool PerformBatch(BlaReqBatch batch)
        {
            //MemoryStream outStream = null;
            if (!Reporter.IsOK) return false;
            __phafabric = new ProhlisFabric();
            __phafabric.InitializeBatch(batch.BatchId, __storage);
            return batch.PhaIDs.All(p => __DoPha(p));
            //var outputs = new List<Tuple<byte, Stream>>();
            // try
            // {
            //     if (b && __reporter.IsOK)
            //     {
            //         if (__storage.HasResults)
            //         {
            //             outStream = new MemoryStream();
            //             StreamWriter sw = new StreamWriter(outStream, encoding: Encoding.Unicode);
            //             b = __phafabric.BatchResults().All(a => { sw.Write(a); return true; });
            //             sw.Flush();
            //             if (b) batch.Result = outStream;
            //         }
            //     }
            // }
            // catch (ArgumentException ine)
            // {
            //     __reporter.AddException(ine, "Stream saver");
            // } 
            // catch (Exception e)
            // {
            //     __reporter.AddException(e, "Stream saver");
            // }
            //// outputs.Add(new Tuple<byte, Stream>(0, __reporter.SaveLog()));
            // return new Tuple<byte, Stream>(batch.BatchId, outStream);
        }

        private bool __DoRck(string Rck_ID)
        {
            var calllerString = "Reick Module[" + Rck_ID + "]";
            try
            {
                __Msg_Init(calllerString);
                var quelle = __rckfabric.Build(Rck_ID, __conn, __storage);
                if (Reporter.IsOK) __CheckPrerequisities(calllerString, quelle);
                if (Reporter.IsOK) __Prepare(calllerString, quelle);
                if (Reporter.IsOK) __Msg_Finalize(calllerString);
            }
            catch (Exception e)
            {
                Reporter.AddException(e, "Reick Module[" + Rck_ID + "]");
            }

            return Reporter.IsOK;
        }

        private bool __DoPha(byte Pha_ID)
        {
            var calllerString = "Pha " + Pha_ID;
            try
            {
                __Msg_Init(calllerString);
                var abnehmer = __phafabric.Build(Pha_ID, __conn, __storage);
                if (__storage.CurrentBatch.Mode == Blasewitz.Bla_WorkMode.NetFile
                    && !abnehmer.WorkInNetMode)
                    throw new BlaENotInNetMode("Not suppoorted mode for this Pha");
                if (Reporter.IsOK) __CheckPrerequisities(calllerString, abnehmer);
                if (Reporter.IsOK) __Prepare(calllerString, abnehmer);
                if (Reporter.IsOK)
                {
                    __storage.CurrentBatch.AddResults(abnehmer.PourResults());
                    __Msg_Finalize(calllerString);
                }
            }
            catch (Exception e)
            {
                Reporter.AddException(e, "Pha " + Pha_ID);
            }

            return Reporter.IsOK;
        }

        private void __Msg_Finalize(string calllerString)
        {
            Reporter.AddMessage(new RoReMessage
                {Caller = calllerString, EvType = RoReEventType.Info, Message = "Wykonanie zakończone"});
        }

        private void __Msg_Init(string calllerString)
        {
            Reporter.AddMessage(new RoReMessage
                {Caller = calllerString, EvType = RoReEventType.ModuleCall, Message = "Rozpoczęcie wykonania"});
        }

        private void __Prepare(string calllerString, IReickQuelle nehmer)
        {
            if (!nehmer.Prepare())
                Reporter.AddMessage(new RoReMessage
                {
                    Caller = calllerString, EvType = RoReEventType.Error,
                    Message = "Niemożliwe przygotowanie danych dla tego modułu Pha"
                });
        }

        private void __CheckPrerequisities(string calllerString, IReickQuelle nehmer)
        {
            if (!nehmer.CheckPrerequisities(Reporter))
                Reporter.AddMessage(new RoReMessage
                {
                    Caller = calllerString, EvType = RoReEventType.Error,
                    Message = "Brak wymaganych wcześniej przygotowanych danych dla tego Pha"
                });
        }
    }
}