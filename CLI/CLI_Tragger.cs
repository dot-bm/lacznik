using System;
using System.ComponentModel;
using System.IO;
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

namespace Rosetta.CLI
{
    internal class CLI_Träger
    {
        private readonly byte? __connID;


        public CLI_Träger(string[] args)
        {
            Reporter = new RoReporter();
            Reporter.PropertyChanged += __Reporter_MessageSent;
            if (args.Length > 0) XMLFile = args[0];
            if (args.Length > 1) WrokPath = args[1];
            try
            {
                if (string.IsNullOrWhiteSpace(WrokPath)) WrokPath = _.I.Cfg.Root;
            }
            catch
            {
            }

            if (args.Length > 2)
            {
                var s = args[2];
                byte y;
                if (byte.TryParse(s, out y)) __connID = y;
            }
        }

        public string XMLFile { get; } = "";

        public string WrokPath { get; } = "";

        public RoReporter Reporter { get; }
        public BlaRequest Request { get; private set; }

        public bool WorkPathExists => Directory.Exists(WrokPath);

        internal void WriteLog()
        {
            try
            {
                Reporter.SaveLog(Path.ChangeExtension(XMLFile, "log"));
            }
            catch (Exception e)
            {
                Reporter.AddException(e, "CLI");
            }
        }

        internal bool SaveResults()
        {
            try
            {
                DBReader.WriteStreamsToFiles(WrokPath, Request, Reporter, "CLI");
            }
            catch (Exception e)
            {
                Reporter.AddException(e, "CLI");
            }

            return Reporter.IsOK;
        }

        internal bool Perform()
        {
            try
            {
                var n = new DBReader(Reporter);
                n.Execute(Request, __connID);
            }
            catch (Exception E)
            {
                Reporter.AddException(E, "CLI");
            }

            return Reporter.IsOK;
        }

        public bool LoadXMLRequest()
        {
            if (string.IsNullOrWhiteSpace(XMLFile))
            {
                Reporter.AddMessage(new RoReMessage
                    {Caller = "CLI", Message = "Nieprawidłowa nazwa pliku", EvType = RoReEventType.Error});
                return Reporter.IsOK;
            }

            if (!File.Exists(XMLFile))
            {
                Reporter.AddMessage(new RoReMessage
                    {Caller = "CLI", Message = "Plik XML nie istnieje", EvType = RoReEventType.Error});
                return Reporter.IsOK;
            }

            try
            {
                Reporter.AddMessage(new RoReMessage
                    {Caller = "CLI", EvType = RoReEventType.Info, Message = "Odczyt pliku XML"});
                Request = BlaRequest.LoadFromFile(XMLFile);
                Reporter.AddMessage(new RoReMessage
                    {Caller = "CLI", EvType = RoReEventType.Info, Message = "Odczytano pliku XML"});
            }
            catch (Exception e)
            {
                Reporter.AddException(e, "CLI");
            }

            return Reporter.IsOK;
        }

        private void __Reporter_MessageSent(object sender, PropertyChangedEventArgs e)
        {
            Console.WriteLine(Reporter.LastMsg);
        }
    }
}