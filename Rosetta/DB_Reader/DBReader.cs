using System;
using System.IO;
using Rosetta.DB_Reader;
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

namespace Rosetta
{
    public class DBReader
    {
        public DBReader() : this(new RoReporter())
        {
        }

        public DBReader(RoReporter reporter)
        {
            Reporter = reporter;
        }

        public RoReporter Reporter { get; private set; }

        /// <summary>
        ///     Wykonuje opisane w żądaniu operacje, zwracając w nim rezultaty
        /// </summary>
        /// <returns></returns>
        public bool Execute(BlaRequest req, byte? connId = null)
        {
            //RoReporter rep = null;
            //Dictionary<byte, Stream> res = null;
            DBReaderBatch n = null;
            try
            {
                n = new DBReaderBatch(req, Reporter, connId);
                //rep = n.Reporter;
                if (Reporter.IsOK) n.FragQuellen(req.Reicks);
                foreach (var batch in req.Batches)
                    if (Reporter.IsOK)
                        n.PerformBatch(batch);
                //if (Reporter.IsOK)
                //{
                //    res = new Dictionary<byte, Stream>();
                //    res.Add(0, n.Log);
                //    foreach (var t in n.Results)
                //    {
                //        res.Add(t.Key, t.Value);
                //    }

                //}
            }
            catch (Exception e)
            {
                var q = Reporter == null;
                if (q) Reporter = new RoReporter();
                Reporter.AddException(e, "Błąd krytyczny ogólny");
            }

            //if (res == null)
            //{
            //    res = new Dictionary<byte, Stream>();
            //    res.Add(0, Reporter.SaveLog());
            //}
            return Reporter != null ? n.Reporter.IsOK : false;
        }

        public static void WriteStreamsToFiles(string catalog, BlaRequest request, RoReporter reporter, string caller)
        {
            if (!Directory.Exists(catalog))
            {
                reporter.AddMessage(new RoReMessage
                {
                    Caller = caller, EvType = RoReEventType.Error,
                    Message = string.Format("Katalog {0} nie istnieje", catalog)
                });
                return;
            }

            try
            {
                foreach (var brs in request.Batches)
                {
                    if ((brs.Result == null) | string.IsNullOrWhiteSpace(brs.FileName))
                    {
                        reporter.AddMessage(new RoReMessage
                        {
                            Caller = caller, EvType = RoReEventType.Warning,
                            Message = string.Format("Pomijanie zapisu pliku dla podzlecenia {0}", brs.BatchId)
                        });
                        continue;
                    }

                    var path = Path.Combine(catalog, brs.FileName);
                    reporter.AddMessage(new RoReMessage
                    {
                        Caller = caller, EvType = RoReEventType.Info,
                        Message = string.Format("Zapis pliku dla podzlecenia {0} [{1}]", brs.BatchId, path)
                    });
                    if (!Directory.Exists(Path.GetDirectoryName(path)))
                        Directory.CreateDirectory(Path.GetDirectoryName(path));
                    WriteStreamToFile(brs.Result, path);
                }
            }
            catch (Exception e)
            {
                reporter.AddException(e, caller);
            }
        }

        public static void WriteStreamToFile(Stream inStream, string filename)
        {
            using (var fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                inStream.Seek(0, SeekOrigin.Begin);
                inStream.CopyTo(fs);
                fs.Flush(true);
            }
        }
    }
}