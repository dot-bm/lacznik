using System;
using System.Data;
using Rosetta.CoreTools;
using Rosetta.KarlsruherStr;

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

namespace Rosetta.Model.Trachau
{
    public class DSeg : Id_Sym_NameObject, IKarlsruheReady
    {
        private readonly Mode __mode;

        public DSeg(IDataRecord inprec, ModesList modes) : base(inprec)
        {
            var b = false;
            //try
            //{
            if (!inprec.IsDBNull(3))
            {
                __mode = modes[inprec.GetIntNumber(3)];
                b = true;
            }

            if (!b && !inprec.IsDBNull(4))
            {
                __mode = modes[inprec.GetIntNumber(4)];
                b = true;
            }

            if (!b)
                throw new ArgumentException(
                    "Nie można utworzyć Segmentu Popytu be z odwołania do Gałęzi Transportu");
            //}
            //catch()
        }

        public string Core()
        {
            return $"{Symbol};{Nazwa};{__mode.Symbol}";
        }
    }
}