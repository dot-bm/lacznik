using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

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

namespace Rosetta.Reporting
{
    /// <inheritdoc />
    /// <summary>
    ///     Rozgłasza informacje o zmianach
    /// </summary>
    public class RoReporter : INotifyPropertyChanged
    {
        private string __lastCaller;
        private byte __percentDone;

        private string __stage;

        public bool IsOK
        {
            get { return Messages.All(y => y.EvType != RoReEventType.Error); }
        }

        /// <summary>
        ///     Etap Pracy
        /// </summary>
        public string Stage
        {
            get => __stage;
            set
            {
                __stage = value;
                __OnPropertyChanged(() => Stage);
            }
        }

        public RoReMessage LastMsg => Messages.Last();

        public ObservableCollection<RoReMessage> Messages { get; } = new ObservableCollection<RoReMessage>();

        public byte PercentDone
        {
            get => __percentDone;
            set
            {
                __percentDone = value;
                __OnPropertyChanged(() => PercentDone);
            }
        }

        //public DepencyProperty a 
        public event PropertyChangedEventHandler PropertyChanged;

        public event ReportingEventHandler StateChanged;

        private void __onStateChanged(string msg = null)
        {
            var e = new ReportingEventArgs {Message = msg, Position = __percentDone};
            StateChanged?.Invoke(this, e);
        }

        public void AddInfo(string message)
        {
            AddMessage(new RoReMessage {EvType = RoReEventType.Info, Message = message});
        }

        public void Warn(string message)
        {
            AddMessage(new RoReMessage {EvType = RoReEventType.Warning, Message = message});
        }

        public void AddMessage(RoReMessage msg)
        {
            if (string.IsNullOrWhiteSpace(msg.Caller))
                msg.Caller = __lastCaller;
            else
                __lastCaller = msg.Caller;
            Messages.Add(msg);
            __OnPropertyChanged(() => msg.Message);
            __onStateChanged(msg.Message);
        }

        public void SaveLog(string filename)
        {
            File.WriteAllLines(filename, Messages.Select(y => y.ToString()));
        }

        public Stream SaveLog()
        {
            var logStream = new MemoryStream();
            var sw = new StreamWriter(logStream, Encoding.Unicode);
            var b = Messages.Select(y => y.ToString() + "\n").All(a =>
            {
                sw.Write(a);
                return true;
            });
            sw.Flush();
            return logStream;
        }

        public void AddException(Exception E, string caller = "")
        {
            var n = E;
            while (n != null)
            {
                AddMessage(new RoReMessage
                {
                    EvType = RoReEventType.Error,
                    Caller = caller,
                    Message = n.Message,
                    AdditTinfo = n.GetType().Name
                });
                n = n.InnerException;
            }

            AddMessage(new RoReMessage
            {
                EvType = RoReEventType.Info,
                Caller = caller,
                Message = E.StackTrace,
                AdditTinfo = E.GetType().Name
            });
        }

        public void Clear()
        {
            PercentDone = 0;
            Messages.Clear();
        }

        public static string __GetPropertyName<T>(Expression<Func<T>> expression)
        {
            var body = (MemberExpression) expression.Body;
            return body.Member.Name;
        }

        private void __OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        private void __OnPropertyChanged<X>(Expression<Func<X>> expression)
        {
            var propertyName = __GetPropertyName(expression);
            __OnPropertyChanged(propertyName);
        }
    }
}