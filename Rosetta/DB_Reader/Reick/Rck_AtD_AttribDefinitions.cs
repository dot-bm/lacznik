using System;
using System.Data.SqlClient;
using System.Linq;
using Rosetta.Model.GrunaerWeg_Atts;
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

namespace Rosetta.DB_Reader.Reick
{
    public class Rck_AtD_AttribDefinitions : Rck_Generic, IReickQuelle
    {
        public override bool Prepare()
        {
            var b = true;
            try
            {
                var results = _cpool.ExecuteQuery(GrWAtt_Definition.SQLCOMMAND,
                    new[]
                    {
                        new SqlParameter("@dslice", _platform.Variant.iLX3050),
                        new SqlParameter("@model", _platform.Variant.iModel)
                    });
                _platform.Attributies.SetDefinitions(results.Select(r => new GrWAtt_Definition(r)));
            }
            catch (Exception e)
            {
                b = false;
                _platform.Reporter.AddException(e, "AttDefsCollector");
            }

            _platform.Reporter.AddMessage(new RoReMessage
            {
                Caller = "AttsDefCollector", EvType = RoReEventType.Info,
                Message = $"Wczytano {_platform.Attributies.Definitions.Count} definicji"
            });
            return b;
        }

        public override bool CheckPrerequisities(RoReporter reporter)
        {
            var b = true;
            if (_platform.DemFoundation.TSysList == null)
            {
                reporter.AddMessage(new RoReMessage
                {
                    Caller = "Reick Module[AtD]", EvType = RoReEventType.Error,
                    Message = "Systemy Transportu (TSys) muszą być przewtorzone przed uruchomieniem tego modułu"
                });
                b = false;
            }

            return b;
        }
    }
}