﻿using System;
using System.Linq;
using Rosetta.CoreTools;
using Rosetta.Model.PostPlatz;
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
    internal class Rck_Kur_KurseCollector : Rck_Generic, IReickQuelle
    {
        public override bool Prepare()
        {
            var fahrPlanen = _platform.PuT.Lines.SelectMany(l => l.Strecken.SelectMany(ls => ls.Profiles)).ToList();
            if (!fahrPlanen.Any()) return true; //nie ma nic do roboty, to koniec
            var b = true;
            try
            {
                var boperator = _platform.Request.ContainsPha(68);
                using (var qr = _platform.EntityReader[465].Reader(_cpool, _platform.Variant))
                {
                    while (qr.Read())
                    {
                        var ifhPlan = qr.GetIntNumber(1);
                        var fhPlan = fahrPlanen.FirstOrDefault(f => f.ObjectId == ifhPlan);
                        if (fhPlan == null)
                        {
                            _platform.Reporter.Warn(
                                $"Kurs o id {qr.GetIntNumber(0)} posiada niewczytany numer sekwencji (przebiegu) (#{ifhPlan}) i nie zostanie przetworzony");
                            continue;
                        }

                        var neukurs = new PosKurs(fhPlan, qr);
                    }
                }
            }
            catch (Exception ex)
            {
                _platform.Reporter.AddException(ex);
                b = false;
            }

            return b;
        }

        public override bool CheckPrerequisities(RoReporter reporter)
        {
            if (!_platform.PuT.Lines.SelectMany(l => l.Strecken.SelectMany(ls => ls.Profiles)).Any())
                reporter.AddMessage(new RoReMessage
                {
                    EvType = RoReEventType.Warning,
                    Message = "Nie znaleziono żadnej linii/przebiegu/profilu. Nie będzie możliwe przypisanie kursów"
                });
            return true;
        }
    }
}